using System.Collections.Generic;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels;

namespace Ikv.ScreenshotWarehouse.Api.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(string postId);
        Task<Post> SavePost(Post post);
        Task<List<Post>> SavePostBulk(List<Post> posts);
        Task<List<Post>> SearchPosts(PostSearchRequestModel model);
        Task<PagedResult<Post>> SearchPostsPaged(PostSearchRequestModel model, PagingRequestModel pagingModel);
    }
}