namespace Everglow.Commons.BiomesText;

public class BiomeLable : ModSystem
{
	public Texture2D Lable => ModAsset.Textboard.Value;
	public int Width => 324;
	public int Height => 100;
	public int Frame = -1;
	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		Frame++;
		if(Frame > 79)
		{
			Frame = 0;
		}
		spriteBatch.Draw(Lable, new Vector2(Main.screenWidth / 2f, 200), new Rectangle(0, Height * Frame, Width, Height), Color.White, 0, new Vector2(Width, Height) * 0.5f, 1f, SpriteEffects.None, 0);
		base.PostDrawInterface(spriteBatch);
	}
}
