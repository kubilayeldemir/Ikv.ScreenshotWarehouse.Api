using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Helpers;
using Ikv.ScreenshotWarehouse.Api.Services;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
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
            var postResponseModel = new PostResponseModel(post);
            return Ok(postResponseModel);
        }

        [HttpGet("all")]
        public async Task<IActionResult> SearchPosts([FromQuery] PostSearchRequestModel model)
        {
            var post = await _postService.SearchPosts(model);

            var postResponseModel = post.ConvertAll(p => new PostResponseModel(p));
            return Ok(postResponseModel);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> SearchPostsPaged([FromQuery] PostSearchRequestModel model,
            [FromQuery] PagingRequestModel pagingModel)
        {
            var postsPaged = await _postService.SearchPostsPaged(model, pagingModel);

            var pagedResponseModel = new PagedResult<PostResponseModel>
            {
                CurrentPage = postsPaged.CurrentPage,
                PageCount = postsPaged.PageCount,
                PageSize = postsPaged.PageSize
            };

            pagedResponseModel.Data = postsPaged.Data.ConvertAll(p => new PostResponseModel(p));
            return Ok(pagedResponseModel);
        }
        
        [NoCache]
        [HttpGet("random")]
        public async Task<IActionResult> GetPostsRandom()
        {
            var postsPaged = await _postService.GetRandomPosts();

            var pagedResponseModel = new PagedResult<PostResponseModel>
            {
                CurrentPage = postsPaged.CurrentPage,
                PageCount = postsPaged.PageCount,
                PageSize = postsPaged.PageSize,
                Data = postsPaged.Data.ConvertAll(p => new PostResponseModel(p))
            };

            return Ok(pagedResponseModel);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SavePost([FromBody] PostSaveRequestModel model)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            var post = await _postService.SaveScreenshot(model, userId == null ? 1 : long.Parse(userId));
            if (post == null)
            {
                return BadRequest();
            }

            var postResponseModel = new PostResponseModel(post);
            return Ok(postResponseModel);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkSavePost([FromBody] List<PostBulkSaveRequestModel> models)
        {
            // var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            var postResponseModel =
                await _postService.BulkSaveScreenshots(models, userId == null ? 1 : long.Parse(userId));
            return Ok(postResponseModel);
        }

        [HttpPost("bulk-save-raw")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BulkSaveRawImages([FromBody] List<RawPostBulkSaveRequestModel> models)
        {
            // var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            
            models.ForEach(x =>
            {
                try
                {
                    x.Timestamp = TurkishMonthsHelper.ParseTurkishDatetimeString(x.TimestampString);
                }
                catch (Exception e)
                {
                    x.Timestamp = DateTime.Now;
                    Console.WriteLine("Can't parse this date:" + x.TimestampString);
                }
            });

            var postResponseModel =
                await _postService.BulkSaveRawImages(models, userId == null ? 1 : long.Parse(userId));
            return Ok(postResponseModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("validate")]
        public async Task<IActionResult> ValidatePosts([FromBody] List<string> postIds)
        {
            // var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            var postResponseModel = await _postService.ValidatePosts(postIds);
            return Ok(postResponseModel);
        }
    }
}