using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using DevTools.Helpers;

namespace DevTools.Pages
{
    public partial class HomePage : Page
    {
        private System.Windows.Threading.DispatcherTimer _searchTimer;
        
        public HomePage()
        {
            InitializeComponent();
            
            _searchTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                FilterTools(SearchBox.Text);
            };

            LoadTabState();
        }

        private void LoadTabState()
        {
            var state = PageStateManager.GetPageState(this);
            if (state != null && state.TryGetValue("SelectedTabIndex", out var indexStr) && int.TryParse(indexStr, out var index))
            {
                if (index >= 0 && index < MainTabControl.Items.Count)
                {
                    MainTabControl.SelectedIndex = index;
                }
            }
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var state = new Dictionary<string, string>
            {
                { "SelectedTabIndex", MainTabControl.SelectedIndex.ToString() }
            };
            PageStateManager.SavePageState(this, state);
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

        private void BtnTimestamp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TimestampPage());
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
            // Debounce: restart timer on each text change
            _searchTimer.Stop();
            _searchTimer.Start();
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
                SetAllButtonsVisibility(Visibility.Visible);
                SetAllTabsVisibility(Visibility.Visible);
                return;
            }

            var searchLower = searchText.ToLower();
            
            // Get TabControl and current selected tab first
            var tabControl = FindVisualChildren<System.Windows.Controls.TabControl>(this).FirstOrDefault();
            var currentSelectedTab = tabControl?.SelectedItem as TabItem;
            
            // Filter buttons in all tabs
            var allButtons = FindVisualChildren<System.Windows.Controls.Button>(this);
            
            foreach (var button in allButtons)
            {
                if (button.Name == "BtnSettings") continue;
                
                var buttonName = button.Name?.ToLower() ?? "";
                var buttonTag = button.Tag?.ToString()?.ToLower() ?? "";
                var buttonText = GetButtonText(button)?.ToLower() ?? "";
                
                var isVisible = buttonName.Contains(searchLower) || 
                               buttonTag.Contains(searchLower) ||
                               buttonText.Contains(searchLower);
                
                button.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            
            // Filter tabs and find first visible tab
            var allTabs = FindVisualChildren<TabItem>(this).ToList();
            TabItem? firstVisibleTab = null;
            
            foreach (var tab in allTabs)
            {
                // Check if tab contains any visible buttons
                var tabButtons = FindVisualChildren<System.Windows.Controls.Button>(tab);
                var hasVisibleButton = tabButtons.Any(b => b.Visibility == Visibility.Visible && b.Name != "BtnSettings");
                
                // Check tab's own header and tag
                var tabHeader = (tab.Header as string)?.ToLower() ?? "";
                var tabTag = tab.Tag?.ToString()?.ToLower() ?? "";
                var tabMatchesSearch = tabHeader.Contains(searchLower) || tabTag.Contains(searchLower);
                
                var isTabVisible = hasVisibleButton || tabMatchesSearch;
                tab.Visibility = isTabVisible ? Visibility.Visible : Visibility.Collapsed;
                
                if (isTabVisible && firstVisibleTab == null)
                {
                    firstVisibleTab = tab;
                }
            }
            
            // Only auto-select if current tab is not visible
            if (tabControl != null && firstVisibleTab != null && currentSelectedTab != null)
            {
                if (currentSelectedTab.Visibility != Visibility.Visible)
                {
                    tabControl.SelectedItem = firstVisibleTab;
                }
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

            // First check logical tree (works for all tabs, not just selected)
            foreach (var child in LogicalTreeHelper.GetChildren(depObj))
            {
                if (child is T typedChild)
                {
                    yield return typedChild;
                }

                if (child is DependencyObject childDepObj)
                {
                    foreach (var childOfChild in FindVisualChildren<T>(childDepObj))
                    {
                        yield return childOfChild;
                    }
                }
            }

            // Fallback to visual tree for templates (only if it's a Visual or Visual3D)
            if (depObj is Visual || depObj is Visual3D)
            {
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
}