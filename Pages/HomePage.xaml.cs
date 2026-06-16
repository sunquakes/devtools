using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;

namespace DevTools.Pages
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void BtnMd5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Md5Page());
        }

        private void BtnBarcode_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BarcodePage());
        }

        private void BtnQr_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new QrPage());
        }

        private void BtnBase64ToImage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Base64ImagePage());
        }

        private void BtnJsonFormat_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new JsonFormatPage());
        }

        private void BtnImageToBase64_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ImageToBase64Page());
        }

        private void BtnSignature_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SignaturePage());
        }

        private void BtnUrlEncode_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UrlEncodePage());
        }

        private void BtnEscape_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EscapePage());
        }

        private void BtnQrBarcodeDecode_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new QrBarcodeDecodePage());
        }

        private void BtnRandomString_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RandomStringPage());
        }

        private void BtnDistrictCode_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DistrictCodePage());
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SettingsPage());
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterTools(SearchBox.Text);
        }

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                FilterTools(SearchBox.Text);
            }
        }

        private void FilterTools(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Show all buttons and tabs
                SetAllButtonsVisibility(Visibility.Visible);
                SetAllTabsVisibility(Visibility.Visible);
                return;
            }

            var searchLower = searchText.ToLower();
            
            // Filter tabs based on their headers
            var allTabs = FindVisualChildren<TabItem>(this);
            foreach (var tab in allTabs)
            {
                // Get tab header text - handle both string and resource binding
                string tabHeader = "";
                if (tab.Header is string headerString)
                {
                    tabHeader = headerString.ToLower();
                }
                else
                {
                    // Try to get text from TextBlock if header is a visual element
                    var textBlock = FindVisualChildren<TextBlock>(tab).FirstOrDefault();
                    if (textBlock != null)
                    {
                        tabHeader = textBlock.Text?.ToLower() ?? "";
                    }
                }
                
                var tabVisible = tabHeader.Contains(searchLower);
                tab.Visibility = tabVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            
            // Filter buttons in all tabs
            var allButtons = FindVisualChildren<System.Windows.Controls.Button>(this);
            var visibleButtonCount = 0;
            
            foreach (var button in allButtons)
            {
                if (button.Name == "BtnSettings") continue; // Skip settings button
                
                var buttonName = button.Name?.ToLower() ?? "";
                var buttonTag = button.Tag?.ToString()?.ToLower() ?? "";
                
                // Get button text content (supports both Chinese and English)
                var buttonText = GetButtonText(button)?.ToLower() ?? "";
                
                // Check if button name, tag, or text contains search text
                var isVisible = buttonName.Contains(searchLower) || 
                               buttonTag.Contains(searchLower) ||
                               buttonText.Contains(searchLower);
                
                button.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                
                if (isVisible) visibleButtonCount++;
            }
            
            // Adjust alignment based on visible button count
            // If only one button is visible, align center; otherwise left
            var allWrapPanels = FindVisualChildren<WrapPanel>(this);
            foreach (var wrapPanel in allWrapPanels)
            {
                wrapPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }
        }

        private string GetButtonText(System.Windows.Controls.Button button)
        {
            if (button == null) return "";
            
            // Try to find TextBlock in button's content
            var textBlocks = FindVisualChildren<System.Windows.Controls.TextBlock>(button);
            foreach (var textBlock in textBlocks)
            {
                // Skip icon text blocks (they only contain FontAwesome codes)
                var text = textBlock.Text?.Trim() ?? "";
                if (!string.IsNullOrWhiteSpace(text) && !text.StartsWith("&#xf"))
                {
                    return text;
                }
            }
            
            return "";
        }

        private void SetAllButtonsVisibility(Visibility visibility)
        {
            var allButtons = FindVisualChildren<System.Windows.Controls.Button>(this);
            var buttonCount = 0;
            foreach (var button in allButtons)
            {
                if (button.Name == "BtnSettings") continue;
                button.Visibility = visibility;
                buttonCount++;
            }
            
            // Adjust alignment: single button center, multiple buttons left
            var allWrapPanels = FindVisualChildren<WrapPanel>(this);
            foreach (var wrapPanel in allWrapPanels)
            {
                wrapPanel.HorizontalAlignment = buttonCount <= 1 ? System.Windows.HorizontalAlignment.Center : System.Windows.HorizontalAlignment.Left;
            }
        }

        private void SetAllTabsVisibility(Visibility visibility)
        {
            var allTabs = FindVisualChildren<TabItem>(this);
            foreach (var tab in allTabs)
            {
                tab.Visibility = visibility;
            }
        }

        private static System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                if (child != null)
                {
                    foreach (var childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}