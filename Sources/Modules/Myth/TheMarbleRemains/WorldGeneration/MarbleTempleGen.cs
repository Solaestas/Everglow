using Everglow.Myth.Common;
using Terraria.ModLoader;

namespace Everglow.Myth.TheMarbleRemains.WorldGeneration
{
	public class MarbleTempleGen : ModSystem
	{
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
		//public override void PostWorldGen()
		//{
		//	//NPC.NewNPC(Main.maxTilesX * 8, 200, ModContent.NPCType("Yasitaya"), 0, 0f, 0f, 0f, 0f, 255);
		//	int Xd = 2000;
		//	int Yd = 600;
		//	if (Main.maxTilesX == 6400)
		//	{
		//		Xd = 3000;
		//		Yd = 900;
		//	}
		//	if (Main.maxTilesX == 8400)
		//	{
		//		Xd = 4000;
		//		Yd = 1200;
		//	}
		//	Texture2D tex = MythContent.QuickTexture("TheMarbleRemains/WorldGeneration/MarbleTempleKill");
		//	Color[] colorTex = new Color[tex.Width * tex.Height];
		//	tex.GetData(colorTex);

		//	for (int y = 0; y < tex.Height; y += 1)
		//	{
		//		for (int x = 0; x < tex.Width; x += 1)
		//		{
		//			if (new Color(colorTex[x + y * tex.Width].R, colorTex[x + y * tex.Width].G, colorTex[x + y * tex.Width].B) == new Color(255, 0, 0))
		//			{
		//				//WorldGen.PlaceTile(x + 2000, y + 100, mod.TileType("朽木"));
		//				Main.tile[x + Xd, y + Yd].ClearEverything();
		//			}
		//		}
		//	}
		//	Texture2D tex1 = MythContent.QuickTexture("TheMarbleRemains/WorldGeneration/MarbleTempleWall");
		//	Color[] colorTex1 = new Color[tex1.Width * tex1.Height];
		//	tex1.GetData(colorTex);

		//	for (int y = 0; y < tex1.Height; y += 1)
		//	{
		//		for (int x = 0; x < tex1.Width; x += 1)
		//		{
		//			if (new Color(colorTex[x + y * tex1.Width].R, colorTex[x + y * tex1.Width].G, colorTex[x + y * tex1.Width].B) == new Color(111, 117, 135))
		//			{
		//				Main.tile[x + Xd, y + Yd].wall = 179;
		//				Main.tile[x + Xd, y + Yd].active(false);
		//			}
		//		}
		//	}
		//	Texture2D tex2 = MythContent.QuickTexture("TheMarbleRemains/WorldGeneration/MarbleTempleTile");
		//	Color[] colortex2 = new Color[tex2.Width * tex2.Height];
		//	tex2.GetData(colorTex);

		//	for (int y = 0; y < tex2.Height; y += 1)
		//	{
		//		for (int x = 0; x < tex2.Width; x += 1)
		//		{
		//			if (new Color(colorTex[x + y * tex2.Width].R, colorTex[x + y * tex2.Width].G, colorTex[x + y * tex2.Width].B) == new Color(168, 178, 204))
		//			{
		//				Main.tile[x + Xd, y + Yd].type = 357;
		//				Main.tile[x + Xd, y + Yd].active(true);
		//			}
		//			if (new Color(colorTex[x + y * tex2.Width].R, colorTex[x + y * tex2.Width].G, colorTex[x + y * tex2.Width].B) == new Color(63, 89, 255))
		//			{
		//				Main.tile[x + Xd, y + Yd].type = (ushort)ModContent.TileType<Tiles.GiantMarbalClock>();
		//				Main.tile[x + Xd, y + Yd].active(true);
		//			}
		//			if (new Color(colorTex[x + y * tex2.Width].R, colorTex[x + y * tex2.Width].G, colorTex[x + y * tex2.Width].B) == new Color(226, 109, 140))
		//			{
		//				Main.tile[x + Xd, y + Yd].type = 19;
		//				Main.tile[x + Xd, y + Yd].active(true);
		//				Main.tile[x + Xd, y + Yd].frameY = 522;
		//				WorldGen.SlopeTile(x + Xd, y + Yd, 1);
		//			}
		//			if (new Color(colorTex[x + y * tex2.Width].R, colorTex[x + y * tex2.Width].G, colorTex[x + y * tex2.Width].B) == new Color(226, 109, 70))
		//			{
		//				Main.tile[x + Xd, y + Yd].type = 19;
		//				Main.tile[x + Xd, y + Yd].active(true);
		//				Main.tile[x + Xd, y + Yd].frameY = 522;
		//				WorldGen.SlopeTile(x + Xd, y + Yd, 2);
		//			}
		//			if (new Color(colorTex[x + y * tex2.Width].R, colorTex[x + y * tex2.Width].G, colorTex[x + y * tex2.Width].B) == new Color(204, 99, 127))
		//			{
		//				Main.tile[x + Xd, y + Yd].type = 19;
		//				Main.tile[x + Xd, y + Yd].active(true);
		//				Main.tile[x + Xd, y + Yd].frameY = 522;
		//			}
		//		}
		//	}
		//	for (int y = 0; y < tex2.Height; y += 1)
		//	{
		//		for (int x = 0; x < tex2.Width; x += 1)
		//		{
		//			if (new Color(colorTex[x + y * tex2.Width].R, colorTex[x + y * tex2.Width].G, colorTex[x + y * tex2.Width].B) == new Color(255, 13, 5))
		//			{
		//				WorldGen.PlaceTile(x + Xd, y + Yd, ModContent.TileType<Tiles.MarbleFragment>(), false, true, -1, 0);
		//			}
		//			if (new Color(colorTex[x + y * tex2.Width].R, colorTex[x + y * tex2.Width].G, colorTex[x + y * tex2.Width].B) == new Color(255, 162, 22))
		//			{
		//				WorldGen.PlaceTile(x + Xd, y + Yd, 34, false, true, -1, 37);
		//				Main.tile[x + Xd - 1, y + Yd - 1].frameX += 54;
		//				Main.tile[x + Xd, y + Yd - 1].frameX += 54;
		//				Main.tile[x + Xd + 1, y + Yd - 1].frameX += 54;
		//				Main.tile[x + Xd - 1, y + Yd].frameX += 54;
		//				Main.tile[x + Xd, y + Yd].frameX += 54;
		//				Main.tile[x + Xd + 1, y + Yd].frameX += 54;
		//				Main.tile[x + Xd - 1, y + Yd + 1].frameX += 54;
		//				Main.tile[x + Xd, y + Yd + 1].frameX += 54;
		//				Main.tile[x + Xd + 1, y + Yd + 1].frameX += 54;
		//			}
		//		}
		//	}
		//	Texture2D tex3 = MythContent.QuickTexture("TheMarbleRemains/WorldGeneration/MarbleTempleLiquid");
		//	Color[] colortex3 = new Color[tex3.Width * tex3.Height];
		//	tex3.GetData(colorTex);

		//	for (int y = 0; y < tex3.Height; y += 1)
		//	{
		//		for (int x = 0; x < tex3.Width; x += 1)
		//		{
		//			if (new Color(colorTex[x + y * tex3.Width].R, colorTex[x + y * tex3.Width].G, colorTex[x + y * tex3.Width].B) == new Color(0, 13, 204))
		//			{
		//				Main.tile[x + Xd, y + Yd].liquid = byte.MaxValue;
		//			}
		//		}
		//	}
		//}
	}
}