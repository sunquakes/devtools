using System;
using System.Threading.Tasks;
using DevTools.API.Server;

namespace DevTools.API
{
    public class ApiManager
    {
        private static ApiManager? _instance;
        private static readonly object Lock = new();
        private HttpApiServer? _server;
        private bool _isInitialized;

        private ApiManager()
        {
        }

        public static ApiManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ApiManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public bool IsRunning => _server?.IsRunning ?? false;
        public int Port => _server?.Port ?? 0;

        public async Task<bool> InitializeAsync(int port = 5000)
        {
            if (_isInitialized)
            {
                return IsRunning;
            }

            try
            {
                var settings = new ApiServerSettings
                {
                    Port = port,
                    EnableCors = true
                };

                _server = new HttpApiServer(settings);
                var result = await _server.StartAsync();

                if (result)
                {
                    _isInitialized = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Shutdown()
        {
            _server?.Stop();
            _isInitialized = false;
        }

        public async Task<bool> RestartAsync(int port = 5000)
        {
            Shutdown();
            return await InitializeAsync(port);
        }
    }
}
