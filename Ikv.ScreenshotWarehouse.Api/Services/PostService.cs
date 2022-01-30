using System;
using System.Collections.Generic;
using System.Linq;
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


        public PostService(IPostRepository postRepository, CloudinaryHelper cloudinaryHelper,
            IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _cloudinaryHelper = cloudinaryHelper;
            _userRepository = userRepository;
        }

        public async Task<Post> GetPostById(string postId)
        {
            return await _postRepository.GetPostById(postId);
        }

        public async Task<Post> SaveScreenshot(PostSaveRequestModel model, long userId)
        {
            var post = new Post
            {
                Id = ShortGuid.NewGuid(),
                Category = model.Category,
                Title = model.Title,
                GameMap = model.GameMap,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var url = await _cloudinaryHelper.UploadBase64Image(model.FileBase64, userId.ToString());
            post.FileURL = url;
            post.ScreenshotDate = ParseScreenshotDateFromFileName(model.FileName);
            post.Username = await _userRepository.GetUsernameOfUserFromUserId(userId);
            return await _postRepository.SavePost(post);
        }

        public async Task<List<PostBulkSaveResponseModel>> BulkSaveScreenshots(List<PostBulkSaveRequestModel> postModels, long userId)
        {
            var username = await _userRepository.GetUsernameOfUserFromUserId(userId);
            var imageUploadTasks = new List<Task<(string postId, string url)>>();
            var uploadedPosts = new List<Post>();
            var failedPosts = new List<Post>();
            var postsToUpload = new List<Post>();
            foreach (var model in postModels)
            {
                var post = new Post
                {
                    Id = ShortGuid.NewGuid(),
                    Category = model.Category,
                    Title = model.Title,
                    GameMap = model.GameMap,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Username = username,
                    ScreenshotDate = ParseScreenshotDateFromFileName(model.FileName)
                };
                var imageUploadTask =  _cloudinaryHelper.UploadBase64ImageParallel(post.Id, model.FileBase64, userId.ToString());
                imageUploadTasks.Add(imageUploadTask);
                postsToUpload.Add(post);
                //TODO check if post is uploaded Create response model
            }

            var uploadTaskResponses = await Task.WhenAll(imageUploadTasks);

            foreach (var (postId, url) in uploadTaskResponses)
            {
                var post = postsToUpload.FirstOrDefault(p => p.Id == postId);
                if (post == null) continue;
                if (url != null)
                {
                    post.FileURL = url;
                    uploadedPosts.Add(post);
                }
                else
                {
                    failedPosts.Add(post);
                }
            }
            
            var savedPosts = await _postRepository.SavePostBulk(uploadedPosts);

            var responseModel = new List<PostBulkSaveResponseModel>();
            
            savedPosts.ForEach(p =>
            {
             responseModel.Add(new PostBulkSaveResponseModel
             {
                 Id = p.Id,
                 IsOk = true,
                 Error = null,
                 ErrorCode = 0,
                 IsDuplicate = false,
                 FileUrl = p.FileURL
             });   
            });
            
            failedPosts.ForEach(p =>
            {
                responseModel.Add(new PostBulkSaveResponseModel
                {
                    Id = null,
                    IsOk = false,
                    Error = "Dosya buluta yüklenemedi.(Cloudinary error)",
                    ErrorCode = 4001,
                    IsDuplicate = false
                });
            });
            return responseModel;
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