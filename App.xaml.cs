using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace DevTools
{
    public partial class App : Application
    {
        private static readonly string MutexName = "DevTools_SingleInstance_Mutex";
        private static System.Threading.Mutex? _mutex;

        private const int HWND_BROADCAST = 0xffff;
        private static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME_DEVTOOLS");

        [DllImport("user32.dll")]
        private static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        public static int ShowMeMessage => WM_SHOWME;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _mutex = new System.Threading.Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                ActivateExistingInstance();
                Shutdown();
                return;
            }

            // Apply saved language setting
            ApplyLanguageSetting();

            base.OnStartup(e);
        }

        private void ApplyLanguageSetting()
        {
            try
            {
                var lang = DevTools.Properties.Settings.Default.Language;
                if (!string.IsNullOrEmpty(lang))
                {
                    var culture = new CultureInfo(lang);
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                }
            }
            catch { }
        }

        private void ActivateExistingInstance()
        {
            PostMessage((IntPtr)HWND_BROADCAST, WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Shutdown background services
            DevTools.API.ApiManager.Instance.Shutdown();
            
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }
}
