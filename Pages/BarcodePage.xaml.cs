using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using DevTools.Helpers;
using DevTools.Resources;
using ZXing;
using ZXing.Common;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using Color = System.Windows.Media.Color;
using Path = System.IO.Path;
using Mouse = System.Windows.Input.Mouse;
using Brushes = System.Windows.Media.Brushes;
using TextBox = System.Windows.Controls.TextBox;

namespace DevTools.Pages
{
    public partial class BarcodePage : Page
    {
        private readonly ObservableCollection<BarcodeLogEntry> _logs = new();
        private BarcodeLogEntry? _draggedItem;
        private Point _startPoint;
        private bool _isDragging;
        private int _draggedIndex = -1;
        private Border? _dragPreview;

        public BarcodePage()
        {
            InitializeComponent();
            LogsList.ItemsSource = _logs;
            Unloaded += BarcodePage_Unloaded;
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
                { "InputText", InputText.Text ?? string.Empty }
            };

            var logsData = new List<Dictionary<string, string>>();
            foreach (var entry in _logs)
            {
                if (entry.Bitmap != null)
                {
                    using var ms = new MemoryStream();
                    entry.Bitmap.Save(ms, ImageFormat.Png);
                    var base64 = Convert.ToBase64String(ms.ToArray());
                    logsData.Add(new Dictionary<string, string>
                    {
                        { "Text", entry.Text ?? string.Empty },
                        { "Timestamp", entry.Timestamp.ToString("O") },
                        { "ImageBase64", base64 },
                        { "IsImageVisible", entry.IsImageVisible.ToString() },
                        { "Remark", entry.Remark ?? "双击修改备注" }
                    });
                }
            }

            if (logsData.Count > 0)
            {
                state["Logs"] = System.Text.Json.JsonSerializer.Serialize(logsData);
            }

            PageStateManager.SavePageState(this, state);
        }

        private void LoadState()
        {
            var state = PageStateManager.GetPageState(this);
            if (state != null)
            {
                InputText.Text = state.GetValueOrDefault("InputText", string.Empty);

                if (state.TryGetValue("Logs", out var logsJson) && !string.IsNullOrEmpty(logsJson))
                {
                    try
                    {
                        var logsData = System.Text.Json.JsonSerializer.Deserialize<List<Dictionary<string, string>>>(logsJson);
                        if (logsData != null)
                        {
                            foreach (var logData in logsData)
                            {
                                var text = logData.GetValueOrDefault("Text", string.Empty);
                                var timestampStr = logData.GetValueOrDefault("Timestamp", string.Empty);
                                var base64 = logData.GetValueOrDefault("ImageBase64", string.Empty);
                                var isVisibleStr = logData.GetValueOrDefault("IsImageVisible", "true");

                                if (DateTime.TryParse(timestampStr, out var timestamp) && !string.IsNullOrEmpty(base64))
                                {
                                    var bytes = Convert.FromBase64String(base64);
                                    using var ms = new MemoryStream(bytes);
                                    var bitmap = new Bitmap(ms);
                                    var imageSource = BitmapToImageSource(bitmap);

                                    var entry = new BarcodeLogEntry
                                    {
                                        Bitmap = bitmap,
                                        Image = imageSource,
                                        Text = text,
                                        Timestamp = timestamp,
                                        IsImageVisible = bool.Parse(isVisibleStr),
                                        Remark = logData.GetValueOrDefault("Remark", "双击修改备注")
                                    };
                                    entry.TimestampString = entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                                    _logs.Add(entry);
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var text = InputText.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(Strings.EnterTextToEncode, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Height = 120,
                        Width = 400,
                        Margin = 10
                    }
                };

                var pixelData = writer.Write(text);

                using var bmp = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                var bitmapData = bmp.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bmp.UnlockBits(bitmapData);
                }

                var storedBmp = new Bitmap(bmp);
                var imageSource = BitmapToImageSource(storedBmp);

                var entry = new BarcodeLogEntry
                {
                    Bitmap = storedBmp,
                    Image = imageSource,
                    Text = text,
                    Timestamp = DateTime.Now,
                    IsImageVisible = true
                };

                entry.TimestampString = entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");

                _logs.Insert(0, entry);
                InputText.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.BarcodeGenerateFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.DataContext is not BarcodeLogEntry entry) return;

            if (entry.Bitmap == null)
            {
                MessageBox.Show(Strings.NoBarcodeToSave, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PNG files (*.png)|*.png|JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|Bitmap (*.bmp)|*.bmp",
                FileName = "barcode.png"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var ext = Path.GetExtension(dlg.FileName).ToLowerInvariant();
                    ImageFormat fmt = ImageFormat.Png;
                    if (ext == ".jpg" || ext == ".jpeg") fmt = ImageFormat.Jpeg;
                    else if (ext == ".bmp") fmt = ImageFormat.Bmp;

                    entry.Bitmap.Save(dlg.FileName, fmt);
                    MessageBox.Show(Strings.SaveSuccess, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Strings.SaveFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ToggleVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.DataContext is not BarcodeLogEntry entry) return;
            entry.IsImageVisible = !entry.IsImageVisible;
        }

        private void ShowOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.DataContext is not BarcodeLogEntry entry) return;

            foreach (var l in _logs)
            {
                l.IsImageVisible = l == entry;
            }
        }

        private void DeleteLog_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.DataContext is not BarcodeLogEntry entry) return;

            if (_logs.Contains(entry))
            {
                _logs.Remove(entry);
                try { entry.Bitmap?.Dispose(); } catch { }
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var l in _logs) { try { l.Bitmap?.Dispose(); } catch { } }
            _logs.Clear();
        }

        private BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            var handle = bitmap.GetHbitmap();
            try
            {
                var source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    handle,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                source.Freeze();
                return source;
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        private void BarcodePage_Unloaded(object? sender, RoutedEventArgs e)
        {
            foreach (var l in _logs)
            {
                try
                {
                    l.Bitmap?.Dispose();
                }
                catch { }
            }
            _logs.Clear();
        }

        private void DragHandle_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is BarcodeLogEntry entry)
            {
                _draggedItem = entry;
                _draggedIndex = _logs.IndexOf(entry);
                _startPoint = e.GetPosition(null);
                Mouse.Capture(border);
                e.Handled = true;
            }
        }

        private void DragHandle_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_draggedItem == null || e.LeftButton != System.Windows.Input.MouseButtonState.Pressed)
            {
                if (_draggedItem != null && e.LeftButton != System.Windows.Input.MouseButtonState.Pressed)
                {
                    CleanupDrag();
                }
                return;
            }

            var currentPoint = e.GetPosition(null);
            var diff = currentPoint - _startPoint;

            if (!_isDragging && (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || 
                                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                _isDragging = true;
                
                _dragPreview = new Border
                {
                    Background = new SolidColorBrush(Color.FromArgb(230, 45, 45, 45)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(12, 8, 12, 8),
                    Child = new TextBlock
                    {
                        Text = _draggedItem.Text ?? "",
                        Foreground = Brushes.White,
                        FontSize = 13,
                        MaxWidth = 300,
                        TextTrimming = TextTrimming.CharacterEllipsis
                    }
                };
                
                DragOverlay.Children.Add(_dragPreview);
            }
            
            if (_isDragging && _dragPreview != null)
            {
                var pos = e.GetPosition(DragOverlay);
                Canvas.SetLeft(_dragPreview, pos.X + 10);
                Canvas.SetTop(_dragPreview, pos.Y + 10);
            }
        }

        private void DragHandle_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isDragging && _draggedItem != null && _draggedIndex >= 0)
            {
                // 确保布局已更新
                LogsList.UpdateLayout();
                
                var mousePos = e.GetPosition(LogsList);
                var targetItem = FindItemAtPosition(mousePos);
                
                if (targetItem != null && targetItem != _draggedItem)
                {
                    var targetIndex = _logs.IndexOf(targetItem);
                    if (targetIndex >= 0 && _draggedIndex != targetIndex)
                    {
                        var item = _logs[_draggedIndex];
                        _logs.RemoveAt(_draggedIndex);
                        _logs.Insert(targetIndex, item);
                        
                        // 交换后再次更新布局
                        LogsList.UpdateLayout();
                    }
                }
            }
            
            CleanupDrag();
        }

        private void CleanupDrag()
        {
            Mouse.Capture(null);
            
            if (_dragPreview != null)
            {
                DragOverlay.Children.Remove(_dragPreview);
                _dragPreview = null;
            }
            
            _isDragging = false;
            _draggedItem = null;
            _draggedIndex = -1;
        }

        private DateTime _lastRemarkClickTime = DateTime.MinValue;
        private object? _lastRemarkClickSender;

        private void RemarkBorder_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton != System.Windows.Input.MouseButton.Left)
                return;

            if (sender is Border border && border.DataContext is BarcodeLogEntry entry)
            {
                var now = DateTime.Now;
                var timeSinceLastClick = (now - _lastRemarkClickTime).TotalMilliseconds;
                
                if (timeSinceLastClick < 500 && timeSinceLastClick > 50 && _lastRemarkClickSender == sender)
                {
                    entry.IsEditingRemark = true;
                    
                    var grid = border.Child as Grid;
                    if (grid != null)
                    {
                        foreach (var child in grid.Children)
                        {
                            if (child is TextBox textBox)
                            {
                                textBox.Focus();
                                textBox.SelectAll();
                                break;
                            }
                        }
                    }
                    
                    _lastRemarkClickTime = DateTime.MinValue;
                    _lastRemarkClickSender = null;
                    e.Handled = true;
                }
                else
                {
                    _lastRemarkClickTime = now;
                    _lastRemarkClickSender = sender;
                }
            }
        }

        private void RemarkTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is BarcodeLogEntry entry)
            {
                entry.IsEditingRemark = false;
                
                if (string.IsNullOrWhiteSpace(entry.Remark))
                {
                    entry.Remark = "双击修改备注";
                }
            }
        }

        private void RemarkTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (sender is TextBox textBox && textBox.DataContext is BarcodeLogEntry entry)
                {
                    entry.IsEditingRemark = false;
                    
                    if (string.IsNullOrWhiteSpace(entry.Remark))
                    {
                        entry.Remark = "双击修改备注";
                    }
                    
                    e.Handled = true;
                }
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                if (sender is TextBox textBox && textBox.DataContext is BarcodeLogEntry entry)
                {
                    entry.IsEditingRemark = false;
                    e.Handled = true;
                }
            }
        }

        private BarcodeLogEntry? FindItemAtPosition(Point position)
        {
            if (LogsList == null) return null;
            
            // 遍历所有项，查找鼠标位置下方的项
            for (int i = 0; i < _logs.Count; i++)
            {
                var container = LogsList.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                if (container == null) continue;
                
                container.UpdateLayout();
                
                var itemPos = container.TranslatePoint(new Point(0, 0), LogsList);
                var itemRect = new Rect(itemPos.X, itemPos.Y, container.ActualWidth, container.ActualHeight);
                
                if (itemRect.Contains(position))
                {
                    return _logs[i];
                }
            }
            
            return null;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);
    }

    internal class BarcodeLogEntry : INotifyPropertyChanged
    {
        private Bitmap? _bitmap;
        private BitmapSource? _image;
        private string? _text;
        private DateTime _timestamp;
        private string? _timestampString;
        private bool _isImageVisible;
        private string? _remark;
        private bool _isEditingRemark;

        public BarcodeLogEntry()
        {
            _remark = "双击修改备注";
            _isEditingRemark = false;
        }

        public Bitmap? Bitmap { get => _bitmap; set { _bitmap = value; OnPropertyChanged(nameof(Bitmap)); } }
        public BitmapSource? Image { get => _image; set { _image = value; OnPropertyChanged(nameof(Image)); } }
        public string? Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }
        public DateTime Timestamp { get => _timestamp; set { _timestamp = value; OnPropertyChanged(nameof(Timestamp)); } }
        public string? TimestampString { get => _timestampString; set { _timestampString = value; OnPropertyChanged(nameof(TimestampString)); } }
        public bool IsImageVisible { get => _isImageVisible; set { _isImageVisible = value; OnPropertyChanged(nameof(IsImageVisible)); } }
        public string? Remark { get => _remark; set { _remark = value; OnPropertyChanged(nameof(Remark)); } }
        public bool IsEditingRemark { get => _isEditingRemark; set { _isEditingRemark = value; OnPropertyChanged(nameof(IsEditingRemark)); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class BoolToHiddenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool b && b) return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility v) return v == Visibility.Visible;
            return false;
        }
    }

    public class InverseBoolToHiddenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool b && !b) return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility v) return v != Visibility.Visible;
            return false;
        }
    }
}
