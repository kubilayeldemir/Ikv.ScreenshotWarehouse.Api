﻿using System;
using System.Collections.Generic;
using System.IO;
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
                GameServer = model.GameServer ?? GameServers.Diger,
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

        public async Task<List<PostBulkSaveResponseModel>> BulkSaveScreenshots(List<PostBulkSaveRequestModel> postModels, long userId)
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
            var fileSizeNotValidPosts = new List<Post>();
            var fileTypeNotValidPosts = new List<Post>();

            foreach (var model in postModels)
            {
                var post = new Post
                {
                    Id = ShortGuid.NewGuid(),
                    Category = "user",
                    Title = model.Title,
                    GameMap = model.GameMap,
                    GameServer = model.GameServer ?? GameServers.Diger,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Username = username,
                    ScreenshotDate = ParseScreenshotDateFromFileName(model.FileName),
                    Md5 = Md5Helper.CreateMd5Checksum(model.FileBase64)
                };
                post.PostRawData = new PostRawData
                {
                    PostId = post.Id,
                    FileBase64 = model.FileBase64,
                    FileMd5 = post.Md5,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt
                };
                if (model.FileBase64.IsNullOrEmpty())
                {
                    nonValidPosts.Add(post);
                    continue;
                }

                if (!IsBase64FileTypeValid(model.FileBase64))
                {
                    fileTypeNotValidPosts.Add(post);
                    continue;
                }

                var byteSizeOfFile = System.Text.Encoding.ASCII.GetByteCount(model.FileBase64);
                if (byteSizeOfFile > 1000*1000)
                {
                    fileSizeNotValidPosts.Add(post);
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
                    try
                    {
                        var filename = $"{post.Id}.png";
                        FileSaveHelper.SaveFile(filename, post.PostRawData.FileBase64.Split(',')[1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("File save failed | " + e);
                    }
                }
                else
                {
                    uploadFailedPosts.Add(post);
                }
            }

            var savedPosts = new List<Post>();

            if (uploadedPosts.Any())
            {
                savedPosts = await _postRepository.SavePostBulk(uploadedPosts);

            }

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

            #region errorMapping
         
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
            
            fileSizeNotValidPosts.ForEach(p =>
            {
                responseModel.Add(PostBulkSaveResponseModel.CreateFailedPostResponseModel(4005,
                    "Dosyanın boyutu belirlenen maksimum boyuttan büyük olduğu için yüklenmedi."));
            });
            
            fileTypeNotValidPosts.ForEach(p =>
            {
                responseModel.Add(PostBulkSaveResponseModel.CreateFailedPostResponseModel(4006,
                    "Yüklediğiniz dosyanın tipi uygun olmadığı için yüklenmedi.(Sisteme JPG JPEG PNG dosyalar yüklenebilir"));
            });
            
            #endregion

            return responseModel;
        }
        
        public async Task<List<PostBulkSaveResponseModel>> BulkSaveRawImages(List<RawPostBulkSaveRequestModel> postModels, long userId)
        {
            var existingPostsMd5List = await _postRepository.CheckPostExistsByMd5(postModels
                .Select(p => Md5Helper.CreateMd5Checksum(p.FileBase64)).ToList());
            var duplicatePostsOnRequest = new List<Post>();
            var nonValidPosts = new List<Post>();
            var existingPostsOnDb = new List<Post>();
            var fileSizeNotValidPosts = new List<Post>();
            var postsToSave = new List<Post>();

            foreach (var model in postModels)
            {
                var post = new Post
                {
                    Id = ShortGuid.NewGuid(),
                    Category = "forum",
                    Title = model.Title,
                    GameServer = GameServers.Diger,
                    UserId = userId,
                    IsValidated = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Username = model.Username,
                    ScreenshotDate = model.Timestamp,
                    Md5 = Md5Helper.CreateMd5Checksum(model.FileBase64)
                };
                // post.PostRawData = new PostRawData
                // {
                //     PostId = post.Id,
                //     FileBase64 = model.FileBase64,
                //     FileMd5 = post.Md5,
                //     CreatedAt = post.CreatedAt,
                //     UpdatedAt = post.UpdatedAt
                // };
                if (model.FileBase64.IsNullOrEmpty())
                {
                    nonValidPosts.Add(post);
                    continue;
                }

                var byteSizeOfFile = System.Text.Encoding.ASCII.GetByteCount(model.FileBase64);
                if (byteSizeOfFile > 5000*1000)
                {
                    fileSizeNotValidPosts.Add(post);
                    continue;
                }

                var duplicateImageCount = postsToSave.Count(p => p.Md5 == post.Md5);

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

                try
                {
                    var filename = $"{post.Id}.{model.FileType}";
                    FileSaveHelper.SaveFile(filename, model.FileBase64.Split(',')[1]);
                    post.FileURL = filename;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }

                postsToSave.Add(post);
            }

            var savedPosts = new List<Post>();

            if (postsToSave.Any())
            {
                savedPosts = await _postRepository.SavePostBulk(postsToSave);
            }
            

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
            
            #region errorMapping
            
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
            
            fileSizeNotValidPosts.ForEach(p =>
            {
                responseModel.Add(PostBulkSaveResponseModel.CreateFailedPostResponseModel(4005,
                    "Dosyanın boyutu belirlenen maksimum boyuttan büyük olduğu için yüklenmedi."));
            });
            
            #endregion

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
        
        public async Task<PagedResult<Post>> GetRandomPosts()
        {
            var posts = await _postRepository.GetRandomPosts();
            return new PagedResult<Post>
            {
                Data = posts,
                PageSize = posts.Count,
                PageCount = 1,
                RowCount = 0,
                CurrentPage = 1
            };
        }


        private bool IsBase64FileTypeValid(string base64String)
        {
            var strings = base64String.Split(",");
            var isValid = false;
            string extension;
            switch (strings[0]) {
                case "data:image/jpeg;base64":
                    extension = "jpeg";
                    isValid = true;
                    break;
                case "data:image/jpg;base64":
                    extension = "jpeg";
                    isValid = true;
                    break;
                case "data:image/png;base64":
                    extension = "png";
                    isValid = true;
                    break;
                case "data:image/bmp;base64":
                    extension = "bmp";
                    isValid = true;
                    break;
            }
            return isValid;
        }

        private static DateTime ParseScreenshotDateFromFileName(string fileName)
            //Example file name with date:  EKRAN_01-15-22_00.40.28.JPG -> real date -> 15.01.2022 00:40
            //Structure: Month-Day-Year's last 2 digit-Hour-Minute-Second
        {
            var originalFileName = fileName;
            try
            {
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