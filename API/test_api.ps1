# DevTools API Test Script

# Test Health Check
Write-Host "Testing Health Check..." -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/health" -Method Get
Write-Host "Health Check Result: $($response.success)" -ForegroundColor Green
Write-Host ""

# Test MD5
Write-Host "Testing MD5..." -ForegroundColor Cyan
$body = @{ input = "hello world" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/md5" -Method Post -Body $body -ContentType "application/json"
Write-Host "MD5 Result: $($response.data.hash32Lower)" -ForegroundColor Green
Write-Host ""

# Test JSON Format
Write-Host "Testing JSON Format..." -ForegroundColor Cyan
$body = @{ json = '{"name":"John","age":30}' } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/json/format" -Method Post -Body $body -ContentType "application/json"
Write-Host "JSON Formatted:`n$($response.data.formatted)" -ForegroundColor Green
Write-Host ""

# Test URL Encode
Write-Host "Testing URL Encode..." -ForegroundColor Cyan
$body = @{ input = "Hello World!" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/url/encode" -Method Post -Body $body -ContentType "application/json"
Write-Host "URL Encoded: $($response.data.output)" -ForegroundColor Green
Write-Host ""

# Test URL Decode
Write-Host "Testing URL Decode..." -ForegroundColor Cyan
$body = @{ input = "Hello%20World!" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/url/decode" -Method Post -Body $body -ContentType "application/json"
Write-Host "URL Decoded: $($response.data.output)" -ForegroundColor Green
Write-Host ""

# Test Base64 Encode
Write-Host "Testing Base64 Encode..." -ForegroundColor Cyan
$body = @{ input = "Hello World" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/base64/encode" -Method Post -Body $body -ContentType "application/json"
Write-Host "Base64 Encoded: $($response.data.output)" -ForegroundColor Green
Write-Host ""

# Test Base64 Decode
Write-Host "Testing Base64 Decode..." -ForegroundColor Cyan
$body = @{ base64 = "SGVsbG8gV29ybGQ=" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/base64/decode" -Method Post -Body $body -ContentType "application/json"
Write-Host "Base64 Decoded: $($response.data.output)" -ForegroundColor Green
Write-Host ""

# Test String Escape
Write-Host "Testing String Escape..." -ForegroundColor Cyan
$body = @{ input = "Hello`nWorld" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/escape" -Method Post -Body $body -ContentType "application/json"
Write-Host "Escaped: $($response.data.output)" -ForegroundColor Green
Write-Host ""

# Test QR Code Generation
Write-Host "Testing QR Code Generation..." -ForegroundColor Cyan
$body = @{ text = "https://example.com"; width = 300; height = 300 } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/qrcode" -Method Post -Body $body -ContentType "application/json"
Write-Host "QR Code Generated (Base64 length): $($response.data.imageBase64.Length) characters" -ForegroundColor Green

# Save QR Code Image
$imageBytes = [Convert]::FromBase64String($response.data.imageBase64)
Save-Item -Path "test_qrcode.png" -Value $imageBytes -Force
Write-Host "QR Code saved to test_qrcode.png" -ForegroundColor Green
Write-Host ""

# Test Barcode Generation
Write-Host "Testing Barcode Generation..." -ForegroundColor Cyan
$body = @{ text = "123456789"; format = "CODE_128"; width = 400; height = 120 } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/barcode" -Method Post -Body $body -ContentType "application/json"
Write-Host "Barcode Generated (Base64 length): $($response.data.imageBase64.Length) characters" -ForegroundColor Green

# Save Barcode Image
$imageBytes = [Convert]::FromBase64String($response.data.imageBase64)
Save-Item -Path "test_barcode.png" -Value $imageBytes -Force
Write-Host "Barcode saved to test_barcode.png" -ForegroundColor Green
Write-Host ""

# Test Get Barcode Formats
Write-Host "Testing Get Barcode Formats..." -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/barcode/formats" -Method Get
Write-Host "Supported Formats: $($response.data.formats -join ', ')" -ForegroundColor Green
Write-Host ""

Write-Host "All tests completed!" -ForegroundColor Green
