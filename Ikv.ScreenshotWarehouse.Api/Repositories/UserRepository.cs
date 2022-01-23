using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Contexts;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ikv.ScreenshotWarehouse.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IkvContext _ikvContext;

        public UserRepository(IkvContext ikvContext)
        {
            _ikvContext = ikvContext;
        }

        public Task<User> GetUserByUsername(string username)
        {
            return _ikvContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> SaveUser(User user)
        {
            await _ikvContext.Users.AddAsync(user);
            await _ikvContext.SaveChangesAsync();
            return user;
        }
    }
}