# DevTools

<p align="center">
  <img src="Resources/Images/logo.png" alt="DevTools Logo" width="128" height="128">
</p>

<p align="center">
  <strong>A lightweight developer toolkit for daily development tasks</strong>
</p>

<p align="center">
  <a href="#features">Features</a> •
  <a href="#installation">Installation</a> •
  <a href="#usage">Usage</a> •
  <a href="#api-documentation">API</a> •
  <a href="#development">Development</a> •
  <a href="README_ZH.md">中文</a>
</p>

---

## Features

- **MD5 Hash Calculator** - Calculate 32-bit and 16-bit MD5 hashes (uppercase/lowercase)
- **Barcode Generator** - Generate CODE 128 barcodes
- **QR Code Generator** - Generate QR codes for text/URLs
- **Base64 ↔ Image** - Convert between Base64 strings and images
- **JSON Formatter** - Format, expand, and collapse JSON data
- **URL Encode/Decode** - Encode and decode URL strings
- **String Escape/Unescape** - Escape and unescape special characters
- **Handwritten Signature** - Draw signatures and convert to Base64 or save as images
- **RESTful API** - All tools accessible via HTTP API for AI agent integration

## RESTful API

DevTools now provides a RESTful API server that exposes all tool functions as HTTP endpoints. This allows AI agents and other applications to programmatically access all features.

### Quick Start

1. Enable API server in `App.config`:
```xml
<appSettings>
    <add key="EnableApiServer" value="true"/>
    <add key="ApiServerPort" value="5000"/>
</appSettings>
```

2. Run DevTools application - API server starts automatically

3. Call API endpoints:
```bash
# Health check
curl http://localhost:5000/api/health

# MD5 hash
curl -X POST http://localhost:5000/api/md5 \
  -H "Content-Type: application/json" \
  -d '{"input":"hello world"}'

# Generate QR code
curl -X POST http://localhost:5000/api/qrcode \
  -H "Content-Type: application/json" \
  -d '{"text":"https://example.com","width":300,"height":300}'
```

### Available API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/health` | GET | Health check |
| `/api/md5` | POST | MD5 hash computation |
| `/api/json/format` | POST | JSON formatting |
| `/api/json/validate` | POST | JSON validation |
| `/api/url/encode` | POST | URL encoding |
| `/api/url/decode` | POST | URL decoding |
| `/api/escape` | POST | String escaping |
| `/api/unescape` | POST | String unescaping |
| `/api/base64/encode` | POST | Base64 encoding |
| `/api/base64/decode` | POST | Base64 decoding |
| `/api/qrcode` | POST | Generate QR code |
| `/api/barcode` | POST | Generate barcode |
| `/api/barcode/formats` | GET | Get supported barcode formats |

### API Documentation

For complete API documentation, see:
- **[API/README.md](API/README.md)** - Full API documentation
- **[API/EXAMPLES.md](API/EXAMPLES.md)** - Usage examples (Python, JavaScript, cURL, PowerShell)
- **[API/QUICK_REFERENCE.md](API/QUICK_REFERENCE.md)** - Quick reference guide

## Installation

Download the latest release for your platform:

| Platform | Architecture | Download |
|----------|-------------|----------|
| Windows | x64 (64-bit) | `DevTools-win-x64.exe` |
| Windows | x86 (32-bit) | `DevTools-win-x86.exe` |
| Windows | ARM64 | `DevTools-win-arm64.exe` |

## Usage

1. Download the executable for your platform
2. Run `DevTools.exe` directly (no installation required)
3. Select a tool from the home screen
4. (Optional) Enable API server in `App.config` for programmatic access

## Development

### Prerequisites

- .NET 8.0 SDK
- Windows OS

### Build

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run

# Publish (self-contained single file)
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

### Project Structure

```
DevTools/
├── API/                # RESTful API implementation
│   ├── Models/         # API data models
│   ├── Services/       # API service implementations
│   ├── Server/         # HTTP server
│   └── Documentation/  # API documentation
├── Pages/              # Application pages
│   ├── HomePage.xaml
│   ├── Md5Page.xaml
│   ├── BarcodePage.xaml
│   ├── QrPage.xaml
│   ├── Base64ImagePage.xaml
│   ├── ImageToBase64Page.xaml
│   ├── JsonFormatPage.xaml
│   ├── UrlEncodePage.xaml
│   ├── EscapePage.xaml
│   └── SignaturePage.xaml
├── Resources/          # Resources (images, strings, fonts)
│   ├── Images/
│   ├── Strings.resx
│   ├── Strings.zh-CN.resx
│   └── Strings.en-US.resx
├── Helpers/            # Utility classes
├── MainWindow.xaml     # Main window
└── App.xaml            # Application entry point
```

## Localization

The application supports multiple languages:
- English (en-US)
- 简体中文 (zh-CN)

## License

MIT License

## Version

**Current Version:** 2.0.0

---

**DevTools** - Your essential development companion, now with RESTful API support!
