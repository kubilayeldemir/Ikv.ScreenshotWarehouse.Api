using System.ComponentModel.DataAnnotations;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Entities
{
    public class PostRawData : DateField
    {
        [Key]
        public long Id { get; set; }
        public string FileBase64 { get; set; }
        public string FileMd5 { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}