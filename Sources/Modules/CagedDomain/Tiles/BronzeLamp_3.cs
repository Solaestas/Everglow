using Everglow.CagedDomain.Items;
using Everglow.Commons.TileHelper;
using Everglow.Commons.Utilities;
using Terraria.ObjectData;

namespace Everglow.CagedDomain.Tiles;
public class BronzeLamp_3 : ShapeDataTile
{
	public override void SetStaticDefaults()
	{
		CustomItemType = ModContent.ItemType<BronzeLamp_3_Item>();
		TotalWidth = 7;
		TotalHeight = 17;

		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = true;

		AdjTiles = new int[] { TileID.Lamps };

		TileObjectData.newSubTile.Width = 7;
		TileObjectData.newSubTile.Height = 17;
		TileObjectData.newSubTile.UsesCustomCanPlace = true;
		TileObjectData.newSubTile.StyleHorizontal = true;
		TileObjectData.newSubTile.CoordinatePadding = 2;
		TileObjectData.newSubTile.CoordinateWidth = 16;
		TileObjectData.newSubTile.CoordinateHeights = new int[17];
		Array.Fill(TileObjectData.newSubTile.CoordinateHeights, 16);
		TileObjectData.newSubTile.CoordinateHeights[^1] = 20;
		TileObjectData.newSubTile.LavaDeath = false;
		TileObjectData.newSubTile.Origin = new(3, 16);
		TileObjectData.newSubTile.AnchorBottom = new(Terraria.Enums.AnchorType.SolidTile, 3, 2);
		TileObjectData.addSubTile(1);

		TileObjectData.newTile.Width = 7;
		TileObjectData.newTile.Height = 17;
		TileObjectData.newTile.UsesCustomCanPlace = true;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinatePadding = 2;
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.CoordinateHeights = new int[17];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.CoordinateHeights[^1] = 20;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new(3, 16);
		TileObjectData.newTile.AnchorBottom = new(Terraria.Enums.AnchorType.SolidTile, 3, 2);
		TileObjectData.addTile(Type);

		AddMapEntry(Color.Gold);
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX == 36)
		{
			if (tile.TileFrameY == 36)
			{
				r = 1;
				g = 0.5f;
				b = 0.2f;
			}
		}
		else if (tile.TileFrameX == 0 || tile.TileFrameX == 72)
		{
			if (tile.TileFrameY == 72)
			{
				r = 1;
				g = 0.5f;
				b = 0.2f;
			}
		}
		else
		{
			r = g = b = 0;
		}
		r *= 4;
		g *= 4;
		b *= 4;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 7, 17);
	}
}