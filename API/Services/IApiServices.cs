using System.Collections.Generic;
using DevTools.API.Models;

namespace DevTools.API.Services
{
    public interface IMd5ApiService
    {
        Md5Result ComputeMd5(string input);
    }

    public interface IJsonApiService
    {
        JsonFormatResult FormatJson(string json);
        bool ValidateJson(string json);
    }

    public interface IUrlEncodeApiService
    {
        UrlEncodeResult Encode(string input);
        UrlEncodeResult Decode(string input);
    }

    public interface IEscapeApiService
    {
        EscapeResult Escape(string input);
        EscapeResult Unescape(string input);
    }

    public interface IBase64ApiService
    {
        Base64EncodeResult EncodeToBase64(string input, bool includeDataUri = false, string? mimeType = null);
        Base64DecodeResult DecodeFromBase64(string base64);
        string ImageToBase64(string imagePath, bool includeDataUri = false);
        string Base64ToImage(string base64, string outputPath);
    }

    public interface IQrCodeApiService
    {
        QrCodeResult GenerateQrCode(string text, int width = 300, int height = 300);
    }

    public interface IBarcodeApiService
    {
        BarcodeResult GenerateBarcode(string text, string format = "CODE_128", int width = 400, int height = 120);
        List<string> GetSupportedFormats();
    }

    public interface ISignatureApiService
    {
        SignatureResult CreateSignatureFromStrokes(List<StrokePoint> strokes, int width, int height, double penWidth);
    }

    public class StrokePoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsDown { get; set; }
    }
}
