using System.Security.Cryptography;
using System.Text;
using DevTools.API.Models;

namespace DevTools.API.Services
{
    public class Md5ApiService : IMd5ApiService
    {
        public Md5Result ComputeMd5(string input)
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            var hexLower = sb.ToString();
            var hexUpper = hexLower.ToUpperInvariant();

            string mid16Lower = string.Empty;
            string mid16Upper = string.Empty;

            if (hexLower.Length >= 24)
            {
                mid16Lower = hexLower.Substring(8, 16);
                mid16Upper = mid16Lower.ToUpperInvariant();
            }

            return new Md5Result
            {
                Input = input,
                Hash32Lower = hexLower,
                Hash32Upper = hexUpper,
                Hash16Lower = mid16Lower,
                Hash16Upper = mid16Upper
            };
        }
    }
}
