using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DevTools.API.Models;
using DevTools.API.Services;

namespace DevTools.API.Server
{
    public class RequestHandler
    {
        private readonly ApiServerSettings _settings;
        private readonly JsonSerializerOptions _jsonOptions;

        private readonly IMd5ApiService _md5Service;
        private readonly IJsonApiService _jsonService;
        private readonly IUrlEncodeApiService _urlEncodeService;
        private readonly IEscapeApiService _escapeService;
        private readonly IBase64ApiService _base64Service;
        private readonly IQrCodeApiService _qrCodeService;
        private readonly IBarcodeApiService _barcodeService;
        private readonly ISignatureApiService _signatureService;

        public RequestHandler(ApiServerSettings settings)
        {
            _settings = settings;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _md5Service = new Md5ApiService();
            _jsonService = new JsonApiService();
            _urlEncodeService = new UrlEncodeApiService();
            _escapeService = new EscapeApiService();
            _base64Service = new Base64ApiService();
            _qrCodeService = new QrCodeApiService();
            _barcodeService = new BarcodeApiService();
            _signatureService = new SignatureApiService();
        }

        public async Task HandleRequestAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (_settings.EnableCors)
            {
                response.Headers.Add("Access-Control-Allow-Origin", _settings.AllowedOrigins ?? "*");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            }

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }

            var path = request.Url!.AbsolutePath.TrimEnd('/');

            try
            {
                var result = await RouteRequestAsync(path, request);
                response.ContentType = "application/json";
                response.StatusCode = result.Success ? 200 : 400;

                var jsonResponse = JsonSerializer.Serialize(result, _jsonOptions);
                var buffer = Encoding.UTF8.GetBytes(jsonResponse);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                await SendErrorAsync(response, ex.Message);
            }
            finally
            {
                response.Close();
            }
        }

        private async Task<ApiResponse> RouteRequestAsync(string path, HttpListenerRequest request)
        {
            var method = request.HttpMethod;
            var body = await ReadRequestBodyAsync(request);

            return path switch
            {
                "/api/md5" when method == "POST" => HandleMd5(body),
                "/api/json/format" when method == "POST" => HandleJsonFormat(body),
                "/api/json/validate" when method == "POST" => HandleJsonValidate(body),
                "/api/url/encode" when method == "POST" => HandleUrlEncode(body),
                "/api/url/decode" when method == "POST" => HandleUrlDecode(body),
                "/api/escape" when method == "POST" => HandleEscape(body),
                "/api/unescape" when method == "POST" => HandleUnescape(body),
                "/api/base64/encode" when method == "POST" => HandleBase64Encode(body),
                "/api/base64/decode" when method == "POST" => HandleBase64Decode(body),
                "/api/qrcode" when method == "POST" => HandleQrCode(body),
                "/api/barcode" when method == "POST" => HandleBarcode(body),
                "/api/barcode/formats" when method == "GET" => HandleBarcodeFormats(),
                "/api/health" when method == "GET" => HandleHealth(),
                _ => ApiResponse.Fail("Not found", "NOT_FOUND")
            };
        }

        private async Task<string> ReadRequestBodyAsync(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return string.Empty;
            }

            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            return await reader.ReadToEndAsync();
        }

        private ApiResponse HandleMd5(string body)
        {
            var req = JsonSerializer.Deserialize<Md5Request>(body, _jsonOptions) ?? new Md5Request();
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return ApiResponse.Fail("Input is required", "INVALID_INPUT");
            }

            var result = _md5Service.ComputeMd5(req.Input);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleJsonFormat(string body)
        {
            var req = JsonSerializer.Deserialize<JsonFormatRequest>(body, _jsonOptions) ?? new JsonFormatRequest();
            if (string.IsNullOrWhiteSpace(req.Json))
            {
                return ApiResponse.Fail("JSON is required", "INVALID_INPUT");
            }

            var result = _jsonService.FormatJson(req.Json);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleJsonValidate(string body)
        {
            var req = JsonSerializer.Deserialize<JsonFormatRequest>(body, _jsonOptions) ?? new JsonFormatRequest();
            if (string.IsNullOrWhiteSpace(req.Json))
            {
                return ApiResponse.Fail("JSON is required", "INVALID_INPUT");
            }

            var isValid = _jsonService.ValidateJson(req.Json);
            return ApiResponse.Ok(new { isValid });
        }

        private ApiResponse HandleUrlEncode(string body)
        {
            var req = JsonSerializer.Deserialize<UrlEncodeRequest>(body, _jsonOptions) ?? new UrlEncodeRequest();
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return ApiResponse.Fail("Input is required", "INVALID_INPUT");
            }

            var result = _urlEncodeService.Encode(req.Input);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleUrlDecode(string body)
        {
            var req = JsonSerializer.Deserialize<UrlEncodeRequest>(body, _jsonOptions) ?? new UrlEncodeRequest();
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return ApiResponse.Fail("Input is required", "INVALID_INPUT");
            }

            var result = _urlEncodeService.Decode(req.Input);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleEscape(string body)
        {
            var req = JsonSerializer.Deserialize<EscapeRequest>(body, _jsonOptions) ?? new EscapeRequest();
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return ApiResponse.Fail("Input is required", "INVALID_INPUT");
            }

            var result = _escapeService.Escape(req.Input);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleUnescape(string body)
        {
            var req = JsonSerializer.Deserialize<EscapeRequest>(body, _jsonOptions) ?? new EscapeRequest();
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return ApiResponse.Fail("Input is required", "INVALID_INPUT");
            }

            var result = _escapeService.Unescape(req.Input);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleBase64Encode(string body)
        {
            var req = JsonSerializer.Deserialize<Base64EncodeRequest>(body, _jsonOptions) ?? new Base64EncodeRequest();
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return ApiResponse.Fail("Input is required", "INVALID_INPUT");
            }

            var result = _base64Service.EncodeToBase64(req.Input, req.IncludeDataUri ?? false, req.MimeType);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleBase64Decode(string body)
        {
            var req = JsonSerializer.Deserialize<Base64DecodeRequest>(body, _jsonOptions) ?? new Base64DecodeRequest();
            if (string.IsNullOrWhiteSpace(req.Base64))
            {
                return ApiResponse.Fail("Base64 is required", "INVALID_INPUT");
            }

            var result = _base64Service.DecodeFromBase64(req.Base64);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleQrCode(string body)
        {
            var req = JsonSerializer.Deserialize<QrCodeRequest>(body, _jsonOptions) ?? new QrCodeRequest();
            if (string.IsNullOrWhiteSpace(req.Text))
            {
                return ApiResponse.Fail("Text is required", "INVALID_INPUT");
            }

            var result = _qrCodeService.GenerateQrCode(req.Text, req.Width ?? 300, req.Height ?? 300);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleBarcode(string body)
        {
            var req = JsonSerializer.Deserialize<BarcodeRequest>(body, _jsonOptions) ?? new BarcodeRequest();
            if (string.IsNullOrWhiteSpace(req.Text))
            {
                return ApiResponse.Fail("Text is required", "INVALID_INPUT");
            }

            var result = _barcodeService.GenerateBarcode(req.Text, req.Format ?? "CODE_128", req.Width ?? 400, req.Height ?? 120);
            return ApiResponse.Ok(result);
        }

        private ApiResponse HandleBarcodeFormats()
        {
            var formats = _barcodeService.GetSupportedFormats();
            return ApiResponse.Ok(new { formats });
        }

        private ApiResponse HandleHealth()
        {
            return ApiResponse.Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow.ToString("o"),
                version = "1.0.0"
            });
        }

        private async Task SendErrorAsync(HttpListenerResponse response, string message)
        {
            response.ContentType = "application/json";
            response.StatusCode = 500;
            var error = ApiResponse.Fail(message, "INTERNAL_ERROR");
            var jsonResponse = JsonSerializer.Serialize(error, _jsonOptions);
            var buffer = Encoding.UTF8.GetBytes(jsonResponse);
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }
    }

    public class Md5Request
    {
        public string Input { get; set; } = string.Empty;
    }

    public class JsonFormatRequest
    {
        public string Json { get; set; } = string.Empty;
    }

    public class UrlEncodeRequest
    {
        public string Input { get; set; } = string.Empty;
    }

    public class EscapeRequest
    {
        public string Input { get; set; } = string.Empty;
    }

    public class Base64EncodeRequest
    {
        public string Input { get; set; } = string.Empty;
        public bool? IncludeDataUri { get; set; }
        public string? MimeType { get; set; }
    }

    public class Base64DecodeRequest
    {
        public string Base64 { get; set; } = string.Empty;
    }

    public class QrCodeRequest
    {
        public string Text { get; set; } = string.Empty;
        public int? Width { get; set; }
        public int? Height { get; set; }
    }

    public class BarcodeRequest
    {
        public string Text { get; set; } = string.Empty;
        public string? Format { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
