using System;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Helpers;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.Repositories;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly CloudinaryHelper _cloudinaryHelper;

        public PostService(IPostRepository postRepository, CloudinaryHelper cloudinaryHelper)
        {
            _postRepository = postRepository;
            _cloudinaryHelper = cloudinaryHelper;
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
                UserId = userId
            };
            post.CreatedAt = DateTime.Now;
            post.UpdatedAt = DateTime.Now;
            post.Id = DateTime.Now.ToString(); //TODO use short uuid
            post.FileURL = "NOT IMPLEMENTED YET";
            return await _postRepository.SavePost(post);
        }
    }
}