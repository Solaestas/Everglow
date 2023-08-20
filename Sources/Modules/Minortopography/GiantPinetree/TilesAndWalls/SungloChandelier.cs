using Everglow.Commons.TileHelper;
using Everglow.Commons.Utilities;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class SungloChandelier : ModTile, ITileFluentlyDrawn, ITileFlameData
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		TileID.Sets.HasOutlines[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = DustID.BrownMoss;
		AdjTiles = new int[] { TileID.Chandeliers };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.addTile(Type);
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<Items.SungloChandelier>());
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 1.2f;
		g = 2.7f;
		b = 0f;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		TileFluentDrawManager.AddFluentPoint(this, i, j);
		return false;
	}
	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		FurnitureUtils.Chandelier3x3FluentDraw(screenPosition, pos, spriteBatch, tileDrawing);
	}

	public TileDrawing.TileFlameData GetTileFlameData(int tileX, int tileY, int type, int tileFrameY) =>
		new TileDrawing.TileFlameData()
		{
			flameCount = 1,
			flameTexture = ModAsset.SungloChandelier_glow.Value,
			flameRangeXMin = -1,
			flameRangeXMax = 1,
			flameRangeMultX = 0.15f,
			flameRangeYMin = -1,
			flameRangeYMax = 1,
			flameRangeMultY = 0.35f,
			flameColor = new Color(130, 130, 130, 0)
		};
}