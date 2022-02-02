using FluentValidation;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.Validations
{
    public class UserLoginRequestModelValidation : AbstractValidator<UserLoginRequestModel>
    {
        public UserLoginRequestModelValidation()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı alanı boş olamaz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş olamaz.");
        }
    }
}