using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DevTools.Resources;
using MessageBox = System.Windows.MessageBox;

namespace DevTools.Pages
{
    public partial class RandomStringPage : Page
    {
        public RandomStringPage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(CountText.Text, out int count) || count <= 0)
            {
                count = 1;
            }
            if (!int.TryParse(LengthText.Text, out int length) || length <= 0)
            {
                length = 8;
            }

            string charset = BuildCharset();
            if (string.IsNullOrEmpty(charset))
            {
                MessageBox.Show(Strings.SelectAtLeastOneOption, Strings.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var sb = new StringBuilder();
            using var rng = RandomNumberGenerator.Create();
            var buffer = new byte[sizeof(uint)];

            for (int i = 0; i < count; i++)
            {
                var lineSb = new StringBuilder();
                for (int j = 0; j < length; j++)
                {
                    rng.GetBytes(buffer);
                    uint num = BitConverter.ToUInt32(buffer, 0);
                    lineSb.Append(charset[(int)(num % (uint)charset.Length)]);
                }
                sb.AppendLine(lineSb.ToString());
            }

            ResultText.Text = sb.ToString().TrimEnd();
        }

        private string BuildCharset()
        {
            var sb = new StringBuilder();
            if (LowercaseCheck.IsChecked == true)
            {
                sb.Append("abcdefghijklmnopqrstuvwxyz");
            }
            if (UppercaseCheck.IsChecked == true)
            {
                sb.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            }
            if (NumbersCheck.IsChecked == true)
            {
                sb.Append("0123456789");
            }
            if (SpecialCheck.IsChecked == true)
            {
                sb.Append("!@#$%^&*()_+-=[]{}|;:,.<>?/~`");
            }
            return sb.ToString();
        }

        private void CopyResult_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ResultText.Text))
            {
                System.Windows.Clipboard.SetText(ResultText.Text);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ResultText.Text = string.Empty;
        }
    }
}
