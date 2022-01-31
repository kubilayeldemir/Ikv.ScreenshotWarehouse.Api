namespace Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels
{
    public class PostBulkSaveResponseModel
    {
        public string Id { get; set; }
        public bool IsOk { get; set; }
        public int ErrorCode { get; set; }
        public string Error { get; set; }
        public string FileUrl { get; set; }

        public static PostBulkSaveResponseModel CreateFailedPostResponseModel(int errorCode, string errorMessage)
        {
            return new PostBulkSaveResponseModel
            {
                Id = null,
                IsOk = false,
                Error = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}