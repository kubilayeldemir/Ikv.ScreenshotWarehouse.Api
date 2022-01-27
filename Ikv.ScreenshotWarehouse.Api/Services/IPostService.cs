using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.Services
{
    public interface IPostService
    {
        Task<Post> GetPostById(string postId);
        Task<Post> SaveScreenshot(PostCreateRequestModel post, long userId);
    }
}