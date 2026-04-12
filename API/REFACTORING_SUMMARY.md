# DevTools API Refactoring Summary

## Refactoring Overview

This refactoring exposes all page functions of the DevTools application as RESTful APIs, allowing other AI agents and applications to call these functions through HTTP requests.

## New Features

### 1. HTTP API Server
- Lightweight HTTP server based on `HttpListener`
- Runs on port 5000 by default
- Supports CORS cross-origin requests
- Automatic start/stop, synchronized with application lifecycle

### 2. API Endpoints (13 total)

#### Basic Tools
1. **MD5 Hash** (`/api/md5`)
   - Compute MD5 hash of strings (32-bit and 16-bit, uppercase and lowercase)

2. **JSON Formatting** (`/api/json/format`)
   - Format JSON strings
   - Support JSON validation

3. **URL Encoding/Decoding** (`/api/url/encode`, `/api/url/decode`)
   - URL encoding and decoding functionality

4. **String Escaping/Unescaping** (`/api/escape`, `/api/unescape`)
   - Escape special characters (\n, \t, \r, etc.)

5. **Base64 Encoding/Decoding** (`/api/base64/encode`, `/api/base64/decode`)
   - Support Data URI format
   - Automatic image Base64 detection

#### Image Generation
6. **QR Code Generation** (`/api/qrcode`)
   - Support custom dimensions
   - Returns PNG format Base64

7. **Barcode Generation** (`/api/barcode`)
   - Support 11 barcode formats
   - Customizable dimensions

8. **Get Supported Barcode Formats** (`/api/barcode/formats`)

### 3. Architecture Design

```
API/
├── Models/              # Data models
│   ├── ApiResponse.cs   # Unified response format
│   └── ApiModels.cs     # Data models for each function
├── Services/            # Business logic layer
│   ├── IApiServices.cs  # Service interface definitions
│   ├── Md5ApiService.cs
│   ├── JsonApiService.cs
│   ├── UrlEncodeApiService.cs
│   ├── EscapeApiService.cs
│   ├── Base64ApiService.cs
│   ├── QrCodeApiService.cs
│   ├── BarcodeApiService.cs
│   └── SignatureApiService.cs
├── Server/              # HTTP server layer
│   ├── HttpApiServer.cs # HTTP server implementation
│   └── RequestHandler.cs # Request routing and handling
└── ApiManager.cs        # API manager (singleton)
```

### 4. Configuration Options

Add to `App.config`:
```xml
<appSettings>
    <add key="EnableApiServer" value="true"/>
    <add key="ApiServerPort" value="5000"/>
</appSettings>
```

## Technical Features

### 1. Unified Response Format
All API endpoints return a unified JSON response format:
```json
{
  "success": true/false,
  "message": "Error message (if any)",
  "data": { ... },
  "errorCode": "Error code (if any)"
}
```

### 2. Error Handling
- Unified error response format
- Clear error codes
- Friendly error messages

### 3. Thread Safety
- API manager uses singleton pattern
- HTTP server handles requests asynchronously
- Supports concurrent requests

### 4. Non-Invasive
- API server runs independently, does not affect existing UI functionality
- Optional enable/disable
- Does not modify original page code

## Use Cases

### 1. AI Agent Integration
Other AI agents can call DevTools functions through HTTP API:
```python
# AI agent can call API to process user input
response = requests.post('http://localhost:5000/api/md5', json={'input': user_input})
hash_value = response.json()['data']['hash32Lower']
```

### 2. Automation Tools
Automation tools can integrate these APIs:
```python
# Batch generate QR codes
for url in url_list:
    response = requests.post('http://localhost:5000/api/qrcode', 
                            json={'text': url})
    save_qrcode(response.json()['data']['imageBase64'])
```

### 3. Development Tools
IDE plugins or development tools can call these APIs:
```javascript
// VS Code plugin can call JSON formatting API
const formatted = await fetch('http://localhost:5000/api/json/format', {
    method: 'POST',
    body: JSON.stringify({ json: unformattedJson })
});
```

## Documentation

Complete documentation is provided:
1. **README.md** - API usage documentation
2. **EXAMPLES.md** - Detailed usage examples (Python, JavaScript, cURL, PowerShell)
3. **QUICK_REFERENCE.md** - Quick reference manual
4. **test_api.ps1** - PowerShell test script

## Compatibility

- .NET 8.0 Windows
- Supports all HTTP clients (any programming language)
- CORS support, allows browser direct calls
- RESTful API design, conforms to industry standards

## Performance

- Lightweight HTTP server, low memory footprint
- Asynchronous request processing, high concurrency support
- No additional dependencies, uses existing libraries

## Security

- Only listens on localhost, not exposed to external network
- Configurable port
- Enable/disable option

## Future Extensions

Easy to add more API endpoints:
1. Create new service class in `Services/` directory
2. Add route in `RequestHandler.cs`
3. Add data model in `Models/ApiModels.cs`

## Testing

After running the application, use PowerShell test script:
```powershell
cd API
.\test_api.ps1
```

Or test manually with cURL:
```bash
curl http://localhost:5000/api/health
curl -X POST http://localhost:5000/api/md5 -H "Content-Type: application/json" -d '{"input":"test"}'
```

## Summary

This refactoring successfully exposed all core functions of DevTools as RESTful APIs, making it easy for other AI agents and applications to integrate and use these functions. The API design follows industry standards, provides complete documentation and examples, and is easy to use and extend.
