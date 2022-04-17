using System;
using System.ComponentModel.DataAnnotations;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Entities
{
    public class Post : DateField
    {
        [Key]
        public string Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string FileURL { get; set; }
        public string Md5 { get; set; }
        public DateTime ScreenshotDate { get; set; }
        public string GameMap { get; set; }
        public bool IsValidated { get; set; }
        public User User { get; set; }
        public string Username { get; set; }
        public string GameServer { get; set; }
        public long UserId { get; set; }
        public PostRawData PostRawData { get; set; }
    }
}