using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;
using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

public class TwilightGrassBlock : ModTile, ISceneTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkForestSoil>()] = true;
		Main.tileMerge[Type][ModContent.TileType<StoneScaleWood>()] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<DarkForestSoil_Dust>();
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(39, 155, 170));
	}
	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		if (VFXManager.InScreen(new Vector2(i, j) * 16, 200))
		{
			Grass_FurPipeline.ShouldUpdateRenderTarget = true;
		}
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}
	public override void RandomUpdate(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		int blockCount = 0;
		for (int x = -1; x < 2; x++)
		{
			for (int y = -1; y < 2; y++)
			{
				Tile checkTile = Main.tile[i + x, j + y];
				if (checkTile.HasTile && !Main.tileFrameImportant[checkTile.TileType])
				{
					blockCount++;
				}
			}
		}
		if (blockCount >= 9)
		{
			tile.TileType = (ushort)(ModContent.TileType<DarkForestSoil>());
		}
		for(int z = 0; z < 20; z++)
		{
			blockCount = 0;
			int spreadX = Main.rand.Next(-5, 6);
			int spreadY = Main.rand.Next(-5, 6);
			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					Tile checkTile = YggdrasilWorldGeneration.SafeGetTile(i + x + spreadX, j + y + spreadY);
					if (checkTile.HasTile && (checkTile.TileType == ModContent.TileType<DarkForestSoil>() || checkTile.TileType == Type))
					{
						blockCount++;
					}
					if (checkTile.HasTile && Main.tileFrameImportant[checkTile.TileType])
					{
						blockCount += 2;
					}
					if (!checkTile.HasTile)
					{
						blockCount += 3;
					}
				}
			}
			if (Main.rand.Next(9, 18) < blockCount)
			{
				Tile checkTile = YggdrasilWorldGeneration.SafeGetTile(i + spreadX, j + spreadY);
				if (checkTile.TileType == ModContent.TileType<DarkForestSoil>())
				{
					checkTile.TileType = Type;
				}
			}
		}
		int height = Main.rand.Next(14, 60);
		switch (Main.rand.Next(18))
		{
			case 0:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i - 4, j, 8, height))
				{
					PlaceTree(i, j - 1, height);
				}
				break;
			case 1:
				switch (Main.rand.Next(4))
				{
					case 0:
						if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 2, 2))
						{
							YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 2, 2, ModContent.TileType<TwilightBlueCrystal_5>(), 36 * Main.rand.Next(4), 0);
						}
						break;
					case 1:
						if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 2))
						{
							YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 2, ModContent.TileType<TwilightBlueCrystal_4>(), 54 * Main.rand.Next(2), 0);
						}
						break;
					case 2:
						if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 1))
						{
							YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 1, ModContent.TileType<TwilightBlueCrystal_3>(), 54 * Main.rand.Next(2), 0);
						}
						break;
					case 3:
						if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 3))
						{
							YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 3, ModContent.TileType<TwilightBlueCrystal_2>(), 54 * Main.rand.Next(2), 0);
						}
						break;
				}
				break;
			case 2:
				switch (Main.rand.Next(2))
				{
					case 0:
						if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 9, 7))
						{
							TwilightBlueCrystal_0 tBC = TileLoader.GetTile(ModContent.TileType<TwilightBlueCrystal_0>()) as TwilightBlueCrystal_0;
							tBC.PlaceOriginAtBottomLeft(i, j - 1);
						}
						break;
					case 1:
						if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 8, 7))
						{
							TwilightBlueCrystal_1 tBC = TileLoader.GetTile(ModContent.TileType<TwilightBlueCrystal_1>()) as TwilightBlueCrystal_1;
							tBC.PlaceOriginAtBottomLeft(i, j - 1);
						}
						break;
				}
				break;
			case 3:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i - 4, j, 8, height))
				{
					PlaceTree(i, j - 1, height);
				}
				break;
			case 4:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i - 4, j, 8, height))
				{
					PlaceTree(i, j - 1, height);
				}
				break;
			case 5:
				if (Main.rand.NextBool(6))
				{
					if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 4, 3))
					{
						YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 4, 3, ModContent.TileType<TwilightBlueCrystalFlower>(), 72 * Main.rand.Next(2), 0);
					}
				}
				break;
			case 6:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 1, ModContent.TileType<Twilight_Grass>(), 18 * Main.rand.Next(6));
				}
				break;
			case 7:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 1, ModContent.TileType<Twilight_Grass>(), 18 * Main.rand.Next(6));
				}
				break;
			case 8:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 5, 3))
				{
					TwilightBlueCrystal_6 tBC = TileLoader.GetTile(ModContent.TileType<TwilightBlueCrystal_6>()) as TwilightBlueCrystal_6;
					tBC.PlaceOriginAtBottomLeft(i, j - 1);
				}
				break;
			case 9:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 5, 3))
				{
					TwilightBlueCrystal_6 tBC = TileLoader.GetTile(ModContent.TileType<TwilightBlueCrystal_6>()) as TwilightBlueCrystal_6;
					tBC.PlaceOriginAtBottomLeft(i, j - 1);
				}
				break;
			case 10:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 8, 4))
				{
					Twilight_Stone_8x4 twilight_Stone_8x4 = TileLoader.GetTile(ModContent.TileType<Twilight_Stone_8x4>()) as Twilight_Stone_8x4;
					twilight_Stone_8x4.PlaceOriginAtBottomLeft(i, j - 1);
				}
				break;
			case 11:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 6, 5))
				{
					TwilightStone_0 t0 = TileLoader.GetTile(ModContent.TileType<TwilightStone_0>()) as TwilightStone_0;
					t0.PlaceOriginAtBottomLeft(i, j - 1);
				}
				break;
			case 12:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 4, 3))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 4, 3, ModContent.TileType<TwilightStone_2>(), 0);
				}
				break;
			case 13:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 5, 3))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 5, 3, ModContent.TileType<TwilightStone_1>(), 90 * Main.rand.Next(4));
				}
				break;
			case 14:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 2, ModContent.TileType<BrokenBoxInTwilight>(), 54 * Main.rand.Next(2));
				}
				break;
			case 15:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 4, 1))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 4, 1, ModContent.TileType<Twilight_Stone_4x1>(), 72 * Main.rand.Next(2));
				}
				break;
			case 16:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 6, 1))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 6, 1, ModContent.TileType<Twilight_Stone_6x1>(), 0);
				}
				break;
			case 17:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 6, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 6, 2, ModContent.TileType<Twilight_Stone_6x2>(), 108 * Main.rand.Next(2));
				}
				break;
		}
	}
	public void PlaceTree(int i, int j, int height)
	{
		TreePlacer.BuildTwilightTree(i, j, height);
	}
	public void AddScene(int i, int j)
	{
		TwilightGrass_grass_fore leaf = new TwilightGrass_grass_fore { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
		leaf.scale = 1f;
		leaf.style = i % 3;
		Ins.VFXManager.Add(leaf);
	}
}