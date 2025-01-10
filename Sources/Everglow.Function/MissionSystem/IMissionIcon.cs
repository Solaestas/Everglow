namespace Everglow.Commons.MissionSystem;

public interface IMissionIcon
{
	public string Tooltip { get; }

	public abstract void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle);
}
