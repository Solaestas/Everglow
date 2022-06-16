namespace Everglow.Sources.Commons.Function.ImageReader
{
    [Flags]
    public enum ColorPass
    {
        None,
        Red,
        Green,
        Blue,
        Alpha
    }
    internal static class ImageReader
    {
        /// <summary>
        /// RGB通道默认值0，Alpha默认值255
        /// </summary>
        /// <param name="path">路径，不带后缀名</param>
        /// <returns></returns>
        public static Color[,] Read(string path)
        {
            using var memoryStream = new MemoryStream(ModContent.GetFileBytes(Path.ChangeExtension(path, ".bin")));
            using var reader = new BinaryReader(memoryStream);
            Header header = new Header(reader);
            Color[,] colors = new Color[header.width, header.height];
            for (int i = 0; i < header.width; i++)
            {
                for (int j = 0; j < header.height; j++)
                {
                    colors[i, j] = new Color(header.red ? reader.ReadByte() : 0,
                        header.green ? reader.ReadByte() : 0,
                        header.blue ? reader.ReadByte() : 0,
                        header.alpha ? reader.ReadByte() : 255);
                }
            }
            Debug.Assert(memoryStream.Length == memoryStream.Position);
            return colors;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">路径，不带后缀名</param>
        /// <param name="rect">读取范围，[)</param>
        /// <returns></returns>
        public static Color[,] Read(string path, Rectangle rect)
        {
            using var memoryStream = new MemoryStream(ModContent.GetFileBytes(Path.ChangeExtension(path, ".bin")));
            using var reader = new BinaryReader(memoryStream);
            Header header = new Header(reader);
            Debug.Assert(rect.X >= 0 && rect.Y >= 0 && rect.X + rect.Width <= header.width && rect.Y + rect.Height <= header.height);
            Color[,] colors = new Color[rect.Width, rect.Height];
            int width = rect.Width + rect.X;
            int height = rect.Height + rect.Y;
            int count = (header.red ? 1 : 0) + (header.green ? 1 : 0) + (header.blue ? 1 : 0) + (header.alpha ? 1 : 0);

            memoryStream.Seek(count * rect.X * header.height, SeekOrigin.Current);
            for (int i = rect.X; i < width; i++)
            {
                memoryStream.Seek(count * rect.Y, SeekOrigin.Current);
                for (int j = rect.Y; j < height; j++)
                {
                    colors[i, j] = new Color(header.red ? reader.ReadByte() : 0,
                        header.green ? reader.ReadByte() : 0,
                        header.blue ? reader.ReadByte() : 0,
                        header.alpha ? reader.ReadByte() : 255);
                }
                memoryStream.Seek(count * (header.height - rect.Y - rect.Height), SeekOrigin.Current);
            }
            return colors;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">路径，不带后缀名</param>
        /// <param name="point">读取位置</param>
        /// <returns></returns>
        public static Color Read(string path, Point point)
        {
            using var memoryStream = new MemoryStream(ModContent.GetFileBytes(Path.ChangeExtension(path, ".bin")));
            using var reader = new BinaryReader(memoryStream);
            Header header = new Header(reader);
            Debug.Assert(point.X >= 0 && point.Y >= 0 && point.X < header.width && point.Y < header.height);
            int count = (header.red ? 1 : 0) + (header.green ? 1 : 0) + (header.blue ? 1 : 0) + (header.alpha ? 1 : 0);
            memoryStream.Seek(count * (point.X * header.height + point.Y), SeekOrigin.Current);
            return new Color(header.red ? reader.ReadByte() : 0,
                header.green ? reader.ReadByte() : 0,
                header.blue ? reader.ReadByte() : 0,
                header.alpha ? reader.ReadByte() : 255);
        }
        /// <summary>
        /// 读取指定通道的颜色，多通道按照RGBA的顺序排列
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[,,] Read(string path, ColorPass pass)
        {
            Debug.Assert(pass != ColorPass.None);
            using var memoryStream = new MemoryStream(ModContent.GetFileBytes(Path.ChangeExtension(path, ".bin")));
            using var reader = new BinaryReader(memoryStream);
            Header header = new Header(reader);
            bool getRed = pass.HasFlag(ColorPass.Red), getGreen = pass.HasFlag(ColorPass.Green),
                getBlue = pass.HasFlag(ColorPass.Blue), getAlpha = pass.HasFlag(ColorPass.Alpha);
            int numPasses = (getRed ? 1 : 0) + (getGreen ? 1 : 0) + (getBlue ? 1 : 0) + (getAlpha ? 1 : 0);
            byte[,,] colors = new byte[numPasses, header.width, header.height];
            Debug.Assert(!getRed || header.red);
            Debug.Assert(!getGreen || header.green);
            Debug.Assert(!getBlue || header.blue);
            Debug.Assert(!getAlpha || header.alpha);
            int redIndex = 0;
            int greenIndex = redIndex + (getRed ? 1 : 0);
            int blueIndex = greenIndex + (getGreen ? 1 : 0);
            int alphaIndex = blueIndex + (getBlue ? 1 : 0);
            for (int i = 0; i < header.width; i++)
            {
                for (int j = 0; j < header.height; j++)
                {
                    if (getRed)
                    {
                        colors[redIndex, i, j] = reader.ReadByte();
                    }
                    else if (header.red)
                    {
                        memoryStream.Seek(1, SeekOrigin.Current);
                    }

                    if (getGreen)
                    {
                        colors[greenIndex, i, j] = reader.ReadByte();
                    }
                    else if (header.green)
                    {
                        memoryStream.Seek(1, SeekOrigin.Current);
                    }

                    if (getBlue)
                    {
                        colors[blueIndex, i, j] = reader.ReadByte();
                    }
                    else if (header.blue)
                    {
                        memoryStream.Seek(1, SeekOrigin.Current);
                    }

                    if (getAlpha)
                    {
                        colors[alphaIndex, i, j] = reader.ReadByte();
                    }
                    else if (header.alpha)
                    {
                        memoryStream.Seek(1, SeekOrigin.Current);
                    }
                }
            }
            return colors;
        }
    }
}
