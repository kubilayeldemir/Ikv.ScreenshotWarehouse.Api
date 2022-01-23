using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.Services
{
    public interface IUserService
    {
        Task<(bool isAuthenticated,User user)> AuthenticateUser(string username, string password);
        Task<string> GenerateJwtTokenForUser(User user);
        Task<User> RegisterUser(UserRegisterRequestModel userRegisterModel);
    }
}