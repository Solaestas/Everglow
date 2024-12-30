namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class FurnaceLavaband : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tilePile[Type] = true;
		Main.tileMergeDirt[Type] = false;
		Main.tileNoAttach[Type] = true;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = false;
		Main.tileLighted[Type] = true;
		DustType = DustID.Lava;
		HitSound = default;

		AddMapEntry(new Color(247, 70, 0));
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		spriteBatch.Draw(texture, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 0.3f, 0.1f, 0), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.4f;
		g = 0.07f;
		b = 0.01f;
		base.ModifyLight(i, j, ref r, ref g, ref b);
	}
}