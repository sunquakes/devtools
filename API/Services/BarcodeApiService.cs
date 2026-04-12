using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DevTools.API.Models;
using ZXing;
using ZXing.Common;

namespace DevTools.API.Services
{
    public class BarcodeApiService : IBarcodeApiService
    {
        public BarcodeResult GenerateBarcode(string text, string format = "CODE_128", int width = 400, int height = 120)
        {
            try
            {
                var barcodeFormat = ParseBarcodeFormat(format);
                var writer = new BarcodeWriterPixelData
                {
                    Format = barcodeFormat,
                    Options = new EncodingOptions
                    {
                        Height = height,
                        Width = width,
                        Margin = 10
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

                return new BarcodeResult
                {
                    Input = text,
                    ImageBase64 = base64,
                    Format = format,
                    Width = width,
                    Height = height
                };
            }
            catch (Exception ex)
            {
                return new BarcodeResult
                {
                    Input = text,
                    ImageBase64 = string.Empty,
                    Format = format,
                    Width = width,
                    Height = height
                };
            }
        }

        public List<string> GetSupportedFormats()
        {
            return new List<string>
            {
                "CODE_128",
                "CODE_39",
                "CODE_93",
                "CODABAR",
                "EAN_8",
                "EAN_13",
                "UPC_A",
                "UPC_E",
                "ITF",
                "DATA_MATRIX",
                "PDF_417"
            };
        }

        private BarcodeFormat ParseBarcodeFormat(string format)
        {
            return format.ToUpperInvariant() switch
            {
                "CODE_128" => BarcodeFormat.CODE_128,
                "CODE_39" => BarcodeFormat.CODE_39,
                "CODE_93" => BarcodeFormat.CODE_93,
                "CODABAR" => BarcodeFormat.CODABAR,
                "EAN_8" => BarcodeFormat.EAN_8,
                "EAN_13" => BarcodeFormat.EAN_13,
                "UPC_A" => BarcodeFormat.UPC_A,
                "UPC_E" => BarcodeFormat.UPC_E,
                "ITF" => BarcodeFormat.ITF,
                "DATA_MATRIX" => BarcodeFormat.DATA_MATRIX,
                "PDF_417" => BarcodeFormat.PDF_417,
                _ => BarcodeFormat.CODE_128
            };
        }
    }
}
