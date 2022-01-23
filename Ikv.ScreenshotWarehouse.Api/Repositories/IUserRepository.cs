using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistent.Entities;

namespace Ikv.ScreenshotWarehouse.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<User> SaveUser(User user);
    }
}