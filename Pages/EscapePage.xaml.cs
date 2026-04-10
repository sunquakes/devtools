using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using DevTools.Helpers;
using DevTools.Resources;
using MessageBox = System.Windows.MessageBox;

namespace DevTools.Pages
{
    public partial class EscapePage : Page
    {
        public EscapePage()
        {
            InitializeComponent();
            LoadState();
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
                { "InputText", InputText.Text ?? string.Empty },
                { "OutputText", OutputText.Text ?? string.Empty }
            };
            PageStateManager.SavePageState(this, state);
        }

        private void LoadState()
        {
            var state = PageStateManager.GetPageState(this);
            if (state != null)
            {
                InputText.Text = state.GetValueOrDefault("InputText", string.Empty);
                OutputText.Text = state.GetValueOrDefault("OutputText", string.Empty);
            }
        }

        private void Escape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var input = InputText.Text;
                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show(Strings.InputEmpty, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var escaped = EscapeString(input);
                OutputText.Text = escaped;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.EncodeFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Unescape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var input = InputText.Text;
                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show(Strings.InputEmpty, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var unescaped = UnescapeString(input);
                OutputText.Text = unescaped;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.DecodeFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyOutput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var output = OutputText.Text;
                if (string.IsNullOrWhiteSpace(output))
                {
                    MessageBox.Show(Strings.OutputEmpty, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ClipboardHelper.CopyWithFeedback(output, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.SaveFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string EscapeString(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                switch (c)
                {
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\'':
                        sb.Append("\\'");
                        break;
                    default:
                        if (c >= 32 && c <= 126)
                        {
                            sb.Append(c);
                        }
                        else
                        {
                            sb.Append($"\\u{(int)c:X4}");
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        private string UnescapeString(string input)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                
                if (c == '\\' && i + 1 < input.Length)
                {
                    char next = input[i + 1];
                    switch (next)
                    {
                        case 'a':
                            sb.Append('\a');
                            i++;
                            break;
                        case 'b':
                            sb.Append('\b');
                            i++;
                            break;
                        case 'f':
                            sb.Append('\f');
                            i++;
                            break;
                        case 'n':
                            sb.Append('\n');
                            i++;
                            break;
                        case 'r':
                            sb.Append('\r');
                            i++;
                            break;
                        case 't':
                            sb.Append('\t');
                            i++;
                            break;
                        case 'v':
                            sb.Append('\v');
                            i++;
                            break;
                        case '\\':
                            sb.Append('\\');
                            i++;
                            break;
                        case '"':
                            sb.Append('"');
                            i++;
                            break;
                        case '\'':
                            sb.Append('\'');
                            i++;
                            break;
                        case '/':
                            // JSON 风格的转义：\/ -> /
                            sb.Append('/');
                            i++;
                            break;
                        case 'u':
                            if (i + 5 < input.Length)
                            {
                                var hex = input.Substring(i + 2, 4);
                                if (ushort.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out ushort unicode))
                                {
                                    sb.Append((char)unicode);
                                    i += 5;
                                    break;
                                }
                            }
                            sb.Append(c);
                            break;
                        default:
                            // 其他情况保留反斜杠
                            sb.Append(c);
                            break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
