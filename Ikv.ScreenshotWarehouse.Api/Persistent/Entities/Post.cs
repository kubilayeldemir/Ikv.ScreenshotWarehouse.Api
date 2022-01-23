using System;
using System.ComponentModel.DataAnnotations;

namespace Ikv.ScreenshotWarehouse.Api.Persistent.Entities
{
    public class Post : DateField
    {
        [Key]
        public string Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public DateTime ScreenshotDate { get; set; }
        public string GameMap { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        
    }
}