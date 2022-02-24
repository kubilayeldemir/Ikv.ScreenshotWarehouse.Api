using System;
using System.ComponentModel.DataAnnotations;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Entities
{
    public class VideoPost : DateField
    {
        [Key]
        public string Id { get; set; }
        public string VideoUrl { get; set; }
        public string OriginalTitle { get; set; }
        public string Title { get; set; }
        public bool IsValidated { get; set; }
        public DateTime OriginalUploadDate { get; set; }
        public string Username { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}