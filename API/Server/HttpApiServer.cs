using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DevTools.API.Server
{
    public class ApiServerSettings
    {
        public int Port { get; set; } = 5000;
        public bool EnableCors { get; set; } = true;
        public string? AllowedOrigins { get; set; }
    }

    public class HttpApiServer : IDisposable
    {
        private HttpListener? _listener;
        private CancellationTokenSource? _cts;
        private readonly ApiServerSettings _settings;
        private readonly RequestHandler _requestHandler;
        private bool _isRunning;
        private readonly object _lock = new();

        public HttpApiServer(ApiServerSettings? settings = null)
        {
            _settings = settings ?? new ApiServerSettings();
            _requestHandler = new RequestHandler(_settings);
        }

        public bool IsRunning => _isRunning;
        public int Port => _settings.Port;

        public async Task<bool> StartAsync()
        {
            if (_isRunning)
            {
                return true;
            }

            try
            {
                _listener = new HttpListener();
                var prefix = $"http://localhost:{_settings.Port}/";
                _listener.Prefixes.Add(prefix);
                _listener.Start();

                _cts = new CancellationTokenSource();
                _isRunning = true;

                Task.Run(() => RunListener(_cts.Token));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (!_isRunning)
                {
                    return;
                }

                _isRunning = false;
                _cts?.Cancel();

                try
                {
                    _listener?.Stop();
                    _listener?.Close();
                }
                catch
                {
                }

                _listener = null;
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task RunListener(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _isRunning)
            {
                try
                {
                    var context = await _listener!.GetContextAsync();
                    _ = Task.Run(() => HandleRequest(context), cancellationToken);
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                await _requestHandler.HandleRequestAsync(context);
            }
            catch (Exception ex)
            {
                await SendErrorResponse(context, 500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task SendErrorResponse(HttpListenerContext context, int statusCode, string message)
        {
            var response = context.Response;
            response.StatusCode = statusCode;
            response.ContentType = "application/json";

            var errorResponse = $"{{\"success\":false,\"message\":\"{message.Replace("\"", "\\\"")}\",\"errorCode\":\"HTTP_{statusCode}\"}}";
            var buffer = System.Text.Encoding.UTF8.GetBytes(errorResponse);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
