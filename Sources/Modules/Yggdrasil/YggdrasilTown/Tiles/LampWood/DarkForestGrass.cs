using Everglow.Yggdrasil.Common.Elevator.Tiles;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class DarkForestGrass : ModTile
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
	public override void RandomUpdate(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		int blockCount = 0;
		for (int x = -1; x < 2; x++)
		{
			for (int y = -1; y < 2; y++)
			{
				Tile checkTile = Main.tile[i + x, j + y];
				if (checkTile.HasTile && (checkTile.TileType == ModContent.TileType<DarkForestSoil>() || checkTile.TileType == Type))
				{
					blockCount++;
				}
			}
		}
		if (blockCount >= 9)
		{
			tile.TileType = (ushort)(ModContent.TileType<DarkForestSoil>());
			return;
		}
		switch (Main.rand.Next(14))
		{
			case 0:
				PlaceTree(i, j);
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
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 4, 1, ModContent.TileType<LampWood_Stone_4x1>(), 0, 0);
				}
				break;
			case 5:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 5, 4))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 5, 4, ModContent.TileType<LampWood_Stone_5x4>(), 90 * Main.rand.Next(2), 0);
				}
				break;
			case 6:
				if(TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 1, 2))
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
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 1, 3, ModContent.TileType<DarkTaro>(), 0);
				}
				break;
			case 11:
				PlaceTree(i, j);
				break;
			case 12:
				PlaceTree(i, j);
				break;
			case 13:
				PlaceTree(i, j);
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
}
