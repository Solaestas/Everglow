using Everglow.Myth.Common;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

public class BoCPostAndBead : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 9;
		TileObjectData.newTile.Width = 3;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			18
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(87, 84, 96));
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/BoCPostAndBead_glow");

		spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

		base.PostDraw(i, j, spriteBatch);
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}
}