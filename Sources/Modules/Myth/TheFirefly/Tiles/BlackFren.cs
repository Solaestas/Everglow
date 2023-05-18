using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

public class BlackFren : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			18
		};
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.addTile(Type);
		DustType = 191;
		AddMapEntry(new Color(11, 11, 11));
		HitSound = SoundID.Grass;
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Texture2D tex = ModAsset.BlackFrenGlow.Value;
		spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(0.8f, 0.8f, 0.8f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);
		base.PostDraw(i, j, spriteBatch);
	}
}