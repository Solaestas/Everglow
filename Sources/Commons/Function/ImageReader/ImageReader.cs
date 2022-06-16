namespace Everglow.Sources.Commons.Function.ImageReader
{
    internal static class ImageReader
    {
        /// <summary>
        /// 
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
                        header.alpha ? reader.ReadByte() : 0);
                }
            }
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
            int count = 0;
            if (header.red)
            {
                count++;
            }

            if (header.green)
            {
                count++;
            }

            if (header.blue)
            {
                count++;
            }

            if (header.alpha)
            {
                count++;
            }

            memoryStream.Seek(count * rect.X * header.height, SeekOrigin.Current);
            for (int i = rect.X; i < width; i++)
            {
                memoryStream.Seek(count * rect.Y, SeekOrigin.Current);
                for (int j = rect.Y; j < height; j++)
                {
                    colors[i, j] = new Color(header.red ? reader.ReadByte() : 0,
                        header.green ? reader.ReadByte() : 0,
                        header.blue ? reader.ReadByte() : 0,
                        header.alpha ? reader.ReadByte() : 0);
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
            int count = 0;
            if (header.red)
            {
                count++;
            }

            if (header.green)
            {
                count++;
            }

            if (header.blue)
            {
                count++;
            }

            if (header.alpha)
            {
                count++;
            }

            memoryStream.Seek(count * (point.X * header.height + point.Y), SeekOrigin.Current);
            return new Color(header.red ? reader.ReadByte() : 0,
                header.green ? reader.ReadByte() : 0,
                header.blue ? reader.ReadByte() : 0,
                header.alpha ? reader.ReadByte() : 0);
        }
    }
}
