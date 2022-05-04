using System;
using System.IO;

namespace Ikv.ScreenshotWarehouse.Api.Helpers
{
    public static class FileSaveHelper
    {
        public static void SaveFile(string filename, string fileAsBase64)
        {
            string filePath = $"./files/{filename}";
            File.WriteAllBytes(filePath, Convert.FromBase64String(fileAsBase64));
        }
    }
}