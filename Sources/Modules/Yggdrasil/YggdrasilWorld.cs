using System.Reflection;
using Everglow.Commons.Mechanics.BiomesText;
using Everglow.Yggdrasil.YggdrasilTown.Biomes;
using SubworldLibrary;
using Terraria.WorldBuilding;

namespace Everglow.Yggdrasil;

internal class YggdrasilWorld : Subworld
{
	public static bool InYggdrasil => SubworldSystem.IsActive<YggdrasilWorld>();

	public static float YggdrasilTimer = 0;
	public Vector2 StoneCageOfChallengesCenter = Vector2.zeroVector;

	public override int Width => 2000;

	public override int Height => 21000;

	public override bool NormalUpdates => true;

	public override bool ShouldSave => false; // Only in debug mode,when published,turn true.

	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new WorldGeneration.YggdrasilWorldGeneration.YggdrasilWorldGenPass(),
	};

	public override void OnEnter()
	{
		SubworldSystem.hideUnderworld = true;
		YggdrasilTimer = 0;
		//for (int x = 20; x < Main.maxTilesX - 20; x++)
		//{
		//	for (int y = 20; y < Main.maxTilesY - 20; y++)
		//	{
		//	}
		//}
		YggdrasilTownBiome.CheckedBiomeCenter = false;
	}

	public override void OnLoad()
	{
		Main.worldSurface = Main.maxTilesY - 100;
		Main.rockLayer = Main.maxTilesY - 50;
	}

	public override void Load()
	{
		On_WorldGen.setWorldSize += WorldGen_setWorldSize;
	}

	private static void WorldGen_setWorldSize(On_WorldGen.orig_setWorldSize orig)
	{
		int fixedwidth = ((Main.maxTilesX - 1) / 200 + 1) * 200;
		int fixedheight = ((Main.maxTilesY - 1) / 150 + 1) * 150;
		if (fixedwidth + 1 > Main.tile.Width || fixedheight + 1 > Main.tile.Height)
		{
			var createmethod = typeof(Tilemap).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(ushort), typeof(ushort) });
			Main.tile = (Tilemap)createmethod.Invoke(new object[] { (ushort)(fixedwidth + 1), (ushort)(fixedheight + 1) });
			Main.Map = new(Main.maxTilesX, Main.maxTilesY);
			Main.mapMinX = 0;
			Main.mapMinY = 0;
			Main.mapMaxX = Main.maxTilesX;
			Main.mapMaxY = Main.maxTilesY;

			Main.instance.mapTarget = new RenderTarget2D[
				(Main.maxTilesX / Main.textureMaxWidth) + 1,
				(Main.maxTilesY / Main.textureMaxHeight) + 1];
			Main.mapWasContentLost = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];
			Main.initMap = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];

			Main.instance.TilePaintSystem = new();
			Main.instance.TilesRenderer = new(Main.instance.TilePaintSystem);
			Main.instance.WallsRenderer = new(Main.instance.TilePaintSystem);
		}
		Main.bottomWorld = Main.maxTilesY * 16;
		Main.rightWorld = Main.maxTilesX * 16;
		Main.maxSectionsX = (Main.maxTilesX - 1) / 200 + 1;
		Main.maxSectionsY = (Main.maxTilesY - 1) / 150 + 1;
	}
}

public class YggdrasilWorldSystem : ModSystem
{
	public override void PostUpdatePlayers()
	{
		ModContent.GetInstance<BiomeLabelSystem>().PlayerInVanillaWorld &= !YggdrasilWorld.InYggdrasil;
	}

	public override void PostUpdateEverything()
	{
		if (YggdrasilWorld.InYggdrasil)
		{
			YggdrasilWorld.YggdrasilTimer++;

			if (Main.bloodMoon)
			{
				Main.bloodMoon = false;
			}
			if(Main.slimeRain)
			{
				Main.slimeRain = false;
			}
		}
	}
}