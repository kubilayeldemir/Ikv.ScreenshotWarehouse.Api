using System.Collections.Generic;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;

namespace Ikv.ScreenshotWarehouse.Api.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(string postId);
        Task<Post> SavePost(Post post);
        Task<List<Post>> SavePostBulk(List<Post> posts);
    }
}