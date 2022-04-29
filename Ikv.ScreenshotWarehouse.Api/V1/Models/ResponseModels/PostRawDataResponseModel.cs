namespace Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels
{
    public class PostRawDataResponseModel
    {
        public long Id { get; set; }
        public string FileBase64 { get; set; }
        public string FileMd5 { get; set; }
        public string PostId { get; set; }
    }
}