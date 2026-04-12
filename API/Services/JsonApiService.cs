using System;
using System.Text.Json;
using DevTools.API.Models;

namespace DevTools.API.Services
{
    public class JsonApiService : IJsonApiService
    {
        private const int MaxNestingDepth = 20;

        public JsonFormatResult FormatJson(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json, new JsonDocumentOptions
                {
                    MaxDepth = MaxNestingDepth
                });
                var root = doc.RootElement;

                var options = new JsonSerializerOptions { WriteIndented = true };
                var formatted = JsonSerializer.Serialize(root, options);

                return new JsonFormatResult
                {
                    Input = json,
                    Formatted = formatted,
                    IsValid = true
                };
            }
            catch (JsonException ex)
            {
                return new JsonFormatResult
                {
                    Input = json,
                    Formatted = string.Empty,
                    IsValid = false,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new JsonFormatResult
                {
                    Input = json,
                    Formatted = string.Empty,
                    IsValid = false,
                    ErrorMessage = $"Unknown error: {ex.Message}"
                };
            }
        }

        public bool ValidateJson(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json, new JsonDocumentOptions
                {
                    MaxDepth = MaxNestingDepth
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
