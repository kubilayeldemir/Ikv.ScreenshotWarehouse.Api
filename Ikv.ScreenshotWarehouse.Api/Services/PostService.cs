using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpVitamins;
using Ikv.ScreenshotWarehouse.Api.Helpers;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.Repositories;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels;

namespace Ikv.ScreenshotWarehouse.Api.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly CloudinaryHelper _cloudinaryHelper;
        private readonly IUserRepository _userRepository;


        public PostService(IPostRepository postRepository, CloudinaryHelper cloudinaryHelper, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _cloudinaryHelper = cloudinaryHelper;
            _userRepository = userRepository;
        }

        public async Task<Post> GetPostById(string postId)
        {
            return await _postRepository.GetPostById(postId);
        }

        public async Task<Post> SaveScreenshot(PostCreateRequestModel model, long userId)
        {
            var post = new Post
            {
                Category = model.Category,
                Title = model.Title,
                GameMap = model.GameMap,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            post.Id = ShortGuid.NewGuid();
            var url = await _cloudinaryHelper.UploadBase64Image(model.FileBase64, userId.ToString());
            post.FileURL = url;
            post.ScreenshotDate = ParseScreenshotDateFromFileName(model.FileName);
            post.Username = await _userRepository.GetUsernameOfUserFromUserId(userId);
            return await _postRepository.SavePost(post);
        }

        public async Task<List<Post>> SearchPosts(PostSearchRequestModel model)
        {
            return await _postRepository.SearchPosts(model);
        }
        
        public async Task<PagedResult<Post>> SearchPostsPaged(PostSearchRequestModel model, PagingRequestModel pagingModel )
        {
            return await _postRepository.SearchPostsPaged(model, pagingModel);
        }

        private static DateTime ParseScreenshotDateFromFileName(string fileName)
        {
            var originalFileName = fileName;
            //Example file name with date:  EKRAN_01-15-22_00.40.28.JPG -> real date -> 15.01.2022 00:40
            //Structure: Month-Day-Year's last 2 digit-Hour-Minute-Second
            try
            {
                if (fileName.EndsWith(".JPG") || fileName.EndsWith(".BMP") || fileName.EndsWith("JPEG"))
                {
                    fileName = fileName[..fileName.LastIndexOf('.')];
                }
                
                fileName = fileName[6..]; //Remove useless part: "EKRAN_"

                var fileNamesDateAndHourSplitted = fileName.Split('_');
                var datePart = fileNamesDateAndHourSplitted[0];
                var timePart = fileNamesDateAndHourSplitted[1];

                var datePartSplit = datePart.Split('-');
                var timePartSplit = timePart.Split('.');

                var dateTime = new DateTime();

                dateTime = dateTime.AddYears(1999 + int.Parse(datePartSplit[2]));
                dateTime = dateTime.AddMonths(int.Parse(datePartSplit[0]) - 1);
                dateTime = dateTime.AddDays(int.Parse(datePartSplit[1]) - 1);

                dateTime = dateTime.AddHours(int.Parse(timePartSplit[0]));
                dateTime = dateTime.AddMinutes(int.Parse(timePartSplit[1]));
                dateTime = dateTime.AddSeconds(int.Parse(timePartSplit[2]));

                return dateTime;
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't parse dateTime of file" + originalFileName);
                return DateTime.Now;
            }
        }
    }
}