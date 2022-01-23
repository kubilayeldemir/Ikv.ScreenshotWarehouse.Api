namespace Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels
{
    public class UserResponseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public UserResponseModel()
        {
            
        }
        public UserResponseModel(string username, string email, string role)
        {
            Username = username;
            Email = email;
            Role = role;
        }
    }
}