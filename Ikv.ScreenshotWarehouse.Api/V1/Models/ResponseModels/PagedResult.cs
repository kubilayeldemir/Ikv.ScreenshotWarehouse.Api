using System.Collections.Generic;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Data { get; set; }

        public PagedResult()
        {
            Data = new List<T>();
        }
    }
}