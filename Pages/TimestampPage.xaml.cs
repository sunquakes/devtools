using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DevTools.Helpers;
using DevTools.Resources;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace DevTools.Pages
{
    public partial class TimestampPage : Page
    {
        public TimestampPage()
        {
            InitializeComponent();
            LoadState();
            RefreshTimestamp();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            SaveState();
            NavigationService?.Navigate(new HomePage());
        }

        private void SaveState()
        {
            var state = new Dictionary<string, string>
            {
                { "InputTimestamp", InputTimestamp.Text ?? string.Empty },
                { "OutConvertDateTime", OutConvertDateTime.Text ?? string.Empty }
            };
            PageStateManager.SavePageState(this, state);
        }

        private void LoadState()
        {
            var state = PageStateManager.GetPageState(this);
            if (state != null)
            {
                InputTimestamp.Text = state.GetValueOrDefault("InputTimestamp", string.Empty);
                OutConvertDateTime.Text = state.GetValueOrDefault("OutConvertDateTime", string.Empty);
            }
        }

        private void RefreshTimestamp()
        {
            var now = DateTime.Now;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var diff = now.ToUniversalTime() - epoch;

            OutMs.Text = ((long)diff.TotalMilliseconds).ToString();
            OutSec.Text = ((long)diff.TotalSeconds).ToString();
            OutDateTime.Text = now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTimestamp();
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            var input = InputTimestamp.Text?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show(Strings.InputEmpty, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (long.TryParse(input, out var timestamp))
            {
                try
                {
                    DateTime dateTime;
                    if (input.Length >= 13)
                    {
                        dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timestamp);
                    }
                    else
                    {
                        dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
                    }
                    OutConvertDateTime.Text = dateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                    MessageBox.Show(Strings.DecodeFailed, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(Strings.DecodeFailed, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyMs_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OutMs.Text))
            {
                MessageBox.Show(Strings.CopyEmpty, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ClipboardHelper.CopyWithFeedback(OutMs.Text, (Button)sender);
        }

        private void CopySec_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OutSec.Text))
            {
                MessageBox.Show(Strings.CopyEmpty, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ClipboardHelper.CopyWithFeedback(OutSec.Text, (Button)sender);
        }

        private void CopyDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OutDateTime.Text))
            {
                MessageBox.Show(Strings.CopyEmpty, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ClipboardHelper.CopyWithFeedback(OutDateTime.Text, (Button)sender);
        }

        private void CopyConvertDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OutConvertDateTime.Text))
            {
                MessageBox.Show(Strings.CopyEmpty, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ClipboardHelper.CopyWithFeedback(OutConvertDateTime.Text, (Button)sender);
        }
    }
}