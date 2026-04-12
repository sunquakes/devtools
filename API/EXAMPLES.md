# DevTools API Usage Examples

## Starting the Application

1. Ensure the API server is enabled in `App.config`:
```xml
<appSettings>
    <add key="EnableApiServer" value="true"/>
    <add key="ApiServerPort" value="5000"/>
</appSettings>
```

2. Run the DevTools application
3. The API server will automatically start in the background and display a notification in the system tray

## API Endpoints List

### Basic Tools
- `POST /api/md5` - MD5 hash computation
- `POST /api/json/format` - JSON formatting
- `POST /api/json/validate` - JSON validation
- `POST /api/url/encode` - URL encoding
- `POST /api/url/decode` - URL decoding
- `POST /api/escape` - String escaping
- `POST /api/unescape` - String unescaping
- `POST /api/base64/encode` - Base64 encoding
- `POST /api/base64/decode` - Base64 decoding

### Image Generation
- `POST /api/qrcode` - Generate QR code
- `POST /api/barcode` - Generate barcode
- `GET /api/barcode/formats` - Get supported barcode formats

### System
- `GET /api/health` - Health check

## Python Usage Examples

### 1. MD5 Hash
```python
import requests
import hashlib

# Compute MD5
response = requests.post('http://localhost:5000/api/md5', json={
    'input': 'hello world'
})

if response.json()['success']:
    data = response.json()['data']
    print(f"Input: {data['input']}")
    print(f"MD5 (32-bit lowercase): {data['hash32Lower']}")
    print(f"MD5 (32-bit uppercase): {data['hash32Upper']}")
    print(f"MD5 (16-bit lowercase): {data['hash16Lower']}")
    print(f"MD5 (16-bit uppercase): {data['hash16Upper']}")
```

### 2. JSON Formatting
```python
# Format JSON
response = requests.post('http://localhost:5000/api/json/format', json={
    'json': '{"name":"John","age":30,"city":"New York"}'
})

result = response.json()
if result['success']:
    print("Formatted JSON:")
    print(result['data']['formatted'])
else:
    print(f"JSON invalid: {result['data']['errorMessage']}")

# Validate JSON
response = requests.post('http://localhost:5000/api/json/validate', json={
    'json': '{"name":"John","age":30}'
})

if response.json()['data']['isValid']:
    print("JSON is valid")
else:
    print("JSON is invalid")
```

### 3. URL Encoding/Decoding
```python
# URL Encode
text = "Hello World! 你好"
response = requests.post('http://localhost:5000/api/url/encode', json={
    'input': text
})
encoded = response.json()['data']['output']
print(f"Encoded: {encoded}")

# URL Decode
response = requests.post('http://localhost:5000/api/url/decode', json={
    'input': encoded
})
decoded = response.json()['data']['output']
print(f"Decoded: {decoded}")
```

### 4. Base64 Encoding/Decoding
```python
# Encode
text = "Hello World"
response = requests.post('http://localhost:5000/api/base64/encode', json={
    'input': text,
    'includeDataUri': False
})
encoded = response.json()['data']['output']
print(f"Base64: {encoded}")

# Decode
response = requests.post('http://localhost:5000/api/base64/decode', json={
    'base64': encoded
})
decoded = response.json()['data']['output']
print(f"Original: {decoded}")
```

### 5. Generate QR Code and Save
```python
import base64

text = "https://github.com"
response = requests.post('http://localhost:5000/api/qrcode', json={
    'text': text,
    'width': 300,
    'height': 300
})

if response.json()['success']:
    image_data = base64.b64decode(response.json()['data']['imageBase64'])
    with open('qrcode.png', 'wb') as f:
        f.write(image_data)
    print("QR code saved as qrcode.png")
```

### 6. Generate Barcode and Save
```python
# Generate CODE_128 barcode
response = requests.post('http://localhost:5000/api/barcode', json={
    'text': '123456789',
    'format': 'CODE_128',
    'width': 400,
    'height': 120
})

if response.json()['success']:
    image_data = base64.b64decode(response.json()['data']['imageBase64'])
    with open('barcode.png', 'wb') as f:
        f.write(image_data)
    print("Barcode saved as barcode.png")

# Get all supported barcode formats
response = requests.get('http://localhost:5000/api/barcode/formats')
formats = response.json()['data']['formats']
print(f"Supported formats: {', '.join(formats)}")
```

### 7. String Escaping
```python
# Escape special characters
text = "Hello\nWorld\t!"
response = requests.post('http://localhost:5000/api/escape', json={
    'input': text
})
escaped = response.json()['data']['output']
print(f"Escaped: {escaped}")

# Unescape
response = requests.post('http://localhost:5000/api/unescape', json={
    'input': escaped
})
unescaped = response.json()['data']['output']
print(f"Unescaped: {unescaped}")
```

## JavaScript/Node.js Usage Examples

### 1. Basic Usage
```javascript
const fetch = require('node-fetch');

// MD5 Hash
async function computeMd5(input) {
    const response = await fetch('http://localhost:5000/api/md5', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ input })
    });
    const result = await response.json();
    return result.data;
}

// Usage
computeMd5('hello world').then(data => {
    console.log('MD5:', data.hash32Lower);
});
```

### 2. JSON Formatting
```javascript
async function formatJson(json) {
    const response = await fetch('http://localhost:5000/api/json/format', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ json })
    });
    const result = await response.json();
    return result.data;
}

// Usage
formatJson('{"name":"John","age":30}').then(data => {
    if (data.isValid) {
        console.log('Formatted JSON:');
        console.log(data.formatted);
    }
});
```

### 3. Generate QR Code
```javascript
const fs = require('fs');

async function generateQrCode(text, outputPath) {
    const response = await fetch('http://localhost:5000/api/qrcode', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            text: text,
            width: 300,
            height: 300
        })
    });
    
    const result = await response.json();
    if (result.success) {
        const imageBuffer = Buffer.from(result.data.imageBase64, 'base64');
        fs.writeFileSync(outputPath, imageBuffer);
        console.log(`QR code saved to ${outputPath}`);
    }
}

// Usage
generateQrCode('https://example.com', 'qrcode.png');
```

## cURL Usage Examples

```bash
# MD5 Hash
curl -X POST http://localhost:5000/api/md5 \
  -H "Content-Type: application/json" \
  -d '{"input":"hello world"}'

# JSON Formatting
curl -X POST http://localhost:5000/api/json/format \
  -H "Content-Type: application/json" \
  -d '{"json":"{\"name\":\"John\",\"age\":30}"}'

# URL Encoding
curl -X POST http://localhost:5000/api/url/encode \
  -H "Content-Type: application/json" \
  -d '{"input":"Hello World!"}'

# Base64 Encoding
curl -X POST http://localhost:5000/api/base64/encode \
  -H "Content-Type: application/json" \
  -d '{"input":"Hello World"}'

# Generate QR Code
curl -X POST http://localhost:5000/api/qrcode \
  -H "Content-Type: application/json" \
  -d '{"text":"https://example.com","width":300,"height":300}' \
  --output qrcode_response.json

# Extract and save QR code image (requires jq)
cat qrcode_response.json | jq -r '.data.imageBase64' | base64 -d > qrcode.png

# Health Check
curl http://localhost:5000/api/health
```

## PowerShell Usage Examples

```powershell
# MD5 Hash
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/md5" -Method Post -Body '{"input":"hello world"}' -ContentType "application/json"
Write-Host "MD5: $($response.data.hash32Lower)"

# JSON Formatting
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/json/format" -Method Post -Body '{"json":"{\"name\":\"John\"}"}' -ContentType "application/json"
Write-Host "Formatted:`n$($response.data.formatted)"

# Generate QR Code
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/qrcode" -Method Post -Body '{"text":"https://example.com"}' -ContentType "application/json"
$imageBytes = [Convert]::FromBase64String($response.data.imageBase64)
[System.IO.File]::WriteAllBytes("qrcode.png", $imageBytes)
```

## Integration Examples

### AI Agent Integration

```python
# Example: AI agent uses DevTools API to process user requests
import requests

class DevToolsAgent:
    def __init__(self, base_url='http://localhost:5000'):
        self.base_url = base_url
    
    def compute_hash(self, text):
        """Compute MD5 hash of text"""
        response = requests.post(f'{self.base_url}/api/md5', json={'input': text})
        return response.json()['data']['hash32Lower']
    
    def format_json(self, json_str):
        """Format JSON string"""
        response = requests.post(f'{self.base_url}/api/json/format', json={'json': json_str})
        return response.json()['data']
    
    def generate_qr(self, text, width=300, height=300):
        """Generate QR code"""
        response = requests.post(f'{self.base_url}/api/qrcode', 
                                json={'text': text, 'width': width, 'height': height})
        return response.json()['data']['imageBase64']

# Usage
agent = DevToolsAgent()
hash_value = agent.compute_hash('hello world')
print(f"MD5: {hash_value}")
```

### Batch Processing

```python
# Batch generate QR codes for multiple URLs
urls = [
    'https://example.com/page1',
    'https://example.com/page2',
    'https://example.com/page3'
]

for i, url in enumerate(urls):
    response = requests.post('http://localhost:5000/api/qrcode', 
                            json={'text': url})
    if response.json()['success']:
        image_data = base64.b64decode(response.json()['data']['imageBase64'])
        with open(f'qrcode_{i}.png', 'wb') as f:
            f.write(image_data)
        print(f"Generated qrcode_{i}.png")
```
