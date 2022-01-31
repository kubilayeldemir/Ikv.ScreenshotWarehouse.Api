using System.Collections.Generic;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels;

namespace Ikv.ScreenshotWarehouse.Api.Services
{
    public interface IPostService
    {
        Task<Post> GetPostById(string postId);
        Task<Post> SaveScreenshot(PostSaveRequestModel post, long userId);
        Task<List<PostBulkSaveResponseModel>> BulkSaveScreenshots(List<PostBulkSaveRequestModel> posts, long userId);
        Task<List<Post>> ValidatePosts(List<string> postIds);
        Task<List<Post>> SearchPosts(PostSearchRequestModel model);
        Task<PagedResult<Post>> SearchPostsPaged(PostSearchRequestModel model, PagingRequestModel pagingModel);
    }
}