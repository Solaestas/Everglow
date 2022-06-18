using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
namespace Everglow.Sources.Commons.Function.ImageReader;

struct ImageKeyPoint
{
    public int Row;
    public int Column;
};

internal static class ImageReader
{
    private static string ConvertImagePath(string path)
    {
        if (Path.GetExtension(path) == string.Empty)
        {
            path = Path.ChangeExtension(path, ".bmp");
        }
        return path;
    }
    public static List<ImageKeyPoint> ReadImageKeyPoints(string path, Rgb24 targetColor)
    {
        List<ImageKeyPoint> keyPoints = new List<ImageKeyPoint>();
        using var image = Read<Rgb24>(path);
        image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        if (pixelRow[x] == targetColor)
                        {
                            keyPoints.Add(new ImageKeyPoint() { Row = y, Column = x });
                        }
                    }
                }
            });
        return keyPoints;
    }

    /// <summary>
    /// RGB通道默认值0，Alpha默认值255，默认路径名为.bmp
    /// </summary>
    /// <param name="path">路径，可以不带后缀名</param>
    /// <returns></returns>
    public static Image<TPixel> Read<TPixel>(string path) where TPixel : unmanaged, IPixel<TPixel>
    {
        path = ConvertImagePath(path);
        using var memoryStream = new MemoryStream(ModContent.GetFileBytes(path));
        using var image = Image.Load(memoryStream, out IImageFormat format);
        return image.CloneAs<TPixel>();
        //if (image is Image<Rgba32> png)
        //{
        //    png.ProcessPixelRows(accessor =>
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            var span = accessor.GetRowSpan(j);
        //            for (int i = 0; i < width; i++)
        //            {
        //                var t = span[i];
        //                colors[i, j] = new Color(t.R, t.G, t.B, t.A);
        //            }
        //        }
        //    });
        //}
        //else if (image is Image<Rgb24> jpg)
        //{
        //    jpg.ProcessPixelRows(accessor =>
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            var span = accessor.GetRowSpan(j);
        //            for (int i = 0; i < width; i++)
        //            {
        //                var t = span[i];
        //                colors[i, j] = new Color(t.R, t.G, t.B, 255);
        //            }
        //        }
        //    });
        //}
        //else
        //{
        //    Debug.Fail("剩下格式没写");
        //}

        //return colors;
    }
    /// <summary>
    /// RGB通道默认值0，Alpha默认值255，默认路径名为.bmp
    /// </summary>
    /// <param name="path">路径，可以不带后缀名</param>
    /// <param name="rect">读取范围，[)</param>
    /// <returns></returns>
    public static Color[,] Read(string path, Rectangle rect)
    {
        if (Path.GetExtension(path) == string.Empty)
        {
            path = Path.ChangeExtension(path, ".png");
        }
        using var memoryStream = new MemoryStream(ModContent.GetFileBytes(path));
        using var image = Image.Load(memoryStream);
        int maxX = rect.X + rect.Width, maxY = rect.Y + rect.Height;
        Color[,] colors = new Color[rect.Width, rect.Height];
        Debug.Assert(maxX <= image.Width && maxY <= image.Height);
        if (image is Image<Rgba32> png)
        {
            png.ProcessPixelRows(accessor =>
            {
                for (int j = rect.Y; j < maxY; j++)
                {
                    var span = accessor.GetRowSpan(j);
                    for (int i = rect.X; i < maxX; i++)
                    {
                        var t = span[i];
                        colors[i - rect.X, j - rect.Y] = new Color(t.R, t.G, t.B, t.A);
                    }
                }
            });
        }
        else if (image is Image<Rgb24> jpg)
        {
            jpg.ProcessPixelRows(accessor =>
            {
                for (int j = rect.Y; j < maxY; j++)
                {
                    var span = accessor.GetRowSpan(j);
                    for (int i = rect.X; i < maxX; i++)
                    {
                        var t = span[i];
                        colors[i, j] = new Color(t.R, t.G, t.B, 255);
                    }
                }
            });
        }
        else
        {
            Debug.Fail("剩下格式没写");
        }
        return colors;
    }
    /// <summary>
    /// RGB通道默认值0，Alpha默认值255，默认路径名为.bmp
    /// </summary>
    /// <param name="path">路径，可以不带后缀名</param>
    /// <param name="point">读取位置</param>
    /// <returns></returns>
    public static Color Read(string path, Point point)
    {
        if (Path.GetExtension(path) == string.Empty)
        {
            path = Path.ChangeExtension(path, ".png");
        }
        using var memoryStream = new MemoryStream(ModContent.GetFileBytes(path));
        using var image = Image.Load(memoryStream);
        Debug.Assert(point.X <= image.Width && point.Y <= image.Height);
        if (image is Image<Rgba32> png)
        {
            var t = png[point.X, point.Y];
            return new Color(t.R, t.G, t.B, t.A);
        }
        else if (image is Image<Rgb24> jpg)
        {
            var t = jpg[point.X, point.Y];
            return new Color(t.R, t.G, t.B, 255);
        }
        else
        {
            Debug.Fail("剩下格式没写");
        }
        return Color.Transparent;
    }
}
