using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class LampWood_newStyleTree_1 : ShapeDataTile
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		Main.tileAxe[Type] = true;
		AddMapEntry(new Color(49, 41, 96));
	}
	public override void PostSetDefaults()
	{
		base.PostSetDefaults();
		MultiItem = true;
		CustomItemType = ModContent.ItemType<LampWood_Wood>();
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return base.PreDraw(i, j, spriteBatch);
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;
		spriteBatch.Draw(ModAsset.LampWood_newStyleTree_glow.Value, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}
}