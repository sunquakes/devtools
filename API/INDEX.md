# DevTools API Documentation

Welcome to the DevTools API documentation. This provides comprehensive documentation for the DevTools RESTful API.

## Documentation Index

### 📖 Main Documentation
- **[README.md](README.md)** - Complete API usage documentation with endpoint descriptions and examples
- **[EXAMPLES.md](EXAMPLES.md)** - Detailed usage examples in Python, JavaScript, cURL, and PowerShell
- **[QUICK_REFERENCE.md](QUICK_REFERENCE.md)** - Quick reference guide for all endpoints
- **[REFACTORING_SUMMARY.md](REFACTORING_SUMMARY.md)** - Overview of the API refactoring work
- **[PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md)** - Project structure and architecture documentation

### 🧪 Testing
- **[test_api.ps1](test_api.ps1)** - PowerShell script to test all API endpoints

## Quick Start

### 1. Enable API Server

Edit `App.config` to enable the API server:
```xml
<appSettings>
    <add key="EnableApiServer" value="true"/>
    <add key="ApiServerPort" value="5000"/>
</appSettings>
```

### 2. Run the Application

Start the DevTools application. The API server will automatically start on port 5000.

### 3. Test the API

Use the provided test script or make API calls directly:

**PowerShell:**
```powershell
Invoke-RestMethod http://localhost:5000/api/health
```

**cURL:**
```bash
curl http://localhost:5000/api/health
```

**Python:**
```python
import requests
requests.get('http://localhost:5000/api/health').json()
```

## API Endpoints Overview

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

## Documentation Guide

### For First-Time Users
Start with **[README.md](README.md)** to understand the API basics, then explore **[EXAMPLES.md](EXAMPLES.md)** for practical usage examples.

### For Quick Reference
Use **[QUICK_REFERENCE.md](QUICK_REFERENCE.md)** for a concise overview of all endpoints and request/response formats.

### For Developers
Read **[PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md)** to understand the architecture and **[REFACTORING_SUMMARY.md](REFACTORING_SUMMARY.md)** to learn about the design decisions.

### For Testing
Run **[test_api.ps1](test_api.ps1)** to verify all API endpoints are working correctly.

## Features

- ✅ **13 RESTful API Endpoints** - All DevTools functions exposed as APIs
- ✅ **Unified Response Format** - Consistent JSON response structure
- ✅ **CORS Support** - Can be called from browsers
- ✅ **Comprehensive Documentation** - Complete guides and examples
- ✅ **Easy to Use** - Simple HTTP requests from any language
- ✅ **Thread-Safe** - Supports concurrent requests
- ✅ **Configurable** - Enable/disable and port configuration

## Support

For detailed information about specific endpoints, please refer to:
- **[README.md](README.md)** - Endpoint descriptions and parameters
- **[EXAMPLES.md](EXAMPLES.md)** - Code examples in multiple languages
- **[QUICK_REFERENCE.md](QUICK_REFERENCE.md)** - Quick lookup table

## Version

- **API Version:** 1.0.0
- **Last Updated:** 2026-04-12

---

**DevTools** - Powerful developer tools at your fingertips, now accessible via RESTful API.
