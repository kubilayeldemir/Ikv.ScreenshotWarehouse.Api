using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Helpers;
using Ikv.ScreenshotWarehouse.Api.Services;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Ikv.ScreenshotWarehouse.Api.V1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly CloudinaryHelper _cloudinaryHelper;
        
        public UserController(IUserService userService, CloudinaryHelper cloudinaryHelper)
        {
            _userService = userService;
            _cloudinaryHelper = cloudinaryHelper;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserLoginRequestModel model)
        {
            var (isAuthenticated, user) = await _userService.AuthenticateUser(model.Username, model.Password);
            if (!isAuthenticated)
            {
                return Unauthorized();
            }
            var jwtToken = await _userService.GenerateJwtTokenForUser(user);
            return Ok(new JwtResponseModel(user, jwtToken));
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestModel model)
        {
            var user = await _userService.RegisterUser(model);
            if (user == null)
            {
                return BadRequest(new
                {
                    Message = $"User with email {model.Email} already exists.",
                    Code = 400001
                });
            }
            return Ok(new UserResponseModel(user.Username, user.Email, user.Role));
        }
        
        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] string base64)
        {
            var url = await _cloudinaryHelper.UploadBase64Image(base64,"test");
            return Ok(url);
        }
    }
}