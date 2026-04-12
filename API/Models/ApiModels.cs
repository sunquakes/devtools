using System;
using System.Collections.Generic;

namespace DevTools.API.Models
{
    public class Md5Result
    {
        public string Input { get; set; } = string.Empty;
        public string Hash32Lower { get; set; } = string.Empty;
        public string Hash32Upper { get; set; } = string.Empty;
        public string Hash16Lower { get; set; } = string.Empty;
        public string Hash16Upper { get; set; } = string.Empty;
    }

    public class JsonFormatResult
    {
        public string Input { get; set; } = string.Empty;
        public string Formatted { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class UrlEncodeResult
    {
        public string Input { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
    }

    public class EscapeResult
    {
        public string Input { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
    }

    public class Base64EncodeResult
    {
        public string Input { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public bool IncludeDataUri { get; set; }
        public string? MimeType { get; set; }
    }

    public class Base64DecodeResult
    {
        public string Input { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public string? MimeType { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
    }

    public class QrCodeResult
    {
        public string Input { get; set; } = string.Empty;
        public string ImageBase64 { get; set; } = string.Empty;
        public string Format { get; set; } = "PNG";
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class BarcodeResult
    {
        public string Input { get; set; } = string.Empty;
        public string ImageBase64 { get; set; } = string.Empty;
        public string Format { get; set; } = "CODE_128";
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class SignatureResult
    {
        public string ImageBase64 { get; set; } = string.Empty;
        public string Format { get; set; } = "PNG";
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class ImageInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public string? MimeType { get; set; }
    }
}
