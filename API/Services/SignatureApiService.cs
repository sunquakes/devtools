using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevTools.API.Models;
using WpfPoint = System.Windows.Point;

namespace DevTools.API.Services
{
    public class SignatureApiService : ISignatureApiService
    {
        public SignatureResult CreateSignatureFromStrokes(List<StrokePoint> strokes, int width, int height, double penWidth)
        {
            try
            {
                var drawingVisual = new DrawingVisual();
                using (var drawingContext = drawingVisual.RenderOpen())
                {
                    var pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, penWidth);

                    var strokePoints = new List<WpfPoint>();
                    foreach (var point in strokes)
                    {
                        if (point.IsDown)
                        {
                            strokePoints.Add(new WpfPoint(point.X, point.Y));
                        }
                        else
                        {
                            if (strokePoints.Count > 1)
                            {
                                var geometry = new StreamGeometry();
                                using (var ctx = geometry.Open())
                                {
                                    ctx.BeginFigure(strokePoints[0], false, false);
                                    ctx.PolyLineTo(strokePoints, true, false);
                                }
                                drawingContext.DrawGeometry(null, pen, geometry);
                                strokePoints.Clear();
                            }
                        }
                    }

                    if (strokePoints.Count > 1)
                    {
                        var geometry = new StreamGeometry();
                        using (var ctx = geometry.Open())
                        {
                            ctx.BeginFigure(strokePoints[0], false, false);
                            ctx.PolyLineTo(strokePoints, true, false);
                        }
                        drawingContext.DrawGeometry(null, pen, geometry);
                    }
                }

                var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(drawingVisual);
                bitmap.Freeze();

                using var ms = new MemoryStream();
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(ms);
                var imageBytes = ms.ToArray();
                var base64 = Convert.ToBase64String(imageBytes);

                return new SignatureResult
                {
                    ImageBase64 = base64,
                    Format = "PNG",
                    Width = width,
                    Height = height
                };
            }
            catch (Exception ex)
            {
                return new SignatureResult
                {
                    ImageBase64 = string.Empty,
                    Format = "PNG",
                    Width = width,
                    Height = height
                };
            }
        }
    }
}
