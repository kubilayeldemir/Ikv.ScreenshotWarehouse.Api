namespace Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels
{
    public class PostCreateRequestModel
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string GameMap { get; set; }
        public string FileBase64 { get; set; }
        public string FileName { get; set; }
    }
}