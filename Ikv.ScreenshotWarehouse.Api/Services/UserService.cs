using System;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Helpers;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.Repositories;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool isAuthenticated, User user)> AuthenticateUser(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var userSalt = user.Salt;
            var hashedModel = EncryptionHelper.EncryptData(password, userSalt);
            var hashedPassword = hashedModel.hashedData;
            var isCredentialsCorrect = user.CheckIfUserCredentialsCorrect(username, hashedPassword);
            return (isCredentialsCorrect, user);
        }

        public async Task<string> GenerateJwtTokenForUser(User user)
        {
            return JwtHelper.GenerateJwtToken(user);
        }

        public async Task<User> RegisterUser(UserRegisterRequestModel userRegisterModel)
        {
            var (hashedData, salt) = EncryptionHelper.EncryptData(userRegisterModel.Password);
            var user = new User
            {
                Username = userRegisterModel.Username,
                Email = userRegisterModel.Email,
                Password = hashedData,
                Salt = salt,
                Role = "user",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            try
            {
                await _userRepository.SaveUser(user);
            }
            catch (Exception e)
            {
                return null;
            }

            return user;
        }
    }
}