using Everglow.Commons.TileHelper;
using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

public class TwilightBlueCrystal_0 : ShapeDataTile, ISceneTile
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		Main.tileLighted[Type] = true;
		AddMapEntry(new Color(40, 80, 148));
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.1f;
		g = 0.42f;
		b = 0.42f;
		base.ModifyLight(i, j, ref r, ref g, ref b);
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Color lightColor = Lighting.GetColor(i, j);
		Tile tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;
		spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), lightColor * 1.5f, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}
	public void AddScene(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX == 72 && tile.TileFrameY == 0)
		{
			TwilightBlueCrystal_0_Mirror mirror = new TwilightBlueCrystal_0_Mirror { position = new Vector2(i, j) * 16 + new Vector2(-64, 6), Active = true, Visible = true, originTile = new Point(i, j), originType = ModContent.TileType<TwilightBlueCrystal_0>() };
			Ins.VFXManager.Add(mirror);
		}
	}
}