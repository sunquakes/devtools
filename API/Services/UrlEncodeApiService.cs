using System;
using DevTools.API.Models;

namespace DevTools.API.Services
{
    public class UrlEncodeApiService : IUrlEncodeApiService
    {
        public UrlEncodeResult Encode(string input)
        {
            try
            {
                var encoded = Uri.EscapeDataString(input);
                return new UrlEncodeResult
                {
                    Input = input,
                    Output = encoded,
                    Operation = "encode"
                };
            }
            catch (Exception ex)
            {
                return new UrlEncodeResult
                {
                    Input = input,
                    Output = string.Empty,
                    Operation = "encode"
                };
            }
        }

        public UrlEncodeResult Decode(string input)
        {
            try
            {
                var decoded = Uri.UnescapeDataString(input);
                return new UrlEncodeResult
                {
                    Input = input,
                    Output = decoded,
                    Operation = "decode"
                };
            }
            catch (Exception ex)
            {
                return new UrlEncodeResult
                {
                    Input = input,
                    Output = string.Empty,
                    Operation = "decode"
                };
            }
        }
    }
}
