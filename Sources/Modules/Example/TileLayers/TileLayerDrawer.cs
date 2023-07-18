using Terraria.GameContent;

namespace Everglow.Example.TileLayers;
public class TileLayerDrawer : GlobalTile
{
	
	public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
	{
		base.PostDraw(i, j, type, spriteBatch);
		Tile tile = Main.tile[i, j];
		if(TileLayerSystem.SwitchingTiles.Contains(tile))
		{
			Texture2D black = TextureAssets.MagicPixel.Value;
			int x = (int)(i * 16 - Main.screenPosition.X);
			int y = (int)(j * 16 - Main.screenPosition.Y);
			spriteBatch.Draw(black, new Rectangle(x, y, 16, 16), new Color(0, 0, 0, (float)(TileLayerSystem.SwitchingTimer / 60f)));
		}
	}
}
