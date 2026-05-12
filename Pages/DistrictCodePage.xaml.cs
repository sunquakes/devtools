using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DevTools.Helpers;
using DevTools.Services;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace DevTools.Pages
{
    public partial class DistrictCodePage : Page
    {
        public DistrictCodePage()
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
                { "SearchInput", SearchInput.Text ?? string.Empty }
            };
            PageStateManager.SavePageState(this, state);
        }

        private void LoadState()
        {
            var state = PageStateManager.GetPageState(this);
            if (state != null)
            {
                SearchInput.Text = state.GetValueOrDefault("SearchInput", string.Empty);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var keyword = SearchInput.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("请输入搜索关键词", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var results = DistrictCodeService.Instance.SearchByKeyword(keyword);
            ResultListView.ItemsSource = results;

            if (results.Count == 0)
            {
                MessageBox.Show("未找到匹配的结果", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
