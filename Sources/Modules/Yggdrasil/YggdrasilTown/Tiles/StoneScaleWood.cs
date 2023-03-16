using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class StoneScaleWood : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreTile>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineStone>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreSmallUp>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreSmall>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreLargeUp>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreLarge>()] = true;
		Main.tileMerge[Type][(ushort)ModContent.TileType<CyanVineOreMiddle>()] = true;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		Main.tileShine2[Type] = false;

		Main.ugBackTransition = 1000;
		DustType = DustID.BorealWood;
		MinPick = 150;
		HitSound = SoundID.Dig;

		ItemDrop = ModContent.ItemType<Items.StoneDragonScaleWood>();

		AddMapEntry(new Color(44, 40, 37));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
	}
	public override void RandomUpdate(int i, int j)
	{
		int CountNoneA = 0;
		int CountA = 0;
		for (int x = -1; x < 2; x++)
		{
			for (int y = -4; y < 0; y++)
			{
				if (Main.tile[i + x, j + y].HasTile)
					CountNoneA++;
			}
		}
		for (int x = -1; x < 2; x++)
		{
			if (Main.tile[i + x, j].HasTile)
				CountA++;
		}
		if (!Main.tile[i - 5, j - 1].HasTile && !Main.tile[i + 5, j - 1].HasTile && !Main.tile[i - 2, j - 1].HasTile && !Main.tile[i + 2, j - 1].HasTile && !Main.tile[i + 3, j - 1].HasTile && !Main.tile[i - 3, j - 1].HasTile && !Main.tile[i - 4, j - 1].HasTile && !Main.tile[i + 4, j - 1].HasTile)
		{
			if (Main.rand.NextBool(8))
			{
				if (CountNoneA == 0)
				{
					if (CountA == 3)
					{
						//int Dy = Main.rand.Next(5);
						//WorldGen.Place3x4(i, j - 1, (ushort)ModContent.TileType<OceanMod.Tiles.Tree1.CyanVineOre>(), 0);
						//CombatText.NewText(new Rectangle(i * 16, j * 16, 16, 16), Color.Cyan, Dy);
					}
				}
			}
		}
	}
}
