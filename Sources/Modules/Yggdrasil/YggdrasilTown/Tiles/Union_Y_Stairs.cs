using Everglow.Commons.TileHelper;
using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class Union_Y_Stairs : ShapeDataTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		CustomItemType = ModContent.ItemType<Union_Y_Stairs_Item>();
		DustType = ModContent.DustType<UnionMarblePost_Dust_Khaki>();
		TotalWidth = 38;
		TotalHeight = 20;
		MinPick = 300;
		Main.tileSolid[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = false;
		Main.tileBlendAll[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Width = 38;
		TileObjectData.newTile.Height = 20;
		TileObjectData.newTile.UsesCustomCanPlace = true;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinatePadding = 2;
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.CoordinateHeights = new int[20];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new(19, 19);
		TileObjectData.newTile.AnchorTop = new AnchorData(0, 0, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(0, 0, 0);
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(194, 165, 134));
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		resetFrame = false;
		return false;
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		if (fail)
		{
			return;
		}
		var thisTile = Main.tile[i, j];
		int x0 = i - thisTile.TileFrameX / 18;
		int y0 = j - thisTile.TileFrameY / 18;
		for (int x = 0; x < TotalWidth; x++)
		{
			for (int y = 0; y < TotalHeight; y++)
			{
				var tile = Main.tile[x0 + x, y0 + y];
				if (tile.TileFrameX == x * 18 && tile.TileFrameY == y * 18)
				{
					if (tile.TileType == Type && PixelHasTile[x, y] >= 200)
					{
						for (int a = 0; a < 3; a++)
						{
							Dust dust = Dust.NewDustDirect(new Vector2(x0 + x, y0 + y) * 16, 16, 16, DustType, 0, 0, 0, default, 1);
						}
					}
				}
			}
		}
		if (!MultiItem)
		{
			CustomDropItem(i, j);
		}
		SoundEngine.PlaySound(HitSound, new Vector2(i * 16, j * 16));
	}

	public void AddScene(int i, int j)
	{
		Tile tile = YggdrasilWorldGeneration.SafeGetTile(i, j);
		if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
		{
			Union_Y_Stairs_Front uYSF = new Union_Y_Stairs_Front { position = new Vector2(i, j) * 16 + new Vector2(0, -70), Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
			Ins.VFXManager.Add(uYSF);
			Union_Y_Stairs_Back uYSB = new Union_Y_Stairs_Back { position = new Vector2(i, j) * 16 + new Vector2(0, -70), Active = true, Visible = true, originTile = new Point(i, j), originType = Type };
			Ins.VFXManager.Add(uYSB);
		}
	}

	public override void PlaceOriginAtTopLeft(int x, int y)
	{
		if (x > Main.maxTilesX - TotalWidth || x < 0 || y > Main.maxTilesY - TotalHeight || y < 0)
		{
			return;
		}

		for (int i = 0; i < TotalWidth; i++)
		{
			for (int j = 0; j < TotalHeight; j++)
			{
				if (PixelHasTile[i, j] >= 200)
				{
					Tile tile = Main.tile[x + i, y + j];

					tile.TileFrameX = (short)(i * 18);
					tile.TileFrameY = (short)(j * 18);

					if ((tile.TileFrameX is >= 252 and < 432) && (tile.TileFrameY is >= 288 and < 360))
					{
						tile.TileType = (ushort)ModContent.TileType<Union_Y_Platform>();
						if (tile.TileFrameX is >= 144 and < 198)
						{
							tile.TileType = (ushort)ModContent.TileType<Union_Y_PlatformRed>();
						}
						tile.HasTile = true;
					}
					else if ((tile.TileFrameX is >= 270 and < 414) && (tile.TileFrameY is >= 216 and < 288))
					{
						tile.TileType = (ushort)ModContent.TileType<Union_Y_Platform>();
						if (tile.TileFrameX is >= 144 and < 198)
						{
							tile.TileType = (ushort)ModContent.TileType<Union_Y_PlatformRed>();
						}
						tile.HasTile = true;
					}
					else
					{
						tile.TileType = Type;
						tile.HasTile = true;
					}
				}
			}
		}
	}
}