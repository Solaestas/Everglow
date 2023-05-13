using Everglow.Myth.Common;
using Everglow.Myth.MagicWeaponsReplace.Items;
using Everglow.Myth.TheFirefly.Pylon;
using Everglow.Myth.TheFirefly.Tiles;
using Everglow.Myth.TheFirefly.Walls;
using Everglow.Commons.TileHelper;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace Everglow.Myth.TheFirefly.WorldGeneration;

public class MothLand : ModSystem
{
	public override void PostUpdateEverything()
	{
		//if (Main.mouseRight && Main.mouseRightRelease && Main.keyState.PressingShift())
		//{
		//    BuildShabbyCastle();
		//    //Main.NewText(SubWorldModule.SubworldSystem.IsActive<MothWorld>());
		//}

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
	public static void BuildShabbyCastle()
	{
		Point16 sbpp = ShabbyPylonPos();
		string Path = "MapIOResources/ShabbyCastle0" + (Main.rand.Next(7) + 1) + ".mapio";
		var mapIO = new Commons.TileHelper.MapIO(sbpp.X, sbpp.Y);
		int Height = mapIO.ReadHeight(ModIns.Mod.GetFileStream("Myth/" + Path));
		QuickBuild(sbpp.X, sbpp.Y - Height / 2, Path);

		var pylonBottom = new Point(sbpp.X + Main.rand.Next(8, 16), sbpp.Y - Height / 2 + 8);
		ushort PylonType = (ushort)ModContent.TileType<ShabbyPylon>();
		PylonSystem.Instance.shabbyPylonEnable = false;
		for (int a = 0; a < 12; a++)
		{
			pylonBottom.Y++;
			if (Main.tile[pylonBottom.X, pylonBottom.Y].HasTile)
			{
				pylonBottom.Y -= 1;
				break;
			}
		}
		for (int i = -2; i <= 2; i++)
		{
			var PylonTile = Main.tile[pylonBottom.X + i, pylonBottom.Y + 1];
			PylonTile.TileType = TileID.GrayBrick;
			PylonTile.HasTile = true;
			PylonTile.Slope = SlopeType.Solid;
			PylonTile.IsHalfBlock = false;
		}

		TileObject.CanPlace(pylonBottom.X, pylonBottom.Y, PylonType, 0, 0, out var tileObject);
		TileObject.Place(tileObject);
		TileObjectData.CallPostPlacementPlayerHook(pylonBottom.X, pylonBottom.Y, PylonType, 0, 0, 0, tileObject);
		//TODO:有概率会爆掉，需要修复
		//switch (Main.rand.Next(5))
		//{
		//    case 0:
		//        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle20x23Style2.mapio");
		//        break;
		//    case 1:
		//        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle21x26Style1.mapio");
		//        break;
		//    case 2:
		//        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle22x22Style0.mapio");
		//        break;
		//    case 3:
		//        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle22x26Style3.mapio");
		//        break;
		//    case 4:
		//        QuickBuild(sbpp.X, sbpp.Y - 13, "MapIOResources/ShabbyPylonWithCastle22x26Style4.mapio");
		//        break;
		//}
	}

	internal class MothLandGenPass : GenPass
	{
		public MothLandGenPass() : base("MothLand", 500)
		{
		}

		public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildMothCave");
			BuildMothCave();
			Main.spawnTileX = 723;
			Main.spawnTileY = 226;
		}
	}

	internal class WorldMothLandGenPass : GenPass
	{
		public WorldMothLandGenPass() : base("MothLand", 500)
		{
		}

		public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everglow.Common.WorldSystem.BuildWorldMothCave");
			BuildWorldMothCave();
			BuildShabbyCastle();
		}
	}


	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) => tasks.Add(new WorldMothLandGenPass());

	/// <summary>
	/// 地形中心坐标
	/// </summary>
	public int fireflyCenterX = 400;

	public int fireflyCenterY = 300;

	public override void SaveWorldData(TagCompound tag)
	{
		tag["FIREFLYcenterX"] = fireflyCenterX;
		tag["FIREFLYcenterY"] = fireflyCenterY;

		var fireFlyTree = ModContent.GetInstance<FluorescentTree>();
		var list = new List<TagCompound>();
		foreach (var (x, y, style) in fireFlyTree.GetRopeStyleList())
		{
			list.Add(new TagCompound() {
				{ "x", x },
				{ "y", y },
				{ "style", style },
			});
		}
		tag.Set("FIREFLY_FireflyTree", list);

		//using (MemoryStream stream = new())
		//{
		//    using (BinaryWriter writer = new(stream))
		//    {
		//        var ropeinfos = ModContent.GetInstance<FluorescentTree>().GetRopeStyleList();
		//        writer.Write(ropeinfos.Count);
		//        ropeinfos.ForEach(info =>
		//        {
		//            writer.Write(info.x);
		//            writer.Write(info.y);
		//            writer.Write(info.style);
		//        });
		//        tag.Set("Ropes", stream.GetBuffer());
		//    }
		//}

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
		fireflyCenterX = tag.GetAsInt("FIREFLYcenterX");
		fireflyCenterY = tag.GetAsInt("FIREFLYcenterY");

		if (tag.ContainsKey("FIREFLY_FireflyTree"))
		{
			var fireFlyTree = ModContent.GetInstance<FluorescentTree>();
			var listTag = tag.GetList<TagCompound>("FIREFLY_FireflyTree");
			var ropeData = new List<(int x, int y, int style)>();
			foreach (var item in listTag)
			{
				int x = item.Get<int>("x");
				int y = item.Get<int>("y");
				int style = item.GetInt("style");
				ropeData.Add((x, y, style));
			}
			fireFlyTree.InitTreeRopes(ropeData);
		}

		//if (tag.TryGet("Ropes", out byte[] ropedata))
		//{
		//    using (MemoryStream stream = new(ropedata))
		//    {
		//        using (BinaryReader reader = new(stream))
		//        {
		//            List<(int, int, int)> ropes = new();
		//            int count = reader.ReadInt32();
		//            for (int i = 0; i < count; i++)
		//            {
		//                ropes.Add((reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()));
		//            }
		//            ModContent.GetInstance<FluorescentTree>().InitTreeRopes(ropes);
		//        }
		//    }
		//}

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
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Myth/TheFirefly/WorldGeneration/" + Shapepath);
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
					switch (type)//21是箱子
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
							if (pixel.R == 45 && pixel.G == 49 && pixel.B == 255)
							{
								MythUtils.PlaceFrameImportantTiles(a + x, b + y, 3, 4, ModContent.TileType<FireflyPylon>());
								TEModdedPylon moddedPylon = ModContent.GetInstance<FireflyPylonTileEntity>();
								moddedPylon.Position = new Point16(a + x, b + y);
								//TODO:I need help to generate map Icon;

								ushort PylonType = (ushort)ModContent.TileType<FireflyPylon>();
								var bottom = new Point(a + x, b + y);
								for (int i = -2; i <= 2; i++)
								{
									var PylonTile = Main.tile[bottom.X + i, bottom.Y + 1];
									PylonTile.TileType = (ushort)ModContent.TileType<DarkCocoon>();
									PylonTile.HasTile = true;
									PylonTile.Slope = SlopeType.Solid;
									PylonTile.IsHalfBlock = false;
									WorldGen.TileFrame(bottom.X + i, bottom.Y + 1);
								}

								TileObject.CanPlace(bottom.X, bottom.Y, PylonType, 0, 0, out var tileObject);
								TileObject.Place(tileObject);
								TileObjectData.CallPostPlacementPlayerHook(bottom.X, bottom.Y, PylonType, 0, 0, 0, tileObject);
							}
							break;
					}
				}
			}
		});
	}

	/// <summary>
	/// 建造流萤之茧
	/// </summary>
	public static void BuildMothCave()
	{
		//Point16 AB = CocoonPos();
		int a = 230;//AB.X;
		int b = 200;//AB.Y;
		MothLand mothLand = ModContent.GetInstance<MothLand>();
		mothLand.fireflyCenterX = a + 140;
		mothLand.fireflyCenterY = b + 140;
		Main.statusText = "CocoonStart";
		ShapeTile("CocoonWorld.bmp", 0, 0, 1);
		ShapeTile("CocoonWorldWall.bmp", 0, 0, 2);
		Main.statusText = "CocoonKillStart";
		ShapeTile("CocoonSubKill.bmp", a, b, 0);
		Main.statusText = "CocoonStart";
		ShapeTile("CocoonSub.bmp", a, b, 1);
		Main.statusText = "CocoonWallStart";
		ShapeTile("CocoonSubWall.bmp", a, b, 2);
		Main.statusText = "CocoonAnotherStart";
		ShapeTile("CocoonSub.bmp", a, b, 3);
		SmoothMothTile(a, b);
		for (int x = 20; x < Main.maxTilesX - 20; x++)
		{
			for (int y = 20; y < Main.maxTilesY - 20; y++)
			{
				RandomUpdate(x, y, ModContent.TileType<DarkCocoon>());
			}
		}
	}

	public static void BuildWorldMothCave()
	{
		Point16 AB = CocoonPos();
		int a = AB.X;
		int b = AB.Y;
		MothLand mothLand = ModContent.GetInstance<MothLand>();
		mothLand.fireflyCenterX = a;
		mothLand.fireflyCenterY = b;
		Main.statusText = "CocoonKillStart";
		ShapeTile("WorldCocoonKill.bmp", a, b, 0);
		Main.statusText = "CocoonStart";
		ShapeTile("WorldCocoon.bmp", a, b, 1);
		Main.statusText = "CocoonWallStart";
		ShapeTile("WorldCocoonWall.bmp", a, b, 2);
		Main.statusText = "CocoonAnotherStart";
		ShapeTile("WorldCocoon.bmp", a, b, 3);
		SmoothMothTile(a, b);

		for (int x = 0; x < 28; x++)
		{
			for (int y = 0; y < 24; y++)
			{
				RandomUpdate(a + x, b + y, ModContent.TileType<DarkCocoon>());
			}
		}
	}

	private static int GetCrash(int PoX, int PoY)
	{
		int CrashCount = 0;
		ushort[] DangerTileType = new ushort[]
		{
			41,//蓝地牢砖
                43,//绿地牢砖
                44,//粉地牢砖
                48,//尖刺
                49,//水蜡烛
                50,//书
                137,//神庙机关
                226,//神庙石砖
                232,//木刺
                237,//神庙祭坛
                481,//碎蓝地牢砖
                482,//碎绿地牢砖
                483//碎粉地牢砖
            };
		for (int x = -256; x < 257; x += 8)
		{
			for (int y = -128; y < 129; y += 8)
			{
				if (Array.Exists(DangerTileType, Ttype => Ttype == Main.tile[x + PoX, y + PoY].TileType))
					CrashCount++;
			}
		}
		return CrashCount;
	}

	private static int GetMergeToJungle(int PoX, int PoY)
	{
		int CrashCount = 0;
		ushort[] MustHaveTileType = new ushort[]
		{
			TileID.JungleGrass,//丛林草方块
                TileID.JunglePlants,//丛林草
                TileID.JungleVines,//丛林藤
                TileID.JunglePlants2,//高大丛林草
                TileID.PlantDetritus//丛林花
            };
		for (int x = -256; x < 257; x += 8)
		{
			for (int y = -128; y < 129; y += 8)
			{
				if (Array.Exists(MustHaveTileType, Ttype => Ttype == Main.tile[x + PoX, y + PoY].TileType))
					CrashCount++;
			}
		}
		return CrashCount;
	}

	/// <summary>
	/// 获取一个不与原版地形冲突的点
	/// </summary>
	/// <returns></returns>
	private static Point16 CocoonPos()
	{
		int PoX = Main.rand.Next(300, Main.maxTilesX - 600);
		int PoY = Main.rand.Next(500, Main.maxTilesY - 700);

		while (GetCrash(PoX, PoY) > 0 || GetMergeToJungle(PoX, PoY) <= 10)
		{
			PoX = Main.rand.Next(300, Main.maxTilesX - 600);
			PoY = Main.rand.Next(500, Main.maxTilesY - 700);
		}
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
	public static void BuildFluorescentTree(int i, int j, int height = 0)
	{
		if (j < 30)
			return;
		int Height = Main.rand.Next(7, height);

		for (int g = 0; g < Height; g++)
		{
			Tile tile = Main.tile[i, j - g];
			if (g > 3)
			{
				if (Main.rand.NextBool(5))
				{
					Tile tileLeft = Main.tile[i - 1, j - g];
					tileLeft.TileType = (ushort)ModContent.TileType<FluorescentTree>();
					tileLeft.TileFrameY = 4;
					tileLeft.TileFrameX = (short)Main.rand.Next(4);
					tileLeft.HasTile = true;
				}
				if (Main.rand.NextBool(5))
				{
					Tile tileRight = Main.tile[i + 1, j - g];
					tileRight.TileType = (ushort)ModContent.TileType<FluorescentTree>();
					tileRight.TileFrameY = 5;
					tileRight.TileFrameX = (short)Main.rand.Next(4);
					tileRight.HasTile = true;
				}
			}
			if (g == 0)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = 0;
				tile.TileFrameX = 0;
				tile.HasTile = true;
				continue;
			}
			if (g == 1)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = -1;
				tile.TileFrameX = 0;
				tile.HasTile = true;
				continue;
			}
			if (g == 2)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = 3;
				tile.TileFrameX = (short)Main.rand.Next(4);
				tile.HasTile = true;
				continue;
			}
			if (g == Height - 1)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = 2;
				tile.TileFrameX = 0;
				tile.HasTile = true;
				continue;
			}
			tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
			tile.TileFrameY = 1;
			tile.TileFrameX = (short)Main.rand.Next(12);
			tile.HasTile = true;
		}
	}
	public static void RandomUpdate(int i, int j, int Type)
	{
		if (Main.tile[i, j].TileType != Type || !Main.tile[i, j].HasTile)
			return;
		if (Main.rand.NextBool(4))
		{
			if (Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 2, j].Slope == SlopeType.Solid && Main.tile[i - 2, j].Slope == SlopeType.Solid &&
				Main.tile[i, j + 1].Slope == SlopeType.Solid && Main.tile[i + 1, j + 1].Slope == SlopeType.Solid && Main.tile[i - 1, j + 1].Slope == SlopeType.Solid && Main.tile[i + 2, j + 1].Slope == SlopeType.Solid && Main.tile[i - 2, j + 1].Slope == SlopeType.Solid)//树木
			{
				int MaxHeight = 0;
				for (int x = -2; x < 3; x++)
				{
					for (int y = -1; y > -30; y--)
					{
						if (j + y > 20)
						{
							if (Main.tile[i + x, j + y].HasTile || Main.tile[i + x, j + y].LiquidAmount > 3)
								return;
						}
						MaxHeight = -y;
					}
				}
				if (MaxHeight > 7)
					BuildFluorescentTree(i, j - 1, MaxHeight);
			}
		}

		if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i, j - 1].LiquidAmount > 0)
		{
			Tile tile = Main.tile[i, j - 1];
			tile.TileType = (ushort)ModContent.TileType<LampLotus>();
			tile.HasTile = true;
			tile.TileFrameX = (short)(28 * Main.rand.Next(8));
		}
		if (Main.rand.NextBool(6))//黑萤藤蔓
		{
			Tile t0 = Main.tile[i, j];

			Tile t2 = Main.tile[i, j + 1];
			if (t0.Slope == SlopeType.Solid && !t2.HasTile)
			{
				t2.TileType = (ushort)ModContent.TileType<BlackVine>();
				t2.HasTile = true;
				t2.TileFrameY = (short)(Main.rand.Next(6, 9) * 18);
			}
		}
		if (Main.rand.NextBool(16))//流萤滴
		{
			int count = 0;
			for (int x = -1; x <= 1; x++)
			{
				for (int y = 1; y <= 3; y++)
				{
					Tile t0 = Main.tile[i + x, j + y];
					if (t0.HasTile)
						count++;
					Tile t1 = Main.tile[i + x, j + y - 1];
					if (y == 1 && (!t1.HasTile || t1.Slope != SlopeType.Solid))
						count++;
				}
			}
			if (count == 0)
				MythUtils.PlaceFrameImportantTiles(i - 1, j + 1, 3, 3, ModContent.TileType<Tiles.Furnitures.GlowingDrop>());

		}
		if (!Main.tile[i, j - 1].HasTile && !Main.tile[i + 1, j - 1].HasTile && !Main.tile[i - 1, j - 1].HasTile && Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid)//黑萤苣
		{
			Tile t1 = Main.tile[i, j - 1];
			Tile t2 = Main.tile[i, j - 2];
			Tile t3 = Main.tile[i, j - 3];
			for (int x = -1; x < 2; x++)
			{
				for (int y = -3; y < 4; y++)
				{
					if (Main.tile[i + x, j + y].LiquidAmount > 3)
						return;
				}
			}
			if (Main.rand.NextBool(2))
			{
				switch (Main.rand.Next(1, 10))
				{
					case 1:
						t1.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t2.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t1.HasTile = true;
						t2.HasTile = true;
						short numa = (short)(Main.rand.Next(0, 6) * 48);
						t1.TileFrameX = numa;
						t2.TileFrameX = numa;
						t1.TileFrameY = 16;
						t2.TileFrameY = 0;
						break;

					case 2:
						t1.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t2.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t1.HasTile = true;
						t2.HasTile = true;
						short num = (short)(Main.rand.Next(0, 6) * 48);
						t2.TileFrameX = num;
						t1.TileFrameX = num;
						t1.TileFrameY = 16;
						t2.TileFrameY = 0;
						break;

					case 3:
						t1.TileType = (ushort)ModContent.TileType<BlackStarShrub>();
						t2.TileType = (ushort)ModContent.TileType<BlackStarShrub>();
						t3.TileType = (ushort)ModContent.TileType<BlackStarShrub>();
						t1.HasTile = true;
						t2.HasTile = true;
						t3.HasTile = true;
						short num1 = (short)(Main.rand.Next(0, 6) * 72);
						t3.TileFrameX = num1;
						t2.TileFrameX = num1;
						t1.TileFrameX = num1;
						t1.TileFrameY = 32;
						t2.TileFrameY = 16;
						t3.TileFrameY = 0;
						break;

					case 4:
						t1.TileType = (ushort)ModContent.TileType<BlueBlossom>();
						t2.TileType = (ushort)ModContent.TileType<BlueBlossom>();
						t3.TileType = (ushort)ModContent.TileType<BlueBlossom>();
						t1.HasTile = true;
						t2.HasTile = true;
						t3.HasTile = true;
						short num2 = (short)(Main.rand.Next(0, 12) * 120);
						t3.TileFrameX = num2;
						t2.TileFrameX = num2;
						t1.TileFrameX = num2;
						t1.TileFrameY = 32;
						t2.TileFrameY = 16;
						t3.TileFrameY = 0;
						break;

					case 5:
						WorldGen.Place3x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFrenLarge>(), Main.rand.Next(3));
						break;

					case 6:
						WorldGen.Place2x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFren>(), Main.rand.Next(3));
						break;

					case 7:
						WorldGen.Place3x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFrenLarge>(), Main.rand.Next(3));
						break;

					case 8:
						WorldGen.Place2x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFren>(), Main.rand.Next(3));
						break;

					case 9:
						WorldGen.Place2x1(i - 1, j - 1, (ushort)ModContent.TileType<CocoonRock>(), Main.rand.Next(3));
						break;

					case 10:
						WorldGen.Place2x1(i - 1, j - 1, (ushort)ModContent.TileType<CocoonRock>(), Main.rand.Next(3));
						break;
				}
			}
		}
	}
	public override void PostWorldGen()
	{
		bool placed = false;
		for (int offX = -36; offX < 36; offX += 72)
		{
			for (int offY = -36; offY < 12; offY++)
			{
				if (!placed)
					placed = TrySpellbookChest(Main.spawnTileX + offX, Main.spawnTileY + offY, 36);
				else
				{
					break;
				}
			}
		}
	}
	private bool TrySpellbookChest(int startX, int startY, int rangeX = 16)
	{
		int[] legalTile = new int[]
		{
				TileID.Stone,
				TileID.Grass,
				TileID.Dirt,
				TileID.SnowBlock,
				TileID.IceBlock,
				TileID.ClayBlock,
				TileID.Mud,
				TileID.JungleGrass,
				TileID.Sand
		};
		bool canPlace = true;
		int dir = -1;
		if (startY < 4 || startY > Main.maxTilesY - 1)
			return false;
		for (int x = startX; dir > 0 ? x <= startX + rangeX : x >= startX - rangeX; x += dir)
		{
			if (x < 0 || x > Main.maxTilesX - 4)
				continue;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Tile tile = Framing.GetTileSafely(x + i, startY - j);
					if (WorldGen.SolidOrSlopedTile(tile))
					{
						canPlace = false;
						break;
					}
				}
			}
			if (canPlace)
			{
				for (int i = 0; i < 4; i++)
				{
					Tile tile = Framing.GetTileSafely(x + i, startY + 1);
					if (!WorldGen.SolidTile(tile) || !legalTile.Contains(tile.TileType))
					{
						canPlace = false;
						break;
					}
				}
			}
			if (canPlace)
			{
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						WorldGen.KillTile(x + i, startY - j);
					}
				}
				WorldGen.PlaceTile(x, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 1, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 2, startY + 1, TileID.MeteoriteBrick, forced: true);
				WorldGen.PlaceTile(x + 3, startY + 1, TileID.MeteoriteBrick, forced: true);
				int c = WorldGen.PlaceChest(x + 1, startY, style: 49);
				Chest chest = Main.chest[c];
				if (chest != null)
				{
					chest.name = "Spellbook Demo";
					chest.item[0].SetDefaults(ModContent.ItemType<CrystalSkull>());
					chest.item[1].SetDefaults(ItemID.WaterBolt);
					chest.item[2].SetDefaults(ItemID.DemonScythe);
					chest.item[3].SetDefaults(ItemID.BookofSkulls);
					chest.item[4].SetDefaults(ItemID.CrystalStorm);
					chest.item[5].SetDefaults(ItemID.CursedFlames);
					chest.item[6].SetDefaults(ItemID.GoldenShower);
					chest.item[7].SetDefaults(ItemID.MagnetSphere);
					chest.item[8].SetDefaults(ItemID.RazorbladeTyphoon);
					chest.item[9].SetDefaults(ItemID.LunarFlareBook);
				}
				return true;
			}
			if (x <= startX - rangeX)
			{
				dir = 1;
				x = startX;
			}
		}
		return false;
	}
}
