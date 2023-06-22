using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.WorldGeneration;
using Everglow.Myth.TheTusk.Tiles;
using SubworldLibrary;
using Terraria.Graphics.Effects;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Everglow.Myth.TheTusk.WorldGeneration;

public class TuskGen : ModSystem
{
	public override void PostUpdateEverything()
	{
	}
	public static void QuickBuild(int x, int y, string Path)
	{
		var mapIO = new Commons.TileHelper.MapIO(x, y);

		mapIO.Read(ModIns.Mod.GetFileStream("Myth/" + Path));

		var it = mapIO.GetEnumerator();
		while (it.MoveNext())
		{
			WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
			WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
		}
	}
	/// <summary>
	/// 判定是否开启地形
	/// </summary>
	/// <returns></returns>
	public static bool TuskLandActive()
	{
		if(SubworldSystem.AnyActive(ModIns.Mod))
		{
			return false;
		}
		TuskGen tuskGen = ModContent.GetInstance<TuskGen>();
		Vector2 TuskBiomeCenter = new Vector2(tuskGen.tuskCenterX, tuskGen.tuskCenterY) * 16;
		Vector2 v0 = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f - TuskBiomeCenter;
		return v0.Length() < 1000;
	}
	internal float TuskS = 0;
	public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
	{
		if (TuskLandActive())
		{
			if (TuskS < 1)
			{
				TuskS += 0.01f;
			}
			else
			{
				TuskS = 1f;
			}


			if (!SkyManager.Instance["TuskSky"].IsActive())
			{
				SkyManager.Instance.Activate("TuskSky");
			}
		}
		else
		{
			if (TuskS > 0)
			{
				TuskS -= 0.01f;
			}
			else
			{
				TuskS = 0;
			}
			if (SkyManager.Instance["TuskSky"].IsActive())
			{
				SkyManager.Instance.Deactivate("TuskSky");
			}
		}
		tileColor *= 1 - TuskS * 0.4f;
		tileColor.G = (byte)(tileColor.G * (1 - TuskS * 0.4f));
		tileColor.B = (byte)(tileColor.B * (1 - TuskS * 0.4f));
		backgroundColor *= 1 - TuskS * 0.4f;
		backgroundColor.A = 255;
	}
	internal class WorldTuskLandGenPass : GenPass
	{
		public WorldTuskLandGenPass() : base("TuskLand", 500)//TODO:给大地安装血肉之颌
		{
		}

		public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Myth.Common.WorldSystem.BuildWorldTuskTable");
			BuildTuskLand();
		}
	}
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
	{
		tasks.Add(new WorldTuskLandGenPass());
	}

	/// <summary>
	/// 地形中心坐标
	/// </summary>
	public int tuskCenterX = 400;

	public int tuskCenterY = 300;

	/// <summary>
	/// 这里偷懒,用固定点标记獠牙的生物群系
	/// </summary>
	/// <param name="tag"></param>
	public override void SaveWorldData(TagCompound tag)
	{
		tag["TUSKcenterX"] = tuskCenterX;
		tag["TUSKcenterY"] = tuskCenterY;
	}
	public override void LoadWorldData(TagCompound tag)
	{
		tuskCenterX = tag.GetAsInt("TUSKcenterX");
		tuskCenterY = tag.GetAsInt("TUSKcenterY");
	}
	/// <summary>
	/// 原版方法,让物块边缘自然
	/// </summary>
	/// <param name="Shapepath"></param>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="type"></param>
	public static void ShapeTile(string Shapepath, int a, int b, int type)
	{
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Myth/TheTusk/WorldGeneration/" + Shapepath);
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				var pixelRow = accessor.GetRowSpan(y);
				if (y + b < 20)
				{
					continue;
				}

				if (y + b > Main.maxTilesY - 20)
				{
					break;
				}

				for (int x = 0; x < pixelRow.Length; x++)
				{
					if (x + a < 20)
					{
						continue;
					}

					if (x + a > Main.maxTilesX - 20)
					{
						break;
					}

					ref var pixel = ref pixelRow[x];
					Tile tile = Main.tile[x + a, y + b];
					switch (type)//21是箱子
					{
						case 0:
							if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.ClearEverything();
								}
							}
							break;

						case 1:
							var Plc = new Vector2[30];
							var Plc2 = new Vector2[60];
							if (pixel.R == 158 && pixel.G == 26 && pixel.B == 37)
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.TileType = (ushort)ModContent.TileType<TuskFlesh>();
									tile.HasTile = true;
								}
							}
							if (pixel.R == 91 && pixel.G == 27 && pixel.B == 52)
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.TileType = TileID.BoneBlock;
									tile.HasTile = true;
								}
							}
							if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.TileType = (ushort)ModContent.TileType<AbTuskFlesh>();
									tile.HasTile = true;
								}
							}
							break;

						case 2:
							if (pixel.R == 96 && pixel.G == 8 && pixel.B == 14)
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.WallType = (ushort)ModContent.WallType<Walls.BloodyStoneWall>();
								}
							}
							break;
					}
				}
			}
		});
	}
	/// <summary>
	/// 主要程序
	/// </summary>
	public static void BuildTuskLand()
	{
		Point abPos = GetTuskLandPosition();
		int a = abPos.X;
		int b = abPos.Y;
		Main.statusText = "CocoonStart";
		ShapeTile("BloodPlat.bmp", a, b, 1);
		Main.statusText = "TuskWallStart";
		ShapeTile("BloodPlatWall.bmp", a, b, 2);
		SmoothTuskTile(a, b, 160, 80);
		TuskGen tuskGen = ModContent.GetInstance<TuskGen>();
		tuskGen.tuskCenterX = a + 80;
		tuskGen.tuskCenterY = b + 10;
		BuildTuskArray(a, b);
	}
	/// <summary>
	/// 制造獠牙地形下半部分的一个盘状物
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public static void BuildTuskArray(int x, int y)
	{
		x += 80;
		y += 120;
		for (int Dy = 0; Dy < 300; Dy++)
		{
			if (Main.tile[x, y + Dy].HasTile)
			{
				y += Dy + 12;
				break;
			}
		}
		for (int i = -30; i < 31; i++)
		{
			for (int j = -30; j < 31; j++)
			{
				float Length = new Vector2(i, j).Length();
				var tile = Main.tile[x + i, y + j];
				if (Length is < 30f and > 18f)
				{
					if (tile.HasTile)
					{
						if (Main.tileSolid[tile.TileType])
						{
							tile.TileType = (ushort)ModContent.TileType<TuskFlesh>();
						}
						else
						{
							tile.ClearEverything();
						}
					}
					if (Length is < 24f and > 20f && j > -7)
					{
						tile.TileType = (ushort)ModContent.TileType<TuskFlesh>();
						tile.HasTile = true;
					}
				}
				if (Length < 19f)
				{
					if (tile.HasTile)
					{
						tile.HasTile = false;
					}
				}
				if (Length < 28f)
				{
					if (tile.WallType != 0 || (Length < 22f && j > -5))
					{
						tile.WallType = (ushort)ModContent.WallType<Walls.TuskFleshWall>();
					}
				}
			}
		}

		PlaceStone(x - 14, y, 1);
		PlaceStone(x - 6, y, 2);
		PlaceStone(x + 6, y, 3);
		PlaceStone(x + 14, y, 4);

		var tileWheel = Main.tile[x, y + 14];
		tileWheel.TileType = (ushort)ModContent.TileType<BloodyMossWheel>();
		tileWheel.HasTile = true;
	}
	/// <summary>
	/// 放置石碑
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="stonetype"></param>
	public static void PlaceStone(int x, int y, int stonetype)
	{
		for (int j = 0; j < 71; j++)
		{
			var tile = Main.tile[x, y + j];
			if (tile.HasTile)
			{
				for (int dx = -2; dx < 3; dx++)
				{
					var tileUp = Main.tile[x + dx, y + j - 1];
					tileUp.HasTile = false;
					var tileDown = Main.tile[x + dx, y + j];
					tileDown.TileType = (ushort)ModContent.TileType<TuskFlesh>();
					tileDown.HasTile = true;
				}
				switch (stonetype)
				{
					case 1:
						MythUtils.PlaceFrameImportantTiles(x, y + j - 7, 1, 7, ModContent.TileType<StrangeTuskStone1>());
						break;
					case 2:
						MythUtils.PlaceFrameImportantTiles(x, y + j - 7, 1, 7, ModContent.TileType<StrangeTuskStone2>());
						break;
					case 3:
						MythUtils.PlaceFrameImportantTiles(x, y + j - 7, 1, 7, ModContent.TileType<StrangeTuskStone3>());
						break;
					case 4:
						MythUtils.PlaceFrameImportantTiles(x, y + j - 7, 1, 7, ModContent.TileType<StrangeTuskStone4>());
						break;
				}
				break;
			}
		}
	}
	/// <summary>
	/// 抽选獠牙地形的地址
	/// </summary>
	/// <returns></returns>
	public static Point GetTuskLandPosition()
	{
		int a = (int)(Main.maxTilesX * 0.3);
		int b = (int)(Main.maxTilesY * 0.1);
		while (!CanPlaceTusk(new Point(a, b)))
		{
			a = (int)(Main.maxTilesX * Main.rand.NextFloat(0.1f, 0.88f));
			while (Math.Abs(a - Main.maxTilesX * 0.5f) < Main.maxTilesX * 0.1f)
			{
				a = (int)(Main.maxTilesX * Main.rand.NextFloat(0.1f, 0.88f));
			}

			b = (int)(Main.maxTilesY * Main.rand.NextFloat(0.11f, 0.31f));
		}
		return new Point(a, b);
	}
	/// <summary>
	/// 判定被抽出来的点是否具备建造獠牙地形的条件
	/// </summary>
	/// <param name="position"></param>
	/// <returns></returns>
	public static bool CanPlaceTusk(Point position)
	{
		if (position.X < 20 || position.Y - 60 < 20)
		{
			return false;
		}

		if (position.X + 160 > Main.maxTilesX - 20 || position.Y + 80 > Main.maxTilesY - 20)
		{
			return false;
		}

		for (int x = 0; x < 161; x++)
		{
			for (int y = -60; y < 81; y++)
			{
				if (Main.tile[x + position.X, y + position.Y].HasTile)
				{
					return false;
				}
			}
		}
		return true;
	}
	/// <summary>
	/// 让獠牙地里面的物块执行ShapeTile
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	private static void SmoothTuskTile(int a, int b, int width = 256, int height = 512)
	{
		for (int y = 0; y < width; y += 1)
		{
			for (int x = 0; x < height; x += 1)
			{
				if (Main.tile[x + a, y + b].TileType == (ushort)ModContent.TileType<TuskFlesh>())
				{
					Tile.SmoothSlope(x + a, y + b, false);
					WorldGen.TileFrame(x + a, y + b, true, false);
				}
				else
				{
					WorldGen.TileFrame(x + a, y + b, true, false);
				}
				WorldGen.SquareWallFrame(x + a, y + b, true);
			}
		}
	}
}
