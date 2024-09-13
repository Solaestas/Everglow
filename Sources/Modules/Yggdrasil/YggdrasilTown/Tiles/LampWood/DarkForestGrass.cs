using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class DarkForestGrass : ModTile, ISceneTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkForestSoil>()] = true;
		Main.tileMerge[Type][ModContent.TileType<StoneScaleWood>()] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<DarkForestSoil_Dust>();
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(71, 71, 145));
	}
	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		if(VFXManager.InScreen(new Vector2(i,j) * 16, 200))
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
				Tile checkTile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
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
		for (int z = 0; z < 20; z++)
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
		switch (Main.rand.Next(27))
		{
			case 0:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 6, 1))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 6, 1, ModContent.TileType<LampWood_Stone_6x1>(), 0, 0);
				}
				break;
			case 1:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 5, 4))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 5, 4, ModContent.TileType<LampWood_Stone_5x4>(), 90 * Main.rand.Next(2), 0);
				}
				break;
			case 2:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 2, ModContent.TileType<LampWood_Stump_3x2>(), 0, 0);
				}
				break;
			case 3:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 8, 6))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 8, 6, ModContent.TileType<LampWood_Bone_8x6>(), 144 * Main.rand.Next(2), 0);
				}
				break;
			case 4:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 4, 1))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 4, 1, ModContent.TileType<LampWood_Stone_4x1>(), 72 * Main.rand.Next(2), 0);
				}
				break;
			case 5:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 5, 4))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 5, 4, ModContent.TileType<LampWood_Stone_5x4>(), 90 * Main.rand.Next(2), 0);
				}
				break;
			case 6:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 1, ModContent.TileType<LampWood_Grass>(), 18 * Main.rand.Next(6));
				}
				break;
			case 7:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 1, ModContent.TileType<LampWood_Grass>(), 18 * Main.rand.Next(6));
				}
				break;
			case 8:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 1, ModContent.TileType<LampWood_Grass>(), 18 * Main.rand.Next(6));
				}
				break;
			case 9:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 1, ModContent.TileType<LampWood_Grass>(), 18 * Main.rand.Next(6));
				}
				break;
			case 10:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 3))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 3, ModContent.TileType<DarkTaro>(), 72 * Main.rand.Next(3));
				}
				break;
			case 11:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 8, 13))
				{
					LampWood_newStyleTree_0 lampWood_NewStyleTree_0 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_0>()) as LampWood_newStyleTree_0;
					lampWood_NewStyleTree_0.PlaceOriginAtBottomLeft(i - 5, j - 1);
				}
				break;
			case 12:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 8, 13))
				{
					LampWood_newStyleTree_0 lampWood_NewStyleTree_0 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_0>()) as LampWood_newStyleTree_0;
					lampWood_NewStyleTree_0.PlaceOriginAtBottomLeft(i - 5, j - 1);
				}
				break;
			case 13:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 8, 13))
				{
					LampWood_newStyleTree_0 lampWood_NewStyleTree_0 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_0>()) as LampWood_newStyleTree_0;
					lampWood_NewStyleTree_0.PlaceOriginAtBottomLeft(i - 5, j - 1);
				}
				break;
			case 14:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 9, 16))
				{
					LampWood_newStyleTree_1 lampWood_NewStyleTree_1 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_1>()) as LampWood_newStyleTree_1;
					lampWood_NewStyleTree_1.PlaceOriginAtBottomLeft(i - 4, j - 1);
				}
				break;
			case 15:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 9, 16))
				{
					LampWood_newStyleTree_1 lampWood_NewStyleTree_1 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_1>()) as LampWood_newStyleTree_1;
					lampWood_NewStyleTree_1.PlaceOriginAtBottomLeft(i - 4, j - 1);
				}
				break;
			case 16:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 9, 16))
				{
					LampWood_newStyleTree_1 lampWood_NewStyleTree_1 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_1>()) as LampWood_newStyleTree_1;
					lampWood_NewStyleTree_1.PlaceOriginAtBottomLeft(i - 4, j - 1);
				}
				break;
			case 17:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 9, 16))
				{
					LampWood_newStyleTree_1 lampWood_NewStyleTree_1 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_1>()) as LampWood_newStyleTree_1;
					lampWood_NewStyleTree_1.PlaceOriginAtBottomLeft(i - 4, j - 1);
				}
				break;
			case 18:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 9, 16))
				{
					LampWood_newStyleTree_1 lampWood_NewStyleTree_1 = TileLoader.GetTile(ModContent.TileType<LampWood_newStyleTree_1>()) as LampWood_newStyleTree_1;
					lampWood_NewStyleTree_1.PlaceOriginAtBottomLeft(i - 4, j - 1);
				}
				break;
			case 19:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 6, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 6, 2, ModContent.TileType<LampWood_Stone_6x2>(), 108 * Main.rand.Next(4), 0);
				}
				break;
			case 20:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 8, 4))
				{
					LampWood_Stone_8x4 lampWood_Stone_8x4 = TileLoader.GetTile(ModContent.TileType<LampWood_Stone_8x4>()) as LampWood_Stone_8x4;
					lampWood_Stone_8x4.PlaceOriginAtBottomLeft(i, j - 1);
				}
				break;
			case 21:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 6))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 6, ModContent.TileType<LampWood_Stone_3x6>());
				}
				break;
			case 22:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 2, ModContent.TileType<BrokenBoxInLampwood>(), 54 * Main.rand.Next(2));
				}
				break;
			case 23:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 3, 2))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 3, 2, ModContent.TileType<BrokenBox>(), 54 * Main.rand.Next(2));
				}
				break;
			case 24:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 4, 6))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 4, 6, ModContent.TileType<WaveLeafFlower4x6>(), 72 * Main.rand.Next(3));
				}
				break;
			case 25:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 4, 7))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 4, 7, ModContent.TileType<WaveLeafFlower4x7>());
				}
				break;
			case 26:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 6, 8))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 6, 8, ModContent.TileType<WaveLeafFlower6x8>());
				}
				break;
		}
	}
	public void PlaceTree(int i, int j)
	{
		if (Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 2, j].Slope == SlopeType.Solid && Main.tile[i - 2, j].Slope == SlopeType.Solid &&
						Main.tile[i, j + 1].Slope == SlopeType.Solid && Main.tile[i + 1, j + 1].Slope == SlopeType.Solid && Main.tile[i - 1, j + 1].Slope == SlopeType.Solid && Main.tile[i + 2, j + 1].Slope == SlopeType.Solid && Main.tile[i - 2, j + 1].Slope == SlopeType.Solid)//树木
		{
			int MaxHeight = 0;
			for (int x = -2; x < 3; x++)
			{
				for (int y = -1; y > -24; y--)
				{
					if (j + y > 20)
					{
						if (Main.tile[i + x, j + y].HasTile || Main.tile[i + x, j + y].LiquidAmount > 3)
							return;
					}
					MaxHeight = -y;
				}
			}
			if (MaxHeight > 3)
				BuildLampTree(i, j - 1, MaxHeight);
		}
	}
	public static void BuildLampTree(int i, int j, int height = 0)
	{
		if (j < 30)
			return;
		int trueHeight = Main.rand.Next(3, height);

		for (int g = 0; g < trueHeight; g++)
		{
			Tile tile = Main.tile[i, j - g];
			if (g == 0)
			{
				tile.TileType = (ushort)ModContent.TileType<LampWood_Tree>();
				tile.TileFrameY = 0;
				tile.TileFrameX = (short)Main.rand.Next(0);
				tile.HasTile = true;
				continue;
			}
			if (g == trueHeight - 1)
			{
				tile.TileType = (ushort)ModContent.TileType<LampWood_Tree>();
				tile.TileFrameY = 2;
				tile.TileFrameX = (short)Main.rand.Next(0);
				tile.HasTile = true;
				continue;
			}
			if (Main.rand.NextBool(6) && g <= trueHeight - 5)
			{
				tile.TileType = (ushort)ModContent.TileType<LampWood_Tree>();
				tile.TileFrameY = 3;
				tile.TileFrameX = (short)Main.rand.Next(0);
				tile.HasTile = true;
				tile = Main.tile[i, j - g - 1];
				tile.TileType = (ushort)ModContent.TileType<LampWood_Tree>();
				tile.TileFrameY = -1;
				tile.HasTile = true;
				tile = Main.tile[i, j - g - 2];
				tile.TileType = (ushort)ModContent.TileType<LampWood_Tree>();
				tile.TileFrameY = -1;
				tile.HasTile = true;
				g += 2;
				continue;
			}
			tile.TileType = (ushort)ModContent.TileType<LampWood_Tree>();
			tile.TileFrameY = 1;
			tile.TileFrameX = (short)Main.rand.Next(3);
			tile.HasTile = true;
		}
	}
	public void AddScene(int i, int j)
	{
		DarkForestGrass_grass_fore leaf = new DarkForestGrass_grass_fore { position = new Vector2(i, j) * 16, Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
		leaf.scale = 1f;
		leaf.style = i % 3;
		Ins.VFXManager.Add(leaf);
	}
}
