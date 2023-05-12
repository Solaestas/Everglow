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
		}
	}
	public static Tile SafeGetTile(int i, int j)
	{
		return Main.tile[Math.Clamp(i, 1, Main.maxTilesX - 1), Math.Clamp(j, 1, Main.maxTilesY - 1)];
	}
	//public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new YggdrasilWorldGenPass());
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
	public static void PlaceLargeCyanVineOre(int i, int j)
	{
		switch (Main.rand.Next(2))
		{
			case 0:
				for (int x = 0; x < 5; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						var tile = SafeGetTile(i + x, j + y);
						if (x == 0 && y == 0)
							continue;
						if (x == 4 && y == 0)
							continue;
						if (x == 4 && y == 1)
							continue;
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.IsHalfBlock = true;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 4 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreLarge>();
							tile.TileFrameX = 36;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 1:
				for (int x = 1; x < 4; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = SafeGetTile(i + x, j + y);
						if (x == 4 && y == 1)
							continue;
						if (x == 2 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreLarge>();
							tile.TileFrameX = 144;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.IsHalfBlock = true;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
		}
	}
	public static void PlaceMiddleCyanVineOre(int i, int j)
	{
		switch (Main.rand.Next(4))
		{
			case 0:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 3; y++)
					{
						var tile = SafeGetTile(i + x, j + y);
						if (x == 0 && y == 0)
							continue;
						if (x == 1 && y == 0)
							continue;
						if (x == 0 && y == 1)
							continue;
						if (x == 2 && y == 0)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 18;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 1:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = SafeGetTile(i + x, j + y);

						if (x == 0 && y == 1)
							continue;
						if (x == 2 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 90;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 2:
				for (int x = 1; x < 4; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = SafeGetTile(i + x, j + y);

						if (x == 3 && y == 1)
							continue;
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 162;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 3:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 1; y < 3; y++)
					{
						var tile = SafeGetTile(i + x, j + y);

						if (x == 0 && y == 1)
							continue;
						if (x == 0 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 2 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.IsHalfBlock = true;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 2)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreMiddle>();
							tile.TileFrameX = 234;
							tile.TileFrameY = 54;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
		}
	}
	public static void PlaceSmallCyanVineOre(int i, int j)
	{
		switch (Main.rand.Next(3))
		{
			case 0:
				for (int x = 0; x < 2; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						var tile = SafeGetTile(i + x, j + y);
						if (x == 0 && y == 0)
							continue;
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmall>();
							tile.TileFrameX = 0;
							tile.TileFrameY = 36;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 1:
				for (int x = 0; x < 2; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						var tile = SafeGetTile(i + x, j + y);

						if (x == 0 && y == 0)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 1 && y == 0)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmall>();
							tile.TileFrameX = 54;
							tile.TileFrameY = 36;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
			case 2:
				for (int x = 0; x < 3; x++)
				{
					for (int y = 0; y < 2; y++)
					{
						var tile = SafeGetTile(i + x, j + y);

						if (x == 0 && y == 0)
							continue;
						if (x == 2 && y == 0)
							continue;
						if (x == 2 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
							tile.Slope = SlopeType.SlopeDownLeft;
							tile.TileFrameX = (short)(x * 18);
							tile.TileFrameY = (short)(y * 18);
							tile.HasTile = true;
							continue;
						}
						if (x == 0 && y == 1)
						{
							tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmall>();
							tile.Slope = SlopeType.SlopeDownRight;
							tile.TileFrameX = 108;
							tile.TileFrameY = 36;
							tile.HasTile = true;
							continue;
						}
						tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
						tile.TileFrameX = (short)(x * 18);
						tile.TileFrameY = (short)(y * 18);
						tile.HasTile = true;
					}
				}
				break;
		}
	}
	public static void PlaceSmallUpCyanVineOre(int i, int j)
	{
		switch (Main.rand.Next(4))
		{
			case 0:
				{
					var tile = SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 18;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.Slope = SlopeType.SlopeUpRight;
					tileII.HasTile = true;
				}

				break;
			case 1:
				{
					var tile = SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 72;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.HasTile = true;
				}
				break;
			case 2:
				{
					var tile = SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 126;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.HasTile = true;

					var tileIII = SafeGetTile(i, j);
					tileIII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIII.TileFrameX = 0;
					tileIII.TileFrameY = 0;
					tileIII.Slope = SlopeType.SlopeUpRight;
					tileIII.HasTile = true;

					var tileIV = SafeGetTile(i + 2, j);
					tileIV.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIV.TileFrameX = 36;
					tileIV.TileFrameY = 0;
					tileIV.Slope = SlopeType.SlopeUpLeft;
					tileIV.HasTile = true;
				}
				break;
			case 3:
				{
					var tile = SafeGetTile(i + 1, j);
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreSmallUp>();
					tile.TileFrameX = 180;
					tile.TileFrameY = 0;
					tile.HasTile = true;

					var tileII = SafeGetTile(i + 1, j + 1);
					tileII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileII.TileFrameX = 18;
					tileII.TileFrameY = 18;
					tileII.Slope = SlopeType.SlopeUpRight;
					tileII.HasTile = true;

					var tileIII = SafeGetTile(i, j);
					tileIII.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIII.TileFrameX = 0;
					tileIII.TileFrameY = 0;
					tileIII.HasTile = true;

					var tileIV = SafeGetTile(i + 2, j);
					tileIV.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tileIV.TileFrameX = 36;
					tileIV.TileFrameY = 0;
					tileIV.Slope = SlopeType.SlopeUpLeft;
					tileIV.HasTile = true;
				}
				break;
		}
	}
	public static void PlaceLargeUpCyanVineOre(int i, int j)
	{
		for (int x = 1; x < 5; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				var tile = SafeGetTile(i + x, j + y);
				if (x == 1 && y == 2)
					continue;
				if (x == 3 && y == 2)
					continue;
				if (x == 4 && y == 2)
					continue;
				if (x == 1 && y == 1)
				{
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tile.Slope = SlopeType.SlopeUpRight;
					tile.TileFrameX = (short)(x * 18);
					tile.TileFrameY = (short)(y * 18);
					tile.HasTile = true;
					continue;
				}
				if (x == 4 && y == 1)
				{
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
					tile.Slope = SlopeType.SlopeUpLeft;
					tile.TileFrameX = (short)(x * 18);
					tile.TileFrameY = (short)(y * 18);
					tile.HasTile = true;
					continue;
				}
				if (x == 2 && y == 0)
				{
					tile.TileType = (ushort)ModContent.TileType<CyanVineOreLargeUp>();
					tile.TileFrameX = 36;
					tile.TileFrameY = 0;
					tile.HasTile = true;
					continue;
				}
				tile.TileType = (ushort)ModContent.TileType<CyanVineOreTile>();
				tile.TileFrameX = (short)(x * 18);
				tile.TileFrameY = (short)(y * 18);
				tile.HasTile = true;
			}
		}
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
	/// 建造天穹树
	/// </summary>
	public static void BuildtheTreeWorld()
	{
		Main.statusText = "YggdrasilStart";
		ShapeTile("Tree.bmp", 0, 0, 1);
		Main.statusText = "YggdrasilWall";
		ShapeTile("TreeWall.bmp", 0, 0, 2);
		SmoothTile();

		Main.statusText = "YggdrasilTown";
		ShapeTile("Tree.bmp", 0, 0, 3);
		Main.statusText = "YggdrasilOre";
		ShapeTile("Tree.bmp", 0, 0, 4);
	}
	private static void SmoothTile(int a = 0, int b = 0, int c = 0, int d = 0)
	{
		for (int x = 20 + b; x < 980 - d; x += 1)
		{
			for (int y = 20 + a; y < 11980 - c; y += 1)
			{

				Tile.SmoothSlope(x + a, y + b, false);
				WorldGen.TileFrame(x + a, y + b, true, false);
				WorldGen.SquareWallFrame(x + a, y + b, true);
			}
		}
	}
}

