using System;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;

namespace Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels
{
    public class PostResponseModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public long UserId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public DateTime ScreenshotDate { get; set; }
        public string GameMap { get; set; }
        public string GameServer { get; set; }
        public bool IsValidated { get; set; }
        public PostRawDataResponseModel RawData { get; set; }

        public PostResponseModel(Post post)
        {
            Id = post.Id;
            Username = post.Username;
            UserId = post.UserId;
            Category = post.Category;
            Title = post.Title;
            FileUrl = post.Category switch
            {
                "user" => "https://res.cloudinary.com/dmo4hvhcj/image/upload/v1645641514/" + post.FileURL,
                "forum" => "https://ikvssapi.tk/" + post.FileURL,
                _ => "https://res.cloudinary.com/dmo4hvhcj/image/upload/v1645876761/web/lazy_dy4ssu.jpg"
            };
            ScreenshotDate = post.ScreenshotDate;
            GameMap = post.GameMap;
            IsValidated = post.IsValidated;
            GameServer = post.GameServer;
            if (post.PostRawData != null)
            {
                RawData = new PostRawDataResponseModel
                {
                    Id = post.PostRawData.Id,
                    FileBase64 = post.PostRawData.FileBase64,
                    FileMd5 = post.PostRawData.FileMd5,
                    PostId = post.PostRawData.PostId
                };
            }
        }
    }
}