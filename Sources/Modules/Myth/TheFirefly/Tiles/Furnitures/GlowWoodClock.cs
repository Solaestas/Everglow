using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodClock : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.Clock[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		DustType = ModContent.DustType<BlueGlow>();
		AdjTiles = new int[] { TileID.GrandfatherClocks };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 18 };
		TileObjectData.addTile(Type);
	}

	public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
	{
		return true;
	}

	public override bool RightClick(int x, int y)
	{
		return FurnitureUtils.ClockRightClick();
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<Items.Furnitures.GlowWoodClock>());
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodClockGlow");
		spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(0.8f, 0.8f, 0.8f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

		base.PostDraw(i, j, spriteBatch);
	}
}