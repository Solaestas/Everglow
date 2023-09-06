using Everglow.Commons.Utilities;
using Everglow.Myth.Common;
using Everglow.Myth.MagicWeaponsReplace.Items;
using Everglow.Myth.TheFirefly.Pylon;
using Everglow.Myth.TheFirefly.Tiles;
using Everglow.Myth.TheFirefly.Walls;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace Everglow.Ocean.WorldGeneration.OceanGen;

public class OceanGen : ModSystem
{
	public override void PostUpdateEverything()
	{
		
	}
	public static void QuickBuild(int x, int y, string Path)
	{
		var mapIO = new Commons.TileHelper.MapIO(x, y);

		mapIO.Read(ModIns.Mod.GetFileStream("Ocean/" + Path));

		var it = mapIO.GetEnumerator();
		while (it.MoveNext())
		{
			WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
			WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
		}
	}

	internal class OceanWorldGenPass : GenPass
	{
		public OceanWorldGenPass() : base("OceanGen", 500)
		{
		}

		public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildOceanWorld");
			BuildOceanWorld();
			Main.spawnTileX = 723;
			Main.spawnTileY = 226;
		}
	}

	/// <summary>
	/// 地形中心坐标
	/// </summary>
	public int oceanCenterX = 400;

	public int oceanCenterY = 300;

	public override void SaveWorldData(TagCompound tag)
	{
		tag["OCEANcenterX"] = oceanCenterX;
		tag["OCEANcenterY"] = oceanCenterY;

		var list = new List<TagCompound>();

		//if (Main.ActiveWorldFileData == SubWorldModule.SubworldSystem.root && SubWorldModule.SubworldSystem.current is not null)
		//{
		//    tag.Add("ExitTo", SubWorldModule.SubworldSystem.current.FullName);
		//    tag.Add("ExitPosX", Main.LocalPlayer.Center.X);
		//    tag.Add("ExitPosY", Main.LocalPlayer.Center.Y);
		//}

		//tag["DepartX"] = (int)(Main.LocalPlayer.Center.X - Main.LocalPlayer.velocity.X);
		//tag["DepartY"] = (int)(Main.LocalPlayer.Center.Y - Main.LocalPlayer.velocity.Y);
	}

	public override void LoadWorldData(TagCompound tag)
	{
		oceanCenterX = tag.GetAsInt("OCEANcenterX");
		oceanCenterY = tag.GetAsInt("OCEANcenterY");

		//if (tag.TryGet("ExitTo", out string subworldname) && SubWorldModule.SubworldSystem.cache is not null && SubWorldModule.SubworldSystem.cache.Name == subworldname)
		//{
		//    Main.LocalPlayer.Center = new(tag.Get<int>("ExitPosX"), tag.Get<int>("ExitPosY"));
		//}

		//if(tag.ContainsKey("DepartX") && tag.ContainsKey("DepartY"))
		//{
		//    Main.LocalPlayer.position = new Vector2(tag.GetAsInt("DepartX"), tag.GetAsInt("DepartY"));
		//}
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
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Ocean/WorldGeneration/" + Shapepath);
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				var pixelRow = accessor.GetRowSpan(y);
				if (y + b < 20)
					continue;
				if (y + b > Main.maxTilesY - 20)
					break;
				for (int x = 0; x < pixelRow.Length; x++)
				{
					if (x + a < 20)
						continue;
					if (x + a > Main.maxTilesX - 20)
						break;
					ref var pixel = ref pixelRow[x];
					Tile tile = Main.tile[x + a, y + b];
					switch (type)//TODO: REPLACE WITH APPROPRIATE TILE AND PIXEL COLOR
					{
						case 0:
							if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(255, 0, 0))
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									tile.ClearEverything();
							}
							break;

						case 1:
							if (pixel.R == 56 && pixel.G == 48 && pixel.B == 61)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(56, 48, 61))
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.TileType = (ushort)ModContent.TileType<DarkCocoon>();
									tile.HasTile = true;
								}
							}
							if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(56, 48, 61))
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.TileType = (ushort)ModContent.TileType<DarkCocoonSpecial>();
									tile.HasTile = true;
								}
							}
							if (pixel.R == 35 && pixel.G == 49 && pixel.B == 122)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(56, 48, 61))
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.TileType = (ushort)ModContent.TileType<DarkCocoonMoss>();
									tile.HasTile = true;
								}
							}
							if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255)//pixel == new SixLabors.ImageSharp.PixelFormats.Rgb24(0, 0, 255))
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
								{
									tile.LiquidType = LiquidID.Water;
									tile.LiquidAmount = 200;
									tile.HasTile = false;
									//WorldGen.PlaceLiquid(x, y, byte.MaxValue, 255);
								}
							}
							break;

						case 2:
							if (pixel.R == 0 && pixel.G == 0 && pixel.B == 5)// == new SixLabors.ImageSharp.PixelFormats.Rgb24(0, 0, 5))
							{
								if (tile.TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
									tile.WallType = (ushort)ModContent.WallType<DarkCocoonWall>();
							}
							break;

						case 3:
							if (pixel.R == 165 && pixel.G == 0 && pixel.B == 255)
								MythUtils.PlaceFrameImportantTiles(a + x, b + y, 5, 7, ModContent.TileType<MothWorldDoor>());
							break;
					}
				}
			}
		});
	}

	/// <summary>
	/// 建造流萤之茧
	/// </summary>
	public static void BuildOceanWorld()
	{
		//Point16 AB = OceanPos();
		int a = 230;//AB.X;
		int b = 200;//AB.Y;
		OceanGen OceanGen = ModContent.GetInstance<OceanGen>();
		OceanGen.oceanCenterX = a + 140;
		OceanGen.oceanCenterY = b + 140;
		Main.statusText = "OceanStart";
		ShapeTile("OceanContinent.bmp", 0, 0, 1);
		ShapeTile("OceanWorldWall.bmp", 0, 0, 2);
		Main.statusText = "OceanKillStart";
		ShapeTile("OceanSubKill.bmp", a, b, 0);
		Main.statusText = "OceanStart";
		ShapeTile("OceanSub.bmp", a, b, 1);
		Main.statusText = "OceanWallStart";
		ShapeTile("OceanSubWall.bmp", a, b, 2);
		Main.statusText = "OceanAnotherStart";
		ShapeTile("OceanSub.bmp", a, b, 3);
		SmoothMothTile(a, b);
	}

	/// <summary>
	/// 获取一个不与原版地形冲突的点
	/// </summary>
	/// <returns></returns>
	private static Point16 CocoonPos()
	{
		int PoX = Main.rand.Next(300, Main.maxTilesX - 600);
		int PoY = Main.rand.Next(500, Main.maxTilesY - 700);

		return new Point16(PoX, PoY);
	}

	/// <summary>
	/// 获取一个出生地附近的平坦地面
	/// </summary>
	/// <returns></returns>
	private static Point16 ShabbyPylonPos()
	{
		int PoX = (int)(Main.rand.Next(80, 160) * (Main.rand.Next(2) - 0.5f) * 2 - 20 + Main.maxTilesX / 2);
		int PoY = 160;

		while (!IsTileSmooth(new Point(PoX, PoY)))
		{
			PoX = (int)(Main.rand.Next(80, 240) * (Main.rand.Next(2) - 0.5f) * 2 - 20 + Main.maxTilesX / 2);
			for (int y = 160; y < Main.maxTilesY / 3; y++)
			{
				if (Main.tile[PoX, y].HasTile && Main.tile[PoX, y].TileType != TileID.Trees)
				{
					PoY = y;
					break;
				}
			}
		}
		return new Point16(PoX, PoY);
	}
	/// <summary>
	/// 判定是否平坦
	/// </summary>
	/// <param name="point"></param>
	/// <param name="Width"></param>
	/// <returns></returns>
	private static bool IsTileSmooth(Point point, int Width = 22)
	{
		if (point.X > Main.maxTilesX - 20 || point.Y > Main.maxTilesY - 20 || point.X < 20 || point.Y < 20)
			return false;
		int x = point.X;
		int y = point.Y;
		var LeftTile = Main.tile[x, y];
		var RightTile = Main.tile[x + Width, y];
		var LeftTileUp = Main.tile[x, y - 1];
		var RightTileUp = Main.tile[x + Width, y - 1];
		if (!LeftTileUp.HasTile && !RightTileUp.HasTile)
		{
			if (LeftTile.HasTile && RightTile.HasTile)
				return true;
		}
		return false;
	}
	private static void SmoothMothTile(int a, int b, int width = 256, int height = 512)
	{
		for (int y = 0; y < width; y += 1)
		{
			for (int x = 0; x < height; x += 1)
			{
				if (Main.tile[x + a, y + b].TileType == (ushort)ModContent.TileType<DarkCocoon>())
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
