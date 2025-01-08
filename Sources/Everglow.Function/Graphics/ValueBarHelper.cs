using Everglow.Commons.Vertex;

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

	/// <summary>
	/// Draw a circle bar progress as value (0~1)
	/// </summary>
	/// <param name="spriteBatch"></param>
	/// <param name="position"></param>
	/// <param name="value"></param>
	/// <param name="color0"></param>
	/// <param name="color1"></param>
	/// <param name="scale"></param>
	public static void DrawCircleValueBar(SpriteBatch spriteBatch, Vector2 position, float value, Color color0, Color color1, float scale = 1, Texture2D centerIcon = null)
	{
		spriteBatch.Draw(ModAsset.White.Value, position, null, Color.White, 0, Vector2.Zero, 0, SpriteEffects.None, 0);

		var frameColor = new Color(0.05f, 0.05f, 0.08f, 0.9f);
		var frameInnerColor = new Color(0.15f, 0.25f, 0.38f, 0.4f);
		if(centerIcon is not null)
		{
			frameInnerColor = color1;
		}

		Color progressColor = color0;
		Color progressInnerColor = progressColor * 0.9f;
		progressInnerColor.A = 255;

		Vector2 radius = new Vector2(0, 100) * scale;
		Vector2 frameRadiusOffset = new Vector2(0, 40) * scale;
		Vector2 frameInnerRadiusOffset = frameRadiusOffset * 0.3f;
		Vector2 progressRadiusOffset = frameRadiusOffset * 0.7f;
		Vector2 progressInnerRadiusOffset = frameRadiusOffset * 0.3f;

		// Draw frame
		if (centerIcon is null)
		{
			DrawCircle(spriteBatch, position, frameColor, radius, frameRadiusOffset, 1);
			DrawCircle(spriteBatch, position, frameInnerColor, radius, frameInnerRadiusOffset, 1);
		}
		else
		{
			var fullFrame = (radius + frameRadiusOffset) / 2f;
			var fullFrameInner = (radius + frameInnerRadiusOffset) / 2f;
			DrawCircle(spriteBatch, position, frameColor, fullFrame, fullFrame, 1);
			DrawCircle(spriteBatch, position, frameInnerColor, fullFrameInner, fullFrameInner, 1);
		}

		// Draw progress
		DrawCircle(spriteBatch, position, frameColor, radius, frameRadiusOffset, value);
		DrawCircle(spriteBatch, position, progressColor, radius, progressRadiusOffset, value);
		DrawCircle(spriteBatch, position, progressInnerColor, radius, progressInnerRadiusOffset, value);

		if (centerIcon is not null)
		{
			var circleRadius = (radius - progressInnerRadiusOffset).Length();
			var iconScale = circleRadius * 2 * 0.8f / centerIcon.Width;
			var iconColor = new Color(1f, 1f, 1f, 0f);
			spriteBatch.Draw(centerIcon, position, null, iconColor, 0, centerIcon.Size() / 2, iconScale, SpriteEffects.None, 0);
		}
	}

	/// <summary>
	/// Draw a circle
	/// </summary>
	/// <param name="spriteBatch"></param>
	/// <param name="position"></param>
	/// <param name="color"></param>
	/// <param name="radius"></param>
	/// <param name="radiusOffset"></param>
	/// <param name="progress"></param>
	/// <param name="totalVertexCount"></param>
	public static void DrawCircle(SpriteBatch spriteBatch, Vector2 position, Color color, Vector2 radius, Vector2 radiusOffset, float progress, int totalVertexCount = 180)
	{
		int circleVertexCount = (int)(totalVertexCount * progress);
		List<Vertex2D> lineShadowVertices = [];
		for (int i = 0; i <= circleVertexCount; i++)
		{
			var rotation = MathF.PI * 2 * i / totalVertexCount;
			lineShadowVertices.Add(position - (radius - radiusOffset).RotatedBy(rotation), color, new Vector3(1, 0, 0));
			lineShadowVertices.Add(position - (radius + radiusOffset).RotatedBy(rotation), color, new Vector3(1, 0, 0));
		}

		spriteBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, lineShadowVertices.ToArray(), 0, lineShadowVertices.Count - 2);
	}
}