using FluentValidation;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.Validations
{
    public class PostSearchRequestModelValidation : AbstractValidator<PostSearchRequestModel>
    {
        public PostSearchRequestModelValidation()
        {
            RuleFor(x => x.GameServer)
                .Must(x => x == null || GameServers.ListOfServers.Contains(x))
                .WithMessage("Oyun serveri doğru değil");
        }
    }
}