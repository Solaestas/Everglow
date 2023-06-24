using Terraria.IO;
using Terraria.WorldBuilding;
using Everglow.Yggdrasil.KelpCurtain.Tiles;
using Everglow.Yggdrasil.KelpCurtain.Walls;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Everglow.Yggdrasil.HurricaneMaze.Tiles;
using Everglow.Yggdrasil.CorruptWormHive.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Walls;
using Everglow.Commons.TileHelper;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilTownGeneration;

namespace Everglow.Yggdrasil.WorldGeneration;

public class YggdrasilWorldGeneration : ModSystem
{
	internal class YggdrasilWorldGenPass : GenPass
	{
		public YggdrasilWorldGenPass() : base("Yggdrasil, the Tree World", 500)
		{
		}

		public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildtheTreeWorld");
			BuildtheTreeWorld();
			Main.spawnTileX = 600;
			Main.spawnTileY = 11630;
			BuildYggdrasilTown();
			EndGenPass();
			Main.statusText = "";
		}
	}
	public static void EndGenPass()
	{
		Main.statusText = "Finished";
	}
    public static void PlaceFrameImportantTiles(int x, int y, int width, int height, int type, int startX = 0, int startY = 0)
	{
		if (x > Main.maxTilesX - width || x < 0 || y > Main.maxTilesY - height || y < 0)
			return;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Tile tile = Main.tile[x + i, y + j];
				tile.TileType = (ushort)type;
				tile.TileFrameX = (short)(i * 18 + startX);
				tile.TileFrameY = (short)(j * 18 + startY);
				tile.HasTile = true;
			}
		}
	}
	public static Tile SafeGetTile(int i, int j)
	{
		return Main.tile[Math.Clamp(i, 20, Main.maxTilesX - 20), Math.Clamp(j, 20, Main.maxTilesY - 20)];
	}
	/// <summary>
	/// 平坦化,x0左y0上x1右y1下
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	public static void SmoothTile(int x0, int y0, int x1, int y1)
	{
		x0 = Math.Clamp(x0, 20, Main.maxTilesX - 20);
		x1 = Math.Clamp(x1, 20, Main.maxTilesX - 20);
		y0 = Math.Clamp(y0, 20, Main.maxTilesY - 20);
		y1 = Math.Clamp(y1, 20, Main.maxTilesY - 20);
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile.SmoothSlope(x, y, false);
				WorldGen.TileFrame(x, y, true, false);
				WorldGen.SquareWallFrame(x, y, true);
			}
		}
	}
	/// <summary>
	/// 放置一个矩形区域的物块,x0左y0上x1右y1下
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="type"></param>
	public static void PlaceRectangleAreaOfBlock(int x0, int y0, int x1, int y1, int type, bool smooth = true)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = SafeGetTile(x, y);
				tile.TileType = (ushort)type;
				tile.HasTile = true;
			}
		}
		if(smooth)
		{
			SmoothTile(x0, y0, x1, y1);
		}
	}
	/// <summary>
	/// 放置一个矩形区域的墙壁,x0左y0上x1右y1下
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="type"></param>
	public static void PlaceRectangleAreaOfWall(int x0, int y0, int x1, int y1, int type)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = SafeGetTile(x, y);
				tile.wall = (ushort)type;
			}
		}
		SmoothTile(x0, y0, x1, y1);
	}
	/// <summary>
	/// 清除给定区域的一切,x0左y0上x1右y1下
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="type"></param>
	public static void ClearRectangleArea(int x0, int y0, int x1, int y1)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = SafeGetTile(x, y);
				tile.ClearEverything();
			}
		}
		SmoothTile(x0, y0, x1, y1);
	}
	/// <summary>
	/// 清除给定区域的物块,x0左y0上x1右y1下
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="type"></param>
	public static void KillRectangleAreaOfTile(int x0, int y0, int x1, int y1)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = SafeGetTile(x, y);
				tile.HasTile = false;
			}
		}
		SmoothTile(x0, y0, x1, y1);
	}
	/// <summary>
	/// 清除给定区域的墙壁,x0左y0上x1右y1下
	/// </summary>
	/// <param name="x0"></param>
	/// <param name="y0"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="type"></param>
	public static void KillRectangleAreaOfWall(int x0, int y0, int x1, int y1)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = SafeGetTile(x, y);
				tile.wall = 0;
			}
		}
		SmoothTile(x0, y0, x1, y1);
	}
	public static void QuickBuild(int x, int y, string Path)
	{
		var mapIO = new MapIO(x, y);

		mapIO.Read(ModIns.Mod.GetFileStream("Yggdrasil/" + Path));

		var it = mapIO.GetEnumerator();
		while (it.MoveNext())
		{
			WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
			WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
		}
	}
	/// <summary>
	/// type = 0:Kill,type = 1:place Tiles,type = 2:place Walls
	/// </summary>
	/// <param name="Shapepath"></param>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="type"></param>
	public static void ShapeTile(string Shapepath, int a, int b, int type)
	{
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Yggdrasil/WorldGeneration/" + Shapepath);
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				var pixelRow = accessor.GetRowSpan(y);
				for (int x = 0; x < pixelRow.Length; x++)
				{
					ref var pixel = ref pixelRow[x];
					Tile tile = SafeGetTile(x + a, y + b);
					switch (type)//21是箱子
					{
						case 0:
							if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
							{
								if (tile.TileType != 21 && SafeGetTile(x + a, y + b - 1).TileType != 21)
									tile.ClearEverything();
							}
							break;
						case 1:
							//天穹古道
							if (pixel.R == 44 && pixel.G == 40 && pixel.B == 37)//石化龙鳞木
							{
								tile.TileType = (ushort)ModContent.TileType<StoneScaleWood>();
								tile.HasTile = true;
							}
							if (pixel.R == 155 && pixel.G == 173 && pixel.B == 183)//青缎矿
							{
								tile.TileType = (ushort)ModContent.TileType<CyanVineStone>();
								tile.HasTile = true;
							}

							if (pixel.R == 31 && pixel.G == 26 && pixel.B == 45)//黑淤泥
							{
								tile.TileType = (ushort)ModContent.TileType<DarkMud>();
								tile.HasTile = true;
							}

							//苍苔蔓帘
							if (pixel.R == 82 && pixel.G == 62 && pixel.B == 44)//龙鳞木
							{
								tile.TileType = (ushort)ModContent.TileType<DragonScaleWood>();
								tile.HasTile = true;
							}
							if (pixel.R == 81 && pixel.G == 107 && pixel.B == 18)//古苔藓
							{
								tile.TileType = (ushort)ModContent.TileType<OldMoss>();
								tile.HasTile = true;
							}
							if (pixel.R == 53 && pixel.G == 29 && pixel.B == 26)//天穹泥
							{
								tile.TileType = (ushort)ModContent.TileType<YggdrasilDirt>();
								tile.HasTile = true;
							}



							//飓风迷宫
							if (pixel.R == 65 && pixel.G == 84 && pixel.B == 63)//青岗岩
							{
								tile.TileType = (ushort)ModContent.TileType<CyanWindGranite>();
								tile.HasTile = true;
							}


							//蛆败之穴
							if (pixel.R == 107 && pixel.G == 34 && pixel.B == 21)//血解光石
							{
								tile.TileType = (ushort)ModContent.TileType<BloodLightCrystal>();
								tile.HasTile = true;
								ModContent.GetInstance<BloodLightCrystalEntity>().Place(x + a, y + b);
							}



							//常规
							if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255)//水
							{
								tile.LiquidType = LiquidID.Water;
								tile.LiquidAmount = 200;
								tile.HasTile = false;
							}
							if (pixel.R == 128 && pixel.G == 128 && pixel.B == 128)//岩石
							{
								tile.TileType = TileID.Stone;
								tile.HasTile = true;
							}
							if (pixel.R == 186 && pixel.G == 168 && pixel.B == 84)//沙
							{
								tile.TileType = TileID.Sand;
								tile.HasTile = true;
							}
							break;
						case 2:
							if (pixel.R == 24 && pixel.G == 0 && pixel.B == 0)//石化龙鳞木
							{
								if (tile.TileType != 21 && SafeGetTile(x + a, y + b - 1).TileType != 21)
									tile.WallType = (ushort)ModContent.WallType<StoneDragonScaleWoodWall>();
							}
							if (pixel.R == 40 && pixel.G == 32 && pixel.B == 31)//龙鳞木
							{
								if (tile.TileType != 21 && SafeGetTile(x + a, y + b - 1).TileType != 21)
									tile.WallType = (ushort)ModContent.WallType<DragonScaleWoodWall>();
							}
							if (pixel.R == 56 && pixel.G == 56 && pixel.B == 56)//石墙
							{
								if (tile.TileType != 21 && SafeGetTile(x + a, y + b - 1).TileType != 21)
									tile.WallType = WallID.Stone;
							}
							if (pixel.R == 25 && pixel.G == 14 && pixel.B == 12)//天穹土墙
							{
								if (tile.TileType != 21 && SafeGetTile(x + a, y + b - 1).TileType != 21)
									tile.WallType = (ushort)ModContent.WallType<YggdrasilDirtWall>();
							}
							break;
						case 3://天穹古道建筑
							if (pixel.R == 121 && pixel.G == 5 && pixel.B == 255)//FolkHouseofChineseStyle TypeA  28x11
								QuickBuild(x, y, "YggdrasilTown/MapIOs/1FolkHouseofChineseStyleTypeA28x11.mapio");
							if (pixel.R == 120 && pixel.G == 5 && pixel.B == 255)//FolkHouseofChineseStyle TypeB  28x11
								QuickBuild(x, y, "YggdrasilTown/MapIOs/1FolkHouseofChineseStyleTypeB28x11.mapio");

							if (pixel.R == 122 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWood＆StoneStruture TypeA  28x11
								QuickBuild(x, y, "YggdrasilTown/MapIOs/2FolkHouseofWoodStoneStrutureTypeA28x11.mapio");
							if (pixel.R == 123 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWood＆StoneStruture TypeB  28x11
								QuickBuild(x, y, "YggdrasilTown/MapIOs/2FolkHouseofWoodStoneStrutureTypeB28x11.mapio");

							if (pixel.R == 124 && pixel.G == 5 && pixel.B == 255)//Smithy TypeA  22x8
								QuickBuild(x, y, "YggdrasilTown/MapIOs/3SmithyTypeA22x8.mapio");
							if (pixel.R == 125 && pixel.G == 5 && pixel.B == 255)//Smithy TypeB  22x8
								QuickBuild(x, y, "YggdrasilTown/MapIOs/3SmithyTypeB22x8.mapio");

							if (pixel.R == 126 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWoodStruture TypeA  22x10
								QuickBuild(x, y, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeA22x10.mapio");
							if (pixel.R == 127 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWoodStruture TypeB  22x10
								QuickBuild(x, y, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeB22x10.mapio");
							if (pixel.R == 128 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWoodStruture TypeC  22x10
								QuickBuild(x, y, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeC22x10.mapio");
							if (pixel.R == 129 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWoodStruture TypeD  22x10
								QuickBuild(x, y, "YggdrasilTown/MapIOs/4FolkHouseofWoodStrutureTypeD22x10.mapio");

							if (pixel.R == 130 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWoodStruture TypeA  23x13
								QuickBuild(x, y, "YggdrasilTown/MapIOs/5TwoStoriedFolkHouseTypeA23x13.mapio");
							if (pixel.R == 131 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWoodStruture TypeB  23x13
								QuickBuild(x, y, "YggdrasilTown/MapIOs/5TwoStoriedFolkHouseTypeB23x13.mapio");
							if (pixel.R == 132 && pixel.G == 5 && pixel.B == 255)//FolkHouseofWoodStruture TypeC  23x13
								QuickBuild(x, y, "YggdrasilTown/MapIOs/5TwoStoriedFolkHouseTypeC23x13.mapio");

							if (pixel.R == 133 && pixel.G == 5 && pixel.B == 255)//Church 80x51
								QuickBuild(x, y, "YggdrasilTown/MapIOs/Church80x51.mapio");
							break;
						case 4:
							if (pixel.R == 195 && pixel.G == 217 && pixel.B == 229)//大青缎矿
								PlaceLargeCyanVineOre(x, y);
							if (pixel.R == 195 && pixel.G == 217 && pixel.B == 230)//中青缎矿
								PlaceMiddleCyanVineOre(x, y);
							if (pixel.R == 195 && pixel.G == 217 && pixel.B == 231)//小青缎矿
								PlaceSmallCyanVineOre(x, y);
							if (pixel.R == 195 && pixel.G == 217 && pixel.B == 232)//倒挂小青缎矿
								PlaceSmallUpCyanVineOre(x, y);
							if (pixel.R == 195 && pixel.G == 217 && pixel.B == 233)//倒挂大青缎矿
								PlaceLargeUpCyanVineOre(x, y);
							break;
					}
				}
			}
		});
	}
	/// <summary>
	/// 建造天穹树
	/// </summary>
	public static void BuildtheTreeWorld()
	{
		//Main.statusText = "YggdrasilStart";
		//ShapeTile("Tree.bmp", 0, 0, 1);
		//Main.statusText = "YggdrasilWall";
		//ShapeTile("TreeWall.bmp", 0, 0, 2);
		//SmoothTile();

		//Main.statusText = "YggdrasilTown";
		//ShapeTile("Tree.bmp", 0, 0, 3);
		//Main.statusText = "YggdrasilOre";
		//ShapeTile("Tree.bmp", 0, 0, 4);
	}
}

