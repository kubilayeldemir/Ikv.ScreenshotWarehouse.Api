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
using Microsoft.IdentityModel.Tokens;

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
            if (model.FileBase64.IsNullOrEmpty())
            {
                return null;
            }

            var url = await _cloudinaryHelper.UploadBase64Image(model.FileBase64, userId.ToString());
            post.FileURL = url;
            post.ScreenshotDate = ParseScreenshotDateFromFileName(model.FileName);
            post.Username = await _userRepository.GetUsernameOfUserFromUserId(userId);
            return await _postRepository.SavePost(post);
        }

        public async Task<List<PostBulkSaveResponseModel>> BulkSaveScreenshots(
            List<PostBulkSaveRequestModel> postModels, long userId)
        {
            var username = await _userRepository.GetUsernameOfUserFromUserId(userId);
            var existingPostsMd5List = await _postRepository.CheckPostExistsByMd5(postModels
                .Select(p => Md5Helper.CreateMd5Checksum(p.FileBase64)).ToList());
            var imageUploadTasks = new List<Task<(string postId, string url)>>();
            var uploadedPosts = new List<Post>();
            var uploadFailedPosts = new List<Post>();
            var duplicatePostsOnRequest = new List<Post>();
            var postsToUpload = new List<Post>();
            var nonValidPosts = new List<Post>();
            var existingPostsOnDb = new List<Post>();

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
                    ScreenshotDate = ParseScreenshotDateFromFileName(model.FileName),
                    Md5 = Md5Helper.CreateMd5Checksum(model.FileBase64)
                };
                if (model.FileBase64.IsNullOrEmpty())
                {
                    nonValidPosts.Add(post);
                    continue;
                }

                var duplicateImageCount = postsToUpload.Count(p => p.Md5 == post.Md5);

                if (duplicateImageCount > 0)
                {
                    duplicatePostsOnRequest.Add(post);
                    continue;
                }

                if (existingPostsMd5List.Contains(post.Md5))
                {
                    existingPostsOnDb.Add(post);
                    continue;
                }

                var imageUploadTask =
                    _cloudinaryHelper.UploadBase64ImageParallel(post.Id, model.FileBase64, userId.ToString());
                imageUploadTasks.Add(imageUploadTask);
                postsToUpload.Add(post);
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
                    uploadFailedPosts.Add(post);
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
                    FileUrl = p.FileURL
                });
            });

            uploadFailedPosts.ForEach(p =>
            {
                responseModel.Add(PostBulkSaveResponseModel.CreateFailedPostResponseModel(4001,
                    "Dosya buluta yüklenemedi.(Cloudinary error)"));
            });

            duplicatePostsOnRequest.ForEach(p =>
            {
                responseModel.Add(PostBulkSaveResponseModel.CreateFailedPostResponseModel(4002,
                    "Bu dosya tekrarlandığı için yüklenmedi."));
            });

            nonValidPosts.ForEach(p =>
            {
                responseModel.Add(PostBulkSaveResponseModel.CreateFailedPostResponseModel(4003,
                    "Dosya bulunamadı."));
            });
            
            existingPostsOnDb.ForEach(p =>
            {
                responseModel.Add(PostBulkSaveResponseModel.CreateFailedPostResponseModel(4004,
                    "Bu dosyanın aynısı sistemde zaten bulunduğu için yüklenmedi."));
            });

            return responseModel;
        }

        public async Task<List<Post>> ValidatePosts(List<string> postIds)
        {
            var postsToValidate = await _postRepository.GetPostsByIdsBulk(postIds);
            postsToValidate.ForEach(p => p.IsValidated = true);
            return await _postRepository.UpdatePosts(postsToValidate);
        }

        public async Task<List<Post>> SearchPosts(PostSearchRequestModel model)
        {
            return await _postRepository.SearchPosts(model);
        }

        public async Task<PagedResult<Post>> SearchPostsPaged(PostSearchRequestModel model,
            PagingRequestModel pagingModel)
        {
            return await _postRepository.SearchPostsPaged(model, pagingModel);
        }

        private static DateTime ParseScreenshotDateFromFileName(string fileName)
            //Example file name with date:  EKRAN_01-15-22_00.40.28.JPG -> real date -> 15.01.2022 00:40
            //Structure: Month-Day-Year's last 2 digit-Hour-Minute-Second
        {
            var originalFileName = fileName;
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