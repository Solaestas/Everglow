using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
	public class FireflyMoss : ModTile
	{
		public override void PostSetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileMoss[Type] = true;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/FireflyMossGlow");

			spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(0.2f, 0.2f, 0.2f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

			base.PostDraw(i, j, spriteBatch);
		}
	}
}