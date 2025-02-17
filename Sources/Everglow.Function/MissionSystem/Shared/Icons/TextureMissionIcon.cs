using Everglow.Commons.MissionSystem.Core;
using Terraria.DataStructures;

namespace Everglow.Commons.MissionSystem.Shared.Icons;

public class TextureMissionIcon : MissionIconBase
{
	private TextureMissionIcon()
	{
	}

	private DrawAnimationVertical animation = null;
	private Texture2D texture;
	private string tooltip;

	public override string Tooltip => tooltip;

	public override void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
	{
		var drawCenter = new Vector2(
			destinationRectangle.X + destinationRectangle.Width / 2,
			destinationRectangle.Y + destinationRectangle.Height / 2);

		var frameRect = new Rectangle(0, 0, texture.Width, texture.Height);
		Vector2 origin = texture.Size() / 2;
		if (animation != null)
		{
			frameRect = animation.GetFrame(texture);
			animation.Update();
			origin = new Vector2(frameRect.Width, frameRect.Height) / 2;
		}

		var scale = GetTextureScale(destinationRectangle, frameRect);
		spriteBatch.Draw(texture, drawCenter, frameRect, Color.White, 0, origin, scale, SpriteEffects.None, 0);
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