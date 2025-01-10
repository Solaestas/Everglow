using Terraria.DataStructures;

namespace Everglow.Commons.MissionSystem.MissionIcons;

public class TextureMissionIcon : IMissionIcon
{
	private TextureMissionIcon()
	{
	}

	private DrawAnimationVertical animation = null;
	private Texture2D texture;
	private string tooltip;

	public string Tooltip => tooltip;

	public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
	{
		var drawCenter = new Vector2(
			destinationRectangle.X + destinationRectangle.Width / 2,
			destinationRectangle.Y + destinationRectangle.Height / 2);

		Rectangle? frameRect = null;
		Vector2 origin = texture.Size() / 2;
		if (animation != null)
		{
			frameRect = animation.GetFrame(texture);
			animation.Update();
			origin = new Vector2(frameRect.Value.Width, frameRect.Value.Height) / 2;
		}

		spriteBatch.Draw(texture, drawCenter, frameRect, Color.White, 0, origin, 1f, SpriteEffects.None, 0);
	}

	public static TextureMissionIcon Create(Texture2D texture, string tooltip = null, DrawAnimationVertical animation = null)
	{
		return new TextureMissionIcon()
		{
			texture = texture,
			tooltip = tooltip ?? string.Empty,
			animation = animation,
		};
	}
}