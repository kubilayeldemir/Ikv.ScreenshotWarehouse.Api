using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ikv.ScreenshotWarehouse.Api.Helpers
{
    public static class Md5Helper
    {
        public static string CreateMd5Checksum(string input)
        {
            if (input.IsNullOrEmpty())
            {
                return null;
            }
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}