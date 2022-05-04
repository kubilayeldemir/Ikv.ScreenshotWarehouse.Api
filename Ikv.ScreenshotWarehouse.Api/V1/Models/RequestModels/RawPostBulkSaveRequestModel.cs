using System;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels
{
    public class RawPostBulkSaveRequestModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string FileBase64 { get; set; }
        public string FileType { get; set; }
        public string TimestampString { get; set; }
        public DateTime Timestamp { get; set; }
    }
}