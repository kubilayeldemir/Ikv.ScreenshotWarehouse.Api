using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Ikv.ScreenshotWarehouse.Api.Helpers
{
    public class CloudinaryHelper
    {
        private readonly string _cloudinaryCloudName = Environment.GetEnvironmentVariable("CloudinaryCloudName");
        private readonly string _cloudinaryApiKey = Environment.GetEnvironmentVariable("CloudinaryApiKey");
        private readonly string _cloudinaryApiSecret = Environment.GetEnvironmentVariable("CloudinaryApiSecret");

        private readonly Account _account;
        private readonly Cloudinary _cloudinary;

        public CloudinaryHelper()
        {
            _account = new Account(_cloudinaryCloudName, _cloudinaryApiKey, _cloudinaryApiSecret);
            _cloudinary = new Cloudinary(_account);
            _cloudinary.Api.Timeout = int.MaxValue;
        }

        public async Task<string> UploadBase64Image(string base64, string folderName)
        {
            var imgUploadParams = new ImageUploadParams()
            {
                File = new FileDescription(base64),
                Folder = folderName
            };
            var imgUploadResult = await _cloudinary.UploadAsync(imgUploadParams);
            return imgUploadResult.SecureUrl.ToString();
        }
    }
}