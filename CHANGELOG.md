# Changelog

All notable changes to this project will be documented in this file.

---

## [2.1.0] - 2026-04-17

### Added
- **JSON Compress Feature** - Compress JSON to single-line format
  - Remove all whitespace and line breaks
  - Preserve JSON structure and data
  - Success/error message feedback
  - Integrated with existing JSON formatter page
- **Copy Input Content Button** - Copy original JSON input
  - Up arrow + copy icon combination
  - Copy content from input textbox
  - Separate from formatted JSON copy button
- **Enhanced Copy Buttons** - Distinguish copy sources with icons
  - Copy Input: Up arrow + Copy icon (⬆️📋)
  - Copy Formatted: Copy icon + Down arrow (📋⬇️)
  - Clear visual distinction between two copy actions
- **Full Internationalization (i18n)** - Complete i18n support for all user-facing text
  - All MessageBox prompts now use resource strings
  - All button tooltips internationalized
  - All loading indicators internationalized
  - Support for Chinese (zh-CN) and English (en-US)
  - New resource strings: JSONCompress, JSONCompressed, DragToSort, ProcessJSONError, CompressJSONError, Processing, CopyInputContent, InputContentCopied, CopyFailed, PleaseEnterContent

### Changed
- **Button Layout** - Reorganized JSON formatter toolbar
  - Moved copy buttons together for better UX
  - Button order: Format | Compress | ExpandAll | CollapseAll | CopyInput | CopyFormatted
  - More intuitive button grouping
- **Icon Design** - Enhanced copy button icons
  - Copy Input: Up arrow indicates source is above
  - Copy Formatted: Down arrow indicates source is below
  - Visual cues match button functionality

### Technical Details
- All hardcoded Chinese strings replaced with resource references
- Complete i18n coverage for all user interface elements
- Resource files updated: Strings.zh-CN.resx, Strings.en-US.resx, Strings.cs

---

## [2.0.0] - 2026-04-12

### Added
- **RESTful API Server** - Complete HTTP API for all tool functions
  - 13 API endpoints for programmatic access
  - AI agent integration support
  - CORS support for browser-based applications
  - Configurable port (default: 5000)
  - Enable/disable option in App.config
- **API Documentation** - Comprehensive API documentation
  - Complete endpoint documentation (README.md)
  - Usage examples in multiple languages (EXAMPLES.md)
  - Quick reference guide (QUICK_REFERENCE.md)
  - Project structure documentation (PROJECT_STRUCTURE.md)
  - Refactoring summary (REFACTORING_SUMMARY.md)
- **API Test Script** - PowerShell script to test all API endpoints
- **API Service Layer** - Modular service architecture
  - IMd5ApiService - MD5 hash computation
  - IJsonApiService - JSON formatting and validation
  - IUrlEncodeApiService - URL encoding/decoding
  - IEscapeApiService - String escaping/unesaping
  - IBase64ApiService - Base64 encoding/decoding
  - IQrCodeApiService - QR code generation
  - IBarcodeApiService - Barcode generation (11 formats)
  - ISignatureApiService - Signature generation
- **HTTP Server** - Lightweight HTTP server based on HttpListener
  - Asynchronous request processing
  - Thread-safe operation
  - Unified response format
  - Error handling with error codes
- **Configuration Options** - App.config settings for API server
  - EnableApiServer - Enable/disable API server
  - ApiServerPort - Configure server port

### Changed
- **README.md** - Updated with API documentation section
- **Project Structure** - Added API directory and documentation
- **MainWindow** - Integrated API server lifecycle management
- **Documentation Language** - All documentation now in English

### Technical Details
- **Architecture** - Service-oriented architecture with clear separation
- **Design Patterns** - Singleton, Dependency Injection, Strategy Pattern
- **Security** - Localhost-only binding, configurable port
- **Performance** - Asynchronous processing, low memory footprint

### Documentation
- All API documentation in English
- Code examples in Python, JavaScript, cURL, and PowerShell
- Quick reference for all endpoints
- Integration examples for AI agents

---

## [1.3.0] - 2026-04-10

### Added
- String Escape/Unescape functionality
  - Escape special characters in strings
  - Unescape escaped strings
  - Support for common escape sequences (\n, \t, \r, etc.)
- JSON formatter improvements
  - Double-click to select only the value (excluding key and colon)
  - Right-click context menu now copies selected text instead of full line
  - Auto word wrap for long JSON values
  - No horizontal scrolling needed

### Changed
- QR Code and Barcode pages
  - Clear input field after generating new codes
- Optimized shortcut creation, working directory and window style settings

---

## [1.2.0] - 2025-03-20

### Added
- QR Code and Barcode page state persistence
  - Generated results are now saved and restored on app restart
  - Images stored as Base64 in local state file

### Changed
- URL Encode/Decode page button layout
  - Button order changed to: URLEncode | URLDecode | Copy
  - Buttons now use text labels instead of icons for better clarity

---

## [1.1.0] - 2025-03-12

### Added
- Single instance application support
  - Prevent multiple instances from running simultaneously
  - Automatically activate existing window when attempting to launch again
  - Works correctly even when minimized to system tray
- Handwritten signature page
  - Convert signature to Base64 string
  - Adjustable pen size with smooth strokes
  - Save signature as image file (PNG, JPEG, BMP, GIF)

### Changed
- Removed balloon tip notification when minimizing to tray
- Signature page UI improvements
  - Buttons changed to icon style
  - Pen size control repositioned to bottom toolbar
  - Default pen size changed to 7
  - Removed image preview, showing Base64 string directly
- Home page cards repositioned for better visual centering

### Improved
- Cleaned up unused using statements in MainWindow.xaml.cs
- Better stroke smoothing with FitToCurve enabled

---

## [1.0.2] - 2025-03-11

### Added
- Single instance application support
  - Prevent multiple instances from running simultaneously
  - Automatically activate existing window when attempting to launch again
  - Works correctly even when minimized to system tray

### Fixed
- Balloon tip icon not displaying when minimized to tray
  - Changed from system icon to application icon for better visibility

### Improved
- Cleaned up unused using statements in MainWindow.xaml.cs

---

## [1.0.1] - 2025-03-10

### Added
- Image preview and save functionality for Base64 decoder
  - Click image to preview in full window
  - Save image to local file (PNG, JPEG, BMP, GIF)
- Close confirmation dialog with minimize to tray option
  - Remember user's choice during session
  - Ask again after app restart
- Multi-architecture build support (x86, x64, arm64)
- MSI installer with Inno Setup
- Code signing with self-signed certificate
- Application icon for exe and installer

### Changed
- Renamed application to "开发者工具" (Chinese) / "DevTools" (English)

---

## Version History

- **2.0.0** - RESTful API integration (Current)
- **1.3.0** - String escape/unescape, JSON improvements
- **1.2.0** - State persistence for QR/Barcode
- **1.1.0** - Single instance, signature feature
- **1.0.2** - Bug fixes
- **1.0.1** - Initial release with full features
