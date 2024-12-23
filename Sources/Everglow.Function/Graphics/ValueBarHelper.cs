namespace Everglow.Commons.Graphics;

/// <summary>
/// UI Bar(like npc health) painter.
/// </summary>
public class ValueBarHelper
{
	/// <summary>
	/// Draw a bar progress as value (0~1)
	/// </summary>
	/// <param name="spriteBatch"></param>
	/// <param name="center"></param>
	/// <param name="scale"></param>
	/// <param name="value"></param>
	/// <param name="color0"></param>
	/// <param name="color1"></param>
	public static void DrawValueBar(SpriteBatch spriteBatch, Vector2 center, float value, Color color0, Color color1, float scale = 1)
	{
		float powerProgress = value;
		var progressTexture = ModAsset.White.Value;
		var progressPosition = center;

		var frameColor = new Color(0.05f, 0.05f, 0.08f, 0.9f);
		var frameColor2 = new Color(0.15f, 0.25f, 0.38f, 0.4f);
		Vector2 frameScale = new Vector2(2f, 0.4f) * 0.05f * scale;
		Vector2 frameScale2 = new Vector2(1.8f, 0.2f) * 0.05f * scale;

		Color lineColor = Color.Lerp(color0, color1, value);
		Color lineColorInner = lineColor * 0.8f;
		lineColorInner.A = 255;
		Vector2 lineScaleOuter = new Vector2(2.2f * powerProgress + 0.2f, 0.7f) * 0.05f * scale;
		Vector2 lineScale = new Vector2(2.2f * powerProgress, 0.5f) * 0.05f * scale;
		Vector2 lineScale2 = new Vector2(2.2f * powerProgress - 0.2f, 0.3f) * 0.05f * scale;
		var linePositionOffset = new Vector2(-2.2f * (1 - powerProgress) * progressTexture.Width * 0.025f, 0);

		spriteBatch.Draw(progressTexture, progressPosition, null, frameColor, 0, progressTexture.Size() / 2, frameScale, SpriteEffects.None, 0);
		spriteBatch.Draw(progressTexture, progressPosition, null, frameColor2, 0, progressTexture.Size() / 2f, frameScale2, SpriteEffects.None, 0);

		spriteBatch.Draw(progressTexture, progressPosition + linePositionOffset, null, frameColor, 0, progressTexture.Size() / 2, lineScaleOuter, SpriteEffects.None, 0);
		spriteBatch.Draw(progressTexture, progressPosition + linePositionOffset, null, lineColor, 0, progressTexture.Size() / 2, lineScale, SpriteEffects.None, 0);
		spriteBatch.Draw(progressTexture, progressPosition + linePositionOffset, null, lineColorInner, 0, progressTexture.Size() / 2, lineScale2, SpriteEffects.None, 0);
	}
}