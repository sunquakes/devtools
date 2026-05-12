using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DevTools.Resources;
using ZXing;
using ZXing.Windows.Compatibility;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace DevTools.Pages
{
    public partial class QrBarcodeDecodePage : Page
    {
        private Bitmap? _currentBitmap;
        private bool _suppressClick;

        public QrBarcodeDecodePage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }

        private void ImageBorder_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_suppressClick)
            {
                _suppressClick = false;
                e.Handled = true;
                return;
            }
            e.Handled = true;
            UploadImage();
        }

        private void UploadImage()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    using var fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);
                    var bitmap = new Bitmap(fs);
                    SetImage(bitmap);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Strings.QrBarcodeDecodeFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PasteFromClipboard()
        {
            if (System.Windows.Clipboard.ContainsImage())
            {
                try
                {
                    var imageSource = System.Windows.Clipboard.GetImage();
                    if (imageSource != null)
                    {
                        var bitmap = ImageSourceToBitmap(imageSource);
                        if (bitmap != null)
                        {
                            SetImage(bitmap);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Strings.QrBarcodeDecodeFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(Strings.NoImageInClipboard, Strings.Info, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetImage(Bitmap bitmap)
        {
            _currentBitmap?.Dispose();
            _currentBitmap = bitmap;

            var imageSource = BitmapToImageSource(bitmap);
            PreviewImage.Source = imageSource;
            PreviewImage.Visibility = Visibility.Visible;
            ImageScrollViewer.Visibility = Visibility.Visible;
            PlaceholderPanel.Visibility = Visibility.Collapsed;

            DecodeImage(bitmap);
        }

        private void DecodeImage(Bitmap bitmap)
        {
            try
            {
                var reader = new BarcodeReader
                {
                    AutoRotate = true,
                    Options = new ZXing.Common.DecodingOptions
                    {
                        TryHarder = true,
                        PossibleFormats = new[]
                        {
                            ZXing.BarcodeFormat.QR_CODE,
                            ZXing.BarcodeFormat.CODE_128,
                            ZXing.BarcodeFormat.CODE_39,
                            ZXing.BarcodeFormat.EAN_13,
                            ZXing.BarcodeFormat.EAN_8,
                            ZXing.BarcodeFormat.UPC_A,
                            ZXing.BarcodeFormat.UPC_E,
                            ZXing.BarcodeFormat.CODABAR,
                            ZXing.BarcodeFormat.ITF,
                            ZXing.BarcodeFormat.DATA_MATRIX,
                            ZXing.BarcodeFormat.PDF_417
                        }
                    }
                };

                var result = reader.Decode(bitmap);

                if (result != null && !string.IsNullOrEmpty(result.Text))
                {
                    ResultText.Text = result.Text;
                }
                else
                {
                    ResultText.Text = Strings.NoResult;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.QrBarcodeDecodeFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Bitmap? ImageSourceToBitmap(BitmapSource source)
        {
            if (source == null) return null;

            var format = source.Format;

            var width = source.PixelWidth;
            var height = source.PixelHeight;
            var stride = width * ((format.BitsPerPixel + 7) / 8);
            var bits = new byte[height * stride];

            source.CopyPixels(bits, stride, 0);

            var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            try
            {
                System.Runtime.InteropServices.Marshal.Copy(bits, 0, bitmapData.Scan0, bits.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            return bitmap;
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

        private void CopyResult_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ResultText.Text) && ResultText.Text != Strings.NoResult)
            {
                System.Windows.Clipboard.SetText(ResultText.Text);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _currentBitmap?.Dispose();
            _currentBitmap = null;
            PreviewImage.Source = null;
            PreviewImage.Visibility = Visibility.Collapsed;
            ImageScrollViewer.Visibility = Visibility.Collapsed;
            PlaceholderPanel.Visibility = Visibility.Visible;
            ResultText.Text = string.Empty;
        }

        private void ImageBorder_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop) || e.Data.GetDataPresent(System.Windows.DataFormats.Bitmap))
            {
                e.Effects = System.Windows.DragDropEffects.Copy;
            }
            else
            {
                e.Effects = System.Windows.DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void ImageBorder_PreviewDragLeave(object sender, System.Windows.DragEventArgs e)
        {
            e.Handled = true;
        }

        private void ImageBorder_Drop(object sender, System.Windows.DragEventArgs e)
        {
            _suppressClick = true;
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => _suppressClick = false), System.Windows.Threading.DispatcherPriority.Input);
            try
            {
                if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
                {
                    var files = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
                    if (files != null && files.Length > 0)
                    {
                        var filePath = files[0];
                        var ext = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
                        if (ext is ".png" or ".jpg" or ".jpeg" or ".bmp" or ".gif")
                        {
                            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                            var bitmap = new Bitmap(fs);
                            SetImage(bitmap);
                        }
                    }
                }
                else if (e.Data.GetDataPresent(System.Windows.DataFormats.Bitmap))
                {
                    var imageSource = e.Data.GetData(System.Windows.DataFormats.Bitmap) as BitmapSource;
                    if (imageSource != null)
                    {
                        var bitmap = ImageSourceToBitmap(imageSource);
                        if (bitmap != null)
                        {
                            SetImage(bitmap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.QrBarcodeDecodeFailed}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            e.Handled = true;
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.V && System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                PasteFromClipboard();
                e.Handled = true;
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}
