using System;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels
{
    public class PostSearchRequestModel
    {
        public string Username { get; set; }
        public long UserId { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string GameMap { get; set; }
        public string GameServer { get; set; }
        public bool IgnoreValidation { get; set; }
        public bool OnlyNonValidatedPosts { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public bool IncludeRawData { get; set; }
        public bool IncludeRawDataIfNeeded { get; set; }
    }
}