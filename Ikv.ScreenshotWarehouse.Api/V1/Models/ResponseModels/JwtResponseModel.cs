using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels
{
    public class JwtResponseModel
    {
        public UserResponseModel User { get; set; }
        public string Jwt { get; set; }

        public JwtResponseModel(User user, string jwt)
        {
            User = new UserResponseModel(user.Username, user.Email, user.Role);
            Jwt = jwt;
        }
    }
}