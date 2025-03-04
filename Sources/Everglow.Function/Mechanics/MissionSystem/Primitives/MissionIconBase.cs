namespace Everglow.Commons.Mechanics.MissionSystem.Primitives;

public abstract class MissionIconBase
{
	public virtual string Tooltip { get; }

	public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle) => Draw(spriteBatch, destinationRectangle, Color.White);

	public abstract void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color);

	public static float GetTextureScale(Rectangle destinationRect, Rectangle frameRect)
	{
		var scale = frameRect.Width > frameRect.Height
			? destinationRect.Width / (float)frameRect.Width
			: destinationRect.Height / (float)frameRect.Height;
		return scale > 1f ? 1f : scale;
	}
}