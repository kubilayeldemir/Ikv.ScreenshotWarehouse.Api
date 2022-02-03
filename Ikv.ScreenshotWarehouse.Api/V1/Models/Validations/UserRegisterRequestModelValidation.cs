using FluentValidation;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.Validations
{
    public class UserRegisterRequestModelValidation : AbstractValidator<UserRegisterRequestModel>
    {
        public UserRegisterRequestModelValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email alanı boş olamaz.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email yanlış gözüküyor.");
            RuleFor(x => x.Email).MaximumLength(330).WithMessage("Email çok uzun.");
            RuleFor(x => x.Email).MinimumLength(3).WithMessage("Email çok kısa.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı alanı boş olamaz.");
            RuleFor(x => x.Username).MaximumLength(32).WithMessage("Kullanıcı adı 32 karakterden fazla olamaz.");
            RuleFor(x => x.Username).MinimumLength(3).WithMessage("Kullanıcı adı çok kısa.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş olamaz.");
            RuleFor(x => x.Password).MinimumLength(4).WithMessage("Şifre 4 karakterden az olamaz.");
            RuleFor(x => x.Password).MaximumLength(50).WithMessage("Şifre 50 karakterden fazla olamaz.");
        }
    }
}