using System.Linq;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Services;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Ikv.ScreenshotWarehouse.Api.V1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] string postId)
        {
            var post = await _postService.GetPostById(postId);
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> SavePost([FromBody] PostCreateRequestModel model)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            var post= await _postService.SaveScreenshot(model, userId == null ? 1 : long.Parse(userId));
            return Ok(post);
        }
    }
}