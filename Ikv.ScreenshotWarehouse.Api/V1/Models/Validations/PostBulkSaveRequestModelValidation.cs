using System.Collections.Generic;
using FluentValidation;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.Validations
{
    public class PostBulkSaveRequestModelValidation : AbstractValidator<List<PostBulkSaveRequestModel>>
    {
        public PostBulkSaveRequestModelValidation()
        {
            RuleForEach(x => x)
                .Must(x => x.GameServer == null || GameServers.ListOfServers.Contains(x.GameServer))
                .WithMessage("Oyun serveri yanlış.");
        }
    }
}