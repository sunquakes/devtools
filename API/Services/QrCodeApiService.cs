using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DevTools.API.Models;
using ZXing;
using ZXing.Common;

namespace DevTools.API.Services
{
    public class QrCodeApiService : IQrCodeApiService
    {
        public QrCodeResult GenerateQrCode(string text, int width = 300, int height = 300)
        {
            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = 1
                    }
                };

                var pixelData = writer.Write(text);

                using var bmp = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
                var bitmapData = bmp.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bmp.UnlockBits(bitmapData);
                }

                using var ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                var base64 = Convert.ToBase64String(ms.ToArray());

                return new QrCodeResult
                {
                    Input = text,
                    ImageBase64 = base64,
                    Format = "PNG",
                    Width = width,
                    Height = height
                };
            }
            catch (Exception ex)
            {
                return new QrCodeResult
                {
                    Input = text,
                    ImageBase64 = string.Empty,
                    Format = "PNG",
                    Width = width,
                    Height = height
                };
            }
        }
    }
}
