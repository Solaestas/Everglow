using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class HeatProofPlatingWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<FurnacePlatingWallDust>();
		AddMapEntry(new Color(82, 54, 43));
	}

	public override bool WallFrame(int i, int j, bool randomizeFrame, ref int style, ref int frameNumber)
	{
		return base.WallFrame(i, j, randomizeFrame, ref style, ref frameNumber);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var offsetScreen = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
		{
			offsetScreen = Vector2.Zero;
		}
		Vector2 drawPos = new Point(i, j).ToWorldCoordinates() - Main.screenPosition + offsetScreen;
		Rectangle frame = new Rectangle(2 + (i % 15) * 16 - 4, 206 + (j % 17) * 16 - 4, 24, 24);
		spriteBatch.Draw(texture, drawPos, frame, Lighting.GetColor(i, j), 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);
	}
}