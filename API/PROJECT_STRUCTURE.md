# DevTools API Project Structure

## Directory Structure

```
DevTools/
├── API/                           # API related code and documentation
│   ├── Models/                    # Data models
│   │   ├── ApiResponse.cs         # Unified API response format
│   │   └── ApiModels.cs           # Data models for each function
│   ├── Services/                  # Business logic services
│   │   ├── IApiServices.cs        # Service interface definitions
│   │   ├── Md5ApiService.cs       # MD5 function implementation
│   │   ├── JsonApiService.cs      # JSON formatting implementation
│   │   ├── UrlEncodeApiService.cs # URL encoding/decoding implementation
│   │   ├── EscapeApiService.cs    # String escaping implementation
│   │   ├── Base64ApiService.cs    # Base64 encoding/decoding implementation
│   │   ├── QrCodeApiService.cs    # QR code generation implementation
│   │   ├── BarcodeApiService.cs   # Barcode generation implementation
│   │   └── SignatureApiService.cs # Signature generation implementation
│   ├── Server/                    # HTTP server implementation
│   │   ├── HttpApiServer.cs       # HTTP server core
│   │   └── RequestHandler.cs      # Request routing and handling
│   ├── ApiManager.cs              # API manager (singleton pattern)
│   ├── README.md                  # API usage documentation
│   ├── EXAMPLES.md                # Usage examples
│   ├── QUICK_REFERENCE.md         # Quick reference
│   ├── REFACTORING_SUMMARY.md     # Refactoring summary
│   └── test_api.ps1               # PowerShell test script
│
├── Pages/                         # WPF pages (original functionality, not modified)
│   ├── HomePage.xaml.cs           # Home page
│   ├── Md5Page.xaml.cs            # MD5 page
│   ├── JsonFormatPage.xaml.cs     # JSON formatting page
│   ├── UrlEncodePage.xaml.cs      # URL encoding page
│   ├── EscapePage.xaml.cs         # String escaping page
│   ├── Base64ImagePage.xaml.cs    # Base64 to image page
│   ├── ImageToBase64Page.xaml.cs  # Image to Base64 page
│   ├── QrPage.xaml.cs             # QR code page
│   ├── BarcodePage.xaml.cs        # Barcode page
│   ├── SignaturePage.xaml.cs      # Signature page
│   └── SettingsPage.xaml.cs       # Settings page
│
├── Helpers/                       # Helper classes
│   ├── PageStateManager.cs        # Page state management
│   └── ClipboardHelper.cs         # Clipboard helper
│
├── Resources/                     # Resource files
│   ├── Strings.Designer.cs        # Localized strings
│   └── Images/                    # Image resources
│
├── Themes/                        # UI themes
│   ├── MaterialStyles.xaml
│   ├── MaterialStylesDark.xaml
│   └── CardStyles.xaml
│
├── App.config                     # Application configuration (API config added)
├── App.xaml                       # Application definition
├── App.xaml.cs                    # Application code
├── MainWindow.xaml                # Main window UI
├── MainWindow.xaml.cs             # Main window code (API integrated)
├── DevTools.csproj                # Project file
└── appsettings.json               # Application settings (optional)
```

## File Descriptions

### API Core Files

#### API/Models/
- **ApiResponse.cs**: Defines unified API response format
  - `ApiResponse<T>`: Generic response class
  - `ApiResponse`: Non-generic response class
  
- **ApiModels.cs**: Defines data models for each function
  - `Md5Result`: MD5 result
  - `JsonFormatResult`: JSON formatting result
  - `UrlEncodeResult`: URL encoding result
  - `EscapeResult`: Escaping result
  - `Base64EncodeResult`/`Base64DecodeResult`: Base64 results
  - `QrCodeResult`: QR code result
  - `BarcodeResult`: Barcode result
  - `SignatureResult`: Signature result
  - `StrokePoint`: Signature point coordinates

#### API/Services/
- **IApiServices.cs**: Defines interfaces for all services
  - `IMd5ApiService`
  - `IJsonApiService`
  - `IUrlEncodeApiService`
  - `IEscapeApiService`
  - `IBase64ApiService`
  - `IQrCodeApiService`
  - `IBarcodeApiService`
  - `ISignatureApiService`

- **Service Implementation Classes**: Each service class implements corresponding interface, providing specific functionality

#### API/Server/
- **HttpApiServer.cs**: HTTP server implementation
  - Listens for HTTP requests
  - Asynchronous request processing
  - Supports start/stop
  
- **RequestHandler.cs**: Request handling
  - Route dispatch
  - Request parameter parsing
  - Response generation

#### API/ApiManager.cs
- API manager (singleton pattern)
- Controls HTTP server lifecycle
- Provides unified access entry

### Modified Files

#### App.config
Added API server configuration:
```xml
<add key="EnableApiServer" value="true"/>
<add key="ApiServerPort" value="5000"/>
```

#### MainWindow.xaml.cs
- Added `using DevTools.API;`
- Added `_apiServerEnabled` field
- Added `InitializeApiServer()` method
- Closes API server in `ExitApplication()`
- Handles API configuration in `LoadSettings()` and `SaveSettings()`

### Documentation Files

#### API/README.md
Complete API usage documentation, including:
- Detailed description of all endpoints
- Request/response examples
- Error handling description
- Usage examples (Python, JavaScript, cURL)

#### API/EXAMPLES.md
Detailed usage examples, including:
- Python examples
- JavaScript/Node.js examples
- cURL examples
- PowerShell examples

#### API/QUICK_REFERENCE.md
Quick reference manual, including:
- Endpoints overview table
- Request/response examples
- Supported barcode formats
- Quick test commands

#### API/REFACTORING_SUMMARY.md
Refactoring summary documentation, including:
- Refactoring overview
- New features
- Architecture design
- Technical features
- Use cases

#### API/test_api.ps1
PowerShell test script, tests all API endpoints

## Dependencies

```
MainWindow.xaml.cs
    └── API/ApiManager.cs
            └── API/Server/HttpApiServer.cs
                    └── API/Server/RequestHandler.cs
                            └── API/Services/*.cs
                                    └── API/Models/*.cs
```

## Design Patterns

1. **Singleton Pattern**: `ApiManager` ensures only one instance
2. **Interface Segregation**: Each service has independent interface
3. **Dependency Injection**: `RequestHandler` composes all services
4. **Strategy Pattern**: Different services implement different functions
5. **RESTful Design**: API endpoints follow REST specification

## Code Organization Principles

1. **Single Responsibility**: Each class is responsible for one function
2. **Interface Segregation**: Interface definitions are clear and explicit
3. **Open-Closed Principle**: Easy to extend new functions
4. **Dependency Inversion**: Depend on abstractions rather than concrete implementations
5. **DRY Principle**: Code reuse, avoid duplication

## Extension Guide

### Adding New API Endpoints

1. Add data model in `API/Models/ApiModels.cs`
2. Add interface in `API/Services/IApiServices.cs`
3. Create service implementation class in `API/Services/`
4. Add route and handling method in `API/Server/RequestHandler.cs`
5. Update documentation

### Example: Adding SHA256 Function

```csharp
// 1. Add model (ApiModels.cs)
public class Sha256Result {
    public string Input { get; set; }
    public string Hash { get; set; }
}

// 2. Add interface (IApiServices.cs)
public interface ISha256ApiService {
    Sha256Result ComputeSha256(string input);
}

// 3. Implement service (Sha256ApiService.cs)
public class Sha256ApiService : ISha256ApiService {
    public Sha256Result ComputeSha256(string input) {
        // Implementation code
    }
}

// 4. Add route (RequestHandler.cs)
private ApiResponse HandleSha256(string body) {
    // Processing code
}

// Add route in RouteRequestAsync
"/api/sha256" when method == "POST" => HandleSha256(body),
```

## Notes

1. All API services should be stateless
2. All exceptions should be caught and converted to friendly error messages
3. All responses should use unified format
4. Avoid using UI-related code in API
5. Maintain code testability
