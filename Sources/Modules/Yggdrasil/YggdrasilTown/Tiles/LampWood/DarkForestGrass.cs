using Everglow.Yggdrasil.Common.Elevator.Tiles;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class DarkForestGrass : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkForestSoil>()] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<StoneDragonScaleWoodDust>();
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(61, 29, 28));
	}
	public override void RandomUpdate(int i, int j)
	{
		switch (Main.rand.Next(5))
		{
			case 0:

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
				break;
			case 1:
				if (TileCollisionUtils.CanPlaceMultiAtTopTowardsUpRight(i, j, 5, 4))
				{
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 5, 4, ModContent.TileType<LampWood_Stone_5x4>(), 90 * Main.rand.Next(1), 0);
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
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 8, 6, ModContent.TileType<LampWood_Bone_8x6>(), 144 * Main.rand.Next(1), 0);
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
					YggdrasilWorldGeneration.PlaceFrameImportantTilesAbove(i, j, 5, 4, ModContent.TileType<LampWood_Stone_5x4>(), 90 * Main.rand.Next(1), 0);
				}
				break;
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
