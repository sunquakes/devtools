using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using DevTools.API.Models;

namespace DevTools.API.Services
{
    public class Base64ApiService : IBase64ApiService
    {
        public Base64EncodeResult EncodeToBase64(string input, bool includeDataUri = false, string? mimeType = null)
        {
            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var base64 = Convert.ToBase64String(bytes);

                if (includeDataUri)
                {
                    var mime = mimeType ?? "text/plain";
                    base64 = $"data:{mime};base64,{base64}";
                }

                return new Base64EncodeResult
                {
                    Input = input,
                    Output = base64,
                    Operation = "encode",
                    IncludeDataUri = includeDataUri,
                    MimeType = mimeType
                };
            }
            catch (Exception ex)
            {
                return new Base64EncodeResult
                {
                    Input = input,
                    Output = string.Empty,
                    Operation = "encode",
                    IncludeDataUri = includeDataUri,
                    MimeType = mimeType
                };
            }
        }

        public Base64DecodeResult DecodeFromBase64(string base64)
        {
            try
            {
                var cleanBase64 = base64;
                string? mimeType = null;

                if (base64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    var commaIndex = base64.IndexOf(',');
                    if (commaIndex >= 0)
                    {
                        var header = base64.Substring(0, commaIndex);
                        var semiIndex = header.IndexOf(';');
                        if (semiIndex > 5)
                        {
                            mimeType = header.Substring(5, semiIndex - 5);
                        }
                        cleanBase64 = base64.Substring(commaIndex + 1);
                    }
                }

                var bytes = Convert.FromBase64String(cleanBase64);
                var output = System.Text.Encoding.UTF8.GetString(bytes);

                int width = 0;
                int height = 0;

                try
                {
                    using var ms = new MemoryStream(bytes);
                    using var bmp = new Bitmap(ms);
                    width = bmp.Width;
                    height = bmp.Height;
                }
                catch
                {
                }

                return new Base64DecodeResult
                {
                    Input = base64,
                    Output = output,
                    MimeType = mimeType,
                    ImageWidth = width,
                    ImageHeight = height
                };
            }
            catch (Exception ex)
            {
                return new Base64DecodeResult
                {
                    Input = base64,
                    Output = string.Empty,
                    MimeType = null,
                    ImageWidth = 0,
                    ImageHeight = 0
                };
            }
        }

        public string ImageToBase64(string imagePath, bool includeDataUri = false)
        {
            var bytes = File.ReadAllBytes(imagePath);
            var base64 = Convert.ToBase64String(bytes);

            if (includeDataUri)
            {
                var ext = Path.GetExtension(imagePath).ToLowerInvariant();
                var mime = ext switch
                {
                    ".png" => "image/png",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".bmp" => "image/bmp",
                    ".gif" => "image/gif",
                    ".webp" => "image/webp",
                    _ => "application/octet-stream"
                };
                base64 = $"data:{mime};base64,{base64}";
            }

            return base64;
        }

        public string Base64ToImage(string base64, string outputPath)
        {
            var cleanBase64 = base64;
            if (base64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                var commaIndex = base64.IndexOf(',');
                if (commaIndex >= 0)
                {
                    cleanBase64 = base64.Substring(commaIndex + 1);
                }
            }

            var bytes = Convert.FromBase64String(cleanBase64);
            File.WriteAllBytes(outputPath, bytes);

            return outputPath;
        }
    }
}
