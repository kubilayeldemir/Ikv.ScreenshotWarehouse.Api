using System;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels
{
    public class PostSearchRequestModel
    {
        public string Username { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string GameMap { get; set; }
        public bool IsValidated { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }
}