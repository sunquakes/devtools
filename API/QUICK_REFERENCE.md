# DevTools API Quick Reference

## Configuration

Enable the API server in `App.config`:
```xml
<appSettings>
    <add key="EnableApiServer" value="true"/>
    <add key="ApiServerPort" value="5000"/>
</appSettings>
```

## Endpoints Overview

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/health` | GET | Health check |
| `/api/md5` | POST | MD5 hash |
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
| `/api/barcode/formats` | GET | Get barcode formats |

## Request/Response Examples

### MD5
```json
// POST /api/md5
Request: {"input": "hello world"}
Response: {
  "hash32Lower": "5eb63bbbe01eeed093cb22bb8f5acdc3",
  "hash32Upper": "5EB63BBBE01EEED093CB22BB8F5ACDC3",
  "hash16Lower": "3bbbe01eeed093cb",
  "hash16Upper": "3BBBBE01EEED093CB"
}
```

### JSON Formatting
```json
// POST /api/json/format
Request: {"json": "{\"name\":\"John\"}"}
Response: {
  "formatted": "{\n  \"name\": \"John\"\n}",
  "isValid": true
}
```

### URL Encoding
```json
// POST /api/url/encode
Request: {"input": "Hello World!"}
Response: {"output": "Hello%20World!", "operation": "encode"}
```

### Base64
```json
// POST /api/base64/encode
Request: {"input": "Hello", "includeDataUri": false}
Response: {"output": "SGVsbG8=", "operation": "encode"}
```

### QR Code
```json
// POST /api/qrcode
Request: {"text": "https://example.com", "width": 300, "height": 300}
Response: {
  "imageBase64": "iVBORw0KGgoAAAANSUhEUg...",
  "format": "PNG",
  "width": 300,
  "height": 300
}
```

### Barcode
```json
// POST /api/barcode
Request: {"text": "123456789", "format": "CODE_128", "width": 400, "height": 120}
Response: {
  "imageBase64": "iVBORw0KGgoAAAANSUhEUg...",
  "format": "CODE_128",
  "width": 400,
  "height": 120
}
```

## Supported Barcode Formats

- CODE_128
- CODE_39
- CODE_93
- CODABAR
- EAN_8
- EAN_13
- UPC_A
- UPC_E
- ITF
- DATA_MATRIX
- PDF_417

## Quick Testing

### PowerShell
```powershell
# Health check
Invoke-RestMethod http://localhost:5000/api/health

# MD5
Invoke-RestMethod http://localhost:5000/api/md5 -Method Post -Body '{"input":"test"}' -ContentType application/json
```

### cURL
```bash
# Health check
curl http://localhost:5000/api/health

# MD5
curl -X POST http://localhost:5000/api/md5 -H "Content-Type: application/json" -d '{"input":"test"}'
```

### Python
```python
import requests

# Health check
requests.get('http://localhost:5000/api/health').json()

# MD5
requests.post('http://localhost:5000/api/md5', json={'input': 'test'}).json()
```

## Error Codes

| Error Code | Description |
|------------|-------------|
| `INVALID_INPUT` | Invalid input parameter |
| `NOT_FOUND` | Endpoint not found |
| `INTERNAL_ERROR` | Internal server error |
