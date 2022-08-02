using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO
{
    internal class ModEntry
    {
        public Dictionary<string, ushort> entries = new Dictionary<string, ushort>();
        public Dictionary<ushort, ushort> typeMaping = new Dictionary<ushort, ushort>();
        public int GetOrAddEntry(string fullName)
        {
            if (entries.ContainsKey(fullName))
            {
                return entries[fullName];
            }
            entries.Add(fullName, (ushort)entries.Count);
            return entries.Count - 1;
        }
        public void Write(BinaryWriter writer)
        {
            writer.Write(entries.Count);
            foreach (var (name, type) in entries)
            {
                writer.Write(name);
                writer.Write(type);
            }
        }
        public void Read(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var name = reader.ReadString();
                var type = reader.ReadUInt16();
                entries.Add(name, type);
                //typeMaping.Add(type, ModContent.Find<Modwal>(name).Type);
                if(ModContent.TryFind<ModWall>(name, out var wall))
                {
                    typeMaping.Add(type, wall.Type);
                }else if(ModContent.TryFind<ModTile>(name, out var tile))
                {
                    typeMaping.Add(type, tile.Type);
                }else
                {
                    Debug.Fail("Fail to find a modblock");
                }
            }
        }
    }
    internal class MapIO
    {
        public static int AirTileType => ModContent.TileType<AirTile>();
        public static int AirWallType => ModContent.WallType<AirWall>();
        public int x;
        public int y;
        public int width;
        public int height;
        public Rectangle Rectangle => new Rectangle(x, y, width, height);
        public MapIO(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public MapIO(Rectangle rect)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }
        public MapIO(Point a, Point b)
        {
            Point min = new Point(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
            Point max = new Point(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
            x = min.X;
            y = min.Y;
            width = max.X - min.X;
            height = max.Y - min.Y;
        }
        public MapIO(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Write(string path)
        {
            using var stream = File.Create(path);
            Write(stream);
        }
        public void Write(Stream stream)
        {
            var entry = new ModEntry();

            using MemoryStream memoryStream = new MemoryStream(500000);
            using var writer = new BinaryWriter(memoryStream);
            var accessor = new TileAccessor(x, y, x + width, y + height);
            writer.Write(width);
            writer.Write(height);
            WriteTile(writer, accessor, entry);
            WriteChest(writer, Rectangle);
            WriteSign(writer, Rectangle);

            using MemoryStream targetStream = new MemoryStream(500000);
            using var writerEntry = new BinaryWriter(targetStream);
            entry.Write(writerEntry);
            memoryStream.WriteTo(targetStream);
            using var zip = new GZipStream(stream, CompressionLevel.Optimal);
            targetStream.WriteTo(zip);

        }
        public void Read(string path)
        {
            using var stream = File.OpenRead(path);
            Read(stream);
        }
        public int ReadWidth(string path)
        {
            using var stream = File.OpenRead(path);
            var entry = new ModEntry();
            using var zip = new GZipStream(stream, CompressionMode.Decompress);
            using var reader = new BinaryReader(zip);
            entry.Read(reader);
            width = reader.ReadInt32();
            height = reader.ReadInt32(); ;
            return width;
        }
        public int ReadHeight(string path)
        {
            using var stream = File.OpenRead(path);
            var entry = new ModEntry();
            using var zip = new GZipStream(stream, CompressionMode.Decompress);
            using var reader = new BinaryReader(zip);
            entry.Read(reader);
            width = reader.ReadInt32();
            height = reader.ReadInt32();
            return height;
        }
        public void Read(Stream stream)
        {
            var entry = new ModEntry();
            using var zip = new GZipStream(stream, CompressionMode.Decompress);
            using var reader = new BinaryReader(zip);
            entry.Read(reader);
            width = reader.ReadInt32();
            height = reader.ReadInt32();
            ReadTile(reader, new TileAccessor(x, y, x + width, y + height), entry);
            ReadChest(reader, new Point(x, y));
            ReadSign(reader, new Point(x, y));
        }
        /// <summary>
        /// 写入<paramref name="accessor"/>访问到的所有Tile，并且采取相邻相同Tile统一保存
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="accessor"></param>
        /// <param name="entry"></param>
        public static void WriteTile(BinaryWriter writer, ITileAccessor accessor, ModEntry entry)
        {
            if (!accessor.MoveNext())
            {
                return;
            }

            int airBlockType = AirTileType;
            do
            {
                BitsByte[] heads = new BitsByte[3];
                var tile = accessor.Current;
                var memoryStream = new MemoryStream(16);
                var writer_local = new BinaryWriter(memoryStream);
                //0：是否有方块，1：是否为Mod方块
                if (!tile.HasTile)
                {
                    heads[0][0] = false;
                    heads[0][1] = false;
                }
                else
                {
                    if (tile.TileType == airBlockType)
                    {
                        heads[0][0] = false;
                        heads[0][1] = true;
                    }
                    else if (tile.TileType > TileID.Count)
                    {
                        heads[0][0] = true;
                        heads[0][1] = true;
                        //ModTile写入FullName对应的Index
                        int index = entry.GetOrAddEntry(ModContent.GetModTile(tile.TileType).FullName);
                        if (index > byte.MaxValue)
                        {
                            heads[0][2] = true;
                            writer_local.Write(index);
                        }
                        else
                        {
                            heads[0][2] = false;
                            writer_local.Write((byte)index);
                        }
                    }
                    else
                    {
                        heads[0][0] = true;
                        heads[0][1] = false;
                        //原版Tile写入TileType
                        if (tile.TileType > byte.MaxValue)
                        {
                            heads[0][2] = true;
                            writer_local.Write(tile.TileType);
                        }
                        else
                        {
                            heads[0][2] = false;
                            writer_local.Write((byte)tile.TileType);
                        }
                    }

                    if (Main.tileFrameImportant[tile.TileType])
                    {
                        //可能不需要的针对旧版的代码
                        ////原版代码---------
                        //short frameX = tile.TileFrameX;
                        //bool mannequin = tile.TileType == 128 || tile.TileType == 269;
                        //if (mannequin)
                        //{
                        //    int slot = tile.TileFrameX / 100;
                        //    int position = tile.TileFrameX / 18;
                        //    var hasModArmor = position switch
                        //    {
                        //        0 => (slot >= 277),
                        //        1 => (slot >= 246),
                        //        2 => (slot >= 234),
                        //        _ => false,
                        //    };
                        //    if (hasModArmor)
                        //    {
                        //        frameX %= 100;
                        //    }
                        //}
                        ////--------------
                        if (tile.TileFrameX > byte.MaxValue)
                        {
                            heads[0][3] = true;
                            writer_local.Write(tile.TileFrameX);
                        }
                        else
                        {
                            heads[0][3] = false;
                            writer_local.Write((byte)tile.TileFrameX);
                        }
                        if (tile.TileFrameY > byte.MaxValue)
                        {
                            heads[0][4] = true;
                            writer_local.Write(tile.TileFrameY);
                        }
                        else
                        {
                            heads[0][4] = false;
                            writer_local.Write((byte)tile.TileFrameY);
                        }
                    }

                    int style = (int)tile.BlockType;
                    if ((style & 1) == 1)
                    {
                        heads[1][0] = true;
                    }
                    if ((style & 2) == 2)
                    {
                        heads[1][1] = true;
                    }
                    if ((style & 4) == 4)
                    {
                        heads[1][2] = true;
                    }
                }

                if (tile.WallType == 0)
                {
                    //无墙壁
                    heads[1][3] = false;
                    heads[1][4] = false;
                }
                else if (tile.WallType == AirWallType)
                {
                    heads[1][3] = false;
                    heads[1][4] = true;
                }
                else
                {
                    //有墙壁
                    heads[1][3] = true;
                    int wallType;
                    if (tile.WallType > WallID.Count)
                    {
                        //Mod墙壁
                        heads[1][4] = true;
                        wallType = entry.GetOrAddEntry(ModContent.GetModWall(tile.WallType).FullName);
                    }
                    else
                    {
                        heads[1][4] = false;
                        wallType = tile.WallType;
                    }

                    if (wallType > byte.MaxValue)
                    {
                        heads[1][5] = true;
                        writer_local.Write(wallType);
                    }
                    else
                    {
                        heads[1][5] = false;
                        writer_local.Write((byte)wallType);
                    }
                    //真的有墙壁要存Frame吗？
                    if (tile.WallFrameX != 0 || tile.WallFrameY != 0)
                    {
                        heads[0][5] = true;//剩下一个5位没用
                        writer_local.Write((byte)((tile.WallFrameX / 36 << 4) | (tile.WallFrameY / 36)));
                    }
                }

                LiquidData liquidData = tile.Get<LiquidData>();
                if (liquidData.Amount > 0)
                {
                    heads[1][6] = true;
                    writer_local.Write(liquidData.Amount);
                    writer_local.Write((byte)((byte)liquidData.LiquidType | (liquidData.SkipLiquid ? 0b0100_0000 : 0) | (liquidData.CheckingLiquid ? 0b1000_0000 : 0)));
                }

                heads[2][0] = tile.RedWire;
                heads[2][1] = tile.BlueWire;
                heads[2][2] = tile.GreenWire;
                heads[2][3] = tile.YellowWire;
                heads[2][4] = tile.HasActuator;
                heads[2][5] = tile.IsActuated;
                if (tile.TileColor != 0)
                {
                    heads[2][6] = true;
                    writer_local.Write(tile.TileColor);
                }
                if (tile.WallColor != 0)
                {
                    heads[2][7] = true;
                    writer_local.Write(tile.WallColor);
                }

                if (TileID.Sets.AllowsSaveCompressionBatching[tile.TileType])
                {
                    ushort count = SameCount(tile, accessor);
                    if (count > 0)
                    {
                        //存在重复物块，则将6位设为true
                        heads[0][6] = true;
                        writer_local.Write(count);
                    }
                }
                else
                {
                    accessor.MoveNext();
                }

                if (heads[2] != 0)
                {
                    heads[1][7] = true;
                }
                if (heads[1] != 0)
                {
                    heads[0][7] = true;
                }

                writer.Write(heads[0]);
                if (heads[0][7])
                {
                    writer.Write(heads[1]);
                }

                if (heads[1][7])
                {
                    writer.Write(heads[2]);
                }

                writer.Write(memoryStream.ToArray());

            } while (accessor.Good);

        }
        public static void ReadTile(BinaryReader reader, ITileAccessor accessor, ModEntry entry)
        {
            while (accessor.MoveNext())
            {
                var tile = accessor.Current;
                BitsByte[] heads = new BitsByte[3];
                heads[0] = reader.ReadByte();
                if (heads[0][7])
                {
                    heads[1] = reader.ReadByte();
                }
                if (heads[1][7])
                {
                    heads[2] = reader.ReadByte();
                }
                if (!heads[0][0])
                {
                    if (!heads[0][1])
                    {
                        //空气
                        tile.HasTile = false;
                    }
                }
                else
                {
                    tile.HasTile = true;
                    if (heads[0][1])
                    {
                        //Mod物块
                        tile.TileType = entry.typeMaping[heads[0][2] ? reader.ReadUInt16() : reader.ReadByte()];
                    }
                    else
                    {
                        //原版物块
                        tile.TileType = heads[0][2] ? reader.ReadUInt16() : reader.ReadByte();
                    }

                    if (Main.tileFrameImportant[tile.TileType])
                    {
                        tile.TileFrameX = heads[0][3] ? reader.ReadInt16() : reader.ReadByte();
                        tile.TileFrameY = heads[0][4] ? reader.ReadInt16() : reader.ReadByte();
                    }
                    //tml,你tm什么时候修
                    //tile.BlockType = (BlockType)((heads[1][0] ? 1 : 0) | (heads[1][1] ? 2 : 0) | (heads[1][2] ? 4 : 0));
                    var blockType = (BlockType)((heads[1][0] ? 1 : 0) | (heads[1][1] ? 2 : 0) | (heads[1][2] ? 4 : 0));
                    tile.IsHalfBlock = blockType == BlockType.HalfBlock;
                    tile.Slope = blockType > BlockType.HalfBlock ? (SlopeType)(blockType - 1) : SlopeType.Solid;
                }

                if (!heads[1][3])
                {
                    if (!heads[1][4])
                    {
                        tile.WallType = 0;
                    }
                }
                else
                {
                    if (heads[1][4])
                    {
                        //Mod墙壁
                        tile.WallType = entry.typeMaping[heads[1][5] ? reader.ReadUInt16() : reader.ReadByte()];
                    }
                    else
                    {
                        //原版墙壁
                        tile.WallType = heads[1][5] ? reader.ReadUInt16() : reader.ReadByte();
                    }

                    if (heads[0][5])
                    {
                        byte wallFrame = reader.ReadByte();
                        tile.WallFrameX = (wallFrame >> 4) * 36;
                        tile.WallFrameY = (wallFrame & 0b0000_1111) * 36;
                    }
                }

                if (heads[1][6])
                {
                    tile.LiquidAmount = reader.ReadByte();
                    byte data = reader.ReadByte();
                    tile.LiquidType = data & 0b0011_1111;
                    tile.SkipLiquid = (data & 0b0100_0000) != 0;
                    tile.CheckingLiquid = (data & 0b1000_0000) != 0;
                }
                else
                { 
                    tile.Clear(TileDataType.Liquid);
                }

                tile.RedWire = heads[2][0];
                tile.BlueWire = heads[2][1];
                tile.GreenWire = heads[2][2];
                tile.YellowWire = heads[2][3];
                tile.HasActuator = heads[2][4];
                tile.IsActuated = heads[2][5];

                if (heads[2][6])
                {
                    tile.TileColor = reader.ReadByte();
                }
                else
                {
                    tile.TileColor = 0;
                }

                if (heads[2][7])
                {
                    tile.WallColor = reader.ReadByte();
                }
                else
                {
                    tile.WallColor = 0;
                }

                if (heads[0][6])
                {
                    ushort count = reader.ReadUInt16();
                    for (int i = 0; i < count; i++)
                    {
                        accessor.MoveNext();
                        Debug.Assert(accessor.Good);
                        accessor.Current.CopyFrom(tile);
                    }
                }
            }
        }
        public static void WriteChest(BinaryWriter writer, Rectangle range)
        {
            var it = Main.chest.Where(c => c != null && range.Contains(new Point(c.x, c.y)));
            writer.Write(it.Count());
            foreach (var c in it)
            {
                writer.Write(c.x - range.X);
                writer.Write(c.y - range.Y);
                writer.Write(c.name);
                writer.Write(c.frame);
                foreach (var item in c.item)
                {
                    TagIO.Write(ItemIO.Save(item), writer);
                }
            }
        }
        public static void ReadChest(BinaryReader reader, Point startPoint)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int index = Array.IndexOf(Main.chest, null);
                Main.chest[index] = new Chest()
                {
                    x = reader.ReadInt32() + startPoint.X,
                    y = reader.ReadInt32() + startPoint.Y,
                    name = reader.ReadString(),
                    frame = reader.ReadInt32()
                };
                for (int j = 0; j < Chest.maxItems; j++)
                {
                    Main.chest[index].item[j] = ItemIO.Load(TagIO.Read(reader));
                }
            }
        }
        public static void WriteSign(BinaryWriter writer, Rectangle range)
        {
            var it = Main.sign.Where(s => s != null && range.Contains(new Point(s.x, s.y)));
            writer.Write(it.Count());
            foreach (var s in it)
            {
                writer.Write(s.x - range.X);
                writer.Write(s.y - range.Y);
                writer.Write(s.text);
            }
        }
        public static void ReadSign(BinaryReader reader, Point startPoint)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int index = Array.IndexOf(Main.sign, null);
                Main.sign[index] = new Sign()
                {
                    x = reader.ReadInt32() + startPoint.X,
                    y = reader.ReadInt32() + startPoint.Y,
                    text = reader.ReadString()
                };
            }
        }
        private static ushort SameCount(in Tile tile, ITileAccessor accessor)
        {
            //从原版Copy过来判断Tile是否相同的方法
            static bool IsSameTile(Tile tile, Tile compTile)
            {
                if (tile.Get<TileWallWireStateData>().NonFrameBits != compTile.Get<TileWallWireStateData>().NonFrameBits)
                {
                    return false;
                }

                if (tile.WallType != compTile.WallType || tile.LiquidAmount != compTile.LiquidAmount)
                {
                    return false;
                }

                if (tile.LiquidAmount > 0 && tile.LiquidType != compTile.LiquidType)
                {
                    return false;
                }

                if (tile.HasTile)
                {
                    if (tile.TileType != compTile.TileType)
                    {
                        return false;
                    }

                    if (Main.tileFrameImportant[tile.TileType] && (tile.TileFrameX != compTile.TileFrameX || tile.TileFrameY != compTile.TileFrameY))
                    {
                        return false;
                    }
                }

                return true;
            }

            ushort count = 0;
            while (accessor.MoveNext())
            {
                var next = accessor.Current;
                if (IsSameTile(tile, next) && count <= short.MaxValue - 1)
                {
                    ++count;
                }
                else
                {
                    return count;
                }
            }
            return count;
        }
        public ITileAccessor GetEnumerator()
        {
            return new TileAccessor(x, y, x + width, y + height);
        }
    }
}
