# DevTools API Documentation

## Overview

DevTools now provides an HTTP API server that allows other AI agents or applications to call all tool functions through RESTful APIs.

## Starting the API Server

The API server runs on port 5000 by default. You can configure it by modifying the `App.config` file:

```xml
<appSettings>
    <add key="EnableApiServer" value="true"/>
    <add key="ApiServerPort" value="5000"/>
</appSettings>
```

## API Endpoints

### Health Check

**GET** `/api/health`

Check if the API server is running properly.

**Example Response:**
```json
{
  "success": true,
  "data": {
    "status": "healthy",
    "timestamp": "2026-04-12T10:00:00.0000000Z",
    "version": "1.0.0"
  }
}
```

---

### MD5 Hash

**POST** `/api/md5`

Compute MD5 hash of a string.

**Request Body:**
```json
{
  "input": "hello world"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "hello world",
    "hash32Lower": "5eb63bbbe01eeed093cb22bb8f5acdc3",
    "hash32Upper": "5EB63BBBE01EEED093CB22BB8F5ACDC3",
    "hash16Lower": "3bbbe01eeed093cb",
    "hash16Upper": "3BBBBE01EEED093CB"
  }
}
```

---

### JSON Format

**POST** `/api/json/format`

Format a JSON string.

**Request Body:**
```json
{
  "json": "{\"name\":\"John\",\"age\":30}"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "{\"name\":\"John\",\"age\":30}",
    "formatted": "{\n  \"name\": \"John\",\n  \"age\": 30\n}",
    "isValid": true
  }
}
```

---

### JSON Validate

**POST** `/api/json/validate`

Validate if a JSON string is valid.

**Request Body:**
```json
{
  "json": "{\"name\":\"John\",\"age\":30}"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "isValid": true
  }
}
```

---

### URL Encode

**POST** `/api/url/encode`

Encode a string for URL usage.

**Request Body:**
```json
{
  "input": "Hello World!"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "Hello World!",
    "output": "Hello%20World!",
    "operation": "encode"
  }
}
```

---

### URL Decode

**POST** `/api/url/decode`

Decode a URL-encoded string.

**Request Body:**
```json
{
  "input": "Hello%20World!"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "Hello%20World!",
    "output": "Hello World!",
    "operation": "decode"
  }
}
```

---

### String Escape

**POST** `/api/escape`

Escape special characters in a string.

**Request Body:**
```json
{
  "input": "Hello\nWorld"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "Hello\nWorld",
    "output": "Hello\\nWorld",
    "operation": "escape"
  }
}
```

---

### String Unescape

**POST** `/api/unescape`

Unescape a string.

**Request Body:**
```json
{
  "input": "Hello\\nWorld"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "Hello\\nWorld",
    "output": "Hello\nWorld",
    "operation": "unescape"
  }
}
```

---

### Base64 Encode

**POST** `/api/base64/encode`

Encode a string to Base64.

**Request Body:**
```json
{
  "input": "Hello World",
  "includeDataUri": false,
  "mimeType": "text/plain"
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "Hello World",
    "output": "SGVsbG8gV29ybGQ=",
    "operation": "encode",
    "includeDataUri": false
  }
}
```

---

### Base64 Decode

**POST** `/api/base64/decode`

Decode a Base64 string to original content.

**Request Body:**
```json
{
  "base64": "SGVsbG8gV29ybGQ="
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "SGVsbG8gV29ybGQ=",
    "output": "Hello World",
    "mimeType": null,
    "imageWidth": 0,
    "imageHeight": 0
  }
}
```

---

### Generate QR Code

**POST** `/api/qrcode`

Generate a QR code image (Base64 encoded).

**Request Body:**
```json
{
  "text": "https://example.com",
  "width": 300,
  "height": 300
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "https://example.com",
    "imageBase64": "iVBORw0KGgoAAAANSUhEUgAAASwAAAEsCAYAAAB5fY51AAA...",
    "format": "PNG",
    "width": 300,
    "height": 300
  }
}
```

---

### Generate Barcode

**POST** `/api/barcode`

Generate a barcode image (Base64 encoded).

**Request Body:**
```json
{
  "text": "123456789",
  "format": "CODE_128",
  "width": 400,
  "height": 120
}
```

**Example Response:**
```json
{
  "success": true,
  "data": {
    "input": "123456789",
    "imageBase64": "iVBORw0KGgoAAAANSUhEUgAAAZAAAABkCAYAAACoy2Z3AAA...",
    "format": "CODE_128",
    "width": 400,
    "height": 120
  }
}
```

---

### Get Supported Barcode Formats

**GET** `/api/barcode/formats`

Get a list of all supported barcode formats.

**Example Response:**
```json
{
  "success": true,
  "data": {
    "formats": [
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
    ]
  }
}
```

---

## Error Responses

All API endpoints return a unified error response format when an error occurs:

```json
{
  "success": false,
  "message": "Error description",
  "errorCode": "ERROR_CODE"
}
```

### Common Error Codes

- `INVALID_INPUT` - Invalid or missing input parameter
- `NOT_FOUND` - API endpoint not found
- `INTERNAL_ERROR` - Internal server error
- `HTTP_4xx` - HTTP protocol errors

---

## CORS Support

The API server has CORS enabled by default, allowing requests from any origin. You can modify this through configuration:

```json
{
  "EnableCors": true,
  "AllowedOrigins": "https://example.com"
}
```

---

## Usage Examples

### Python Example

```python
import requests

# MD5 Hash
response = requests.post('http://localhost:5000/api/md5', json={
    'input': 'hello world'
})
result = response.json()
print(result['data']['hash32Lower'])

# JSON Format
response = requests.post('http://localhost:5000/api/json/format', json={
    'json': '{"name":"John","age":30}'
})
result = response.json()
print(result['data']['formatted'])

# Generate QR Code
response = requests.post('http://localhost:5000/api/qrcode', json={
    'text': 'https://example.com',
    'width': 300,
    'height': 300
})
result = response.json()

# Save QR Code Image
import base64
image_data = base64.b64decode(result['data']['imageBase64'])
with open('qrcode.png', 'wb') as f:
    f.write(image_data)
```

### JavaScript Example

```javascript
// MD5 Hash
const response = await fetch('http://localhost:5000/api/md5', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ input: 'hello world' })
});
const result = await response.json();
console.log(result.data.hash32Lower);

// JSON Format
const response = await fetch('http://localhost:5000/api/json/format', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ json: '{"name":"John","age":30}' })
});
const result = await response.json();
console.log(result.data.formatted);
```

### cURL Example

```bash
# MD5 Hash
curl -X POST http://localhost:5000/api/md5 \
  -H "Content-Type: application/json" \
  -d '{"input":"hello world"}'

# JSON Format
curl -X POST http://localhost:5000/api/json/format \
  -H "Content-Type: application/json" \
  -d '{"json":"{\"name\":\"John\",\"age\":30}"}'

# Health Check
curl http://localhost:5000/api/health
```

---

## Notes

1. The API server is only available when the application is running
2. Default port is 5000, can be modified through configuration file
3. All API endpoints require `Content-Type: application/json` header
4. Image-related APIs (QR Code, Barcode) return Base64-encoded PNG images
5. API server supports CORS requests
