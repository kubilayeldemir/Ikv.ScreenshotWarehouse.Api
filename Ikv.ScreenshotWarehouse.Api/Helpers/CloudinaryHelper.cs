﻿using System;
using System.Net;
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
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env != null && env == "Development")
            {
                folderName = "development-" + folderName;
            }
            var imgUploadParams = new ImageUploadParams()
            {
                File = new FileDescription(base64),
                Folder = folderName
            };
            var imgUploadResult = await _cloudinary.UploadAsync(imgUploadParams);
            Console.WriteLine(imgUploadResult.StatusCode);
            if (imgUploadResult.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            return imgUploadResult.SecureUrl.ToString();
        }
        
        public async Task<(string postId,string url)> UploadBase64ImageParallel(string postId, string base64, string folderName)
        {
            var url = await UploadBase64Image(base64, folderName);
            return (postId, url);
        }
    }
}