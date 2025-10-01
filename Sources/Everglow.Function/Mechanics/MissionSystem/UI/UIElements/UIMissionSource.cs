using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

/// <summary>
/// UI element for displaying a headshot of a mission source.
/// The mission source is the source that gives the player the mission, which can be a character, an organization of a specific yggdrasil layer, etc.
/// </summary>
public class UIMissionSource : BaseElement
{
	public MissionSourceBase Source { get; set; } = null;

	public override void Calculation()
	{
		base.Calculation();

		Info.Width.SetValue(80 * MissionContainer.Scale);
		Info.Height.SetValue(80 * MissionContainer.Scale);
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		if (Source is not null)
		{
			float timeValue = MathF.Sin((float)Main.timeForVisualEffects * 0.04f);
			sb.Draw(TextureAssets.MagicPixel.Value, HitBox, new Color(0f, 0f, 0f, 0.6f + 0.1f * timeValue));

			Texture2D sourceTexture = Source.Texture;
			float sourceColor = 0.8f + 0.1f * timeValue;
			Vector2 sourceOrigin = sourceTexture.Size() / 2;
			float sourceScale = sourceTexture.Width >= sourceTexture.Height
				? HitBox.Width / (float)sourceTexture.Width * 0.8f
				: HitBox.Height / (float)sourceTexture.Height * 0.8f;

			sb.Draw(sourceTexture, HitBox.Center(), null, new Color(sourceColor, sourceColor, sourceColor, sourceColor), 0, sourceOrigin, sourceScale, SpriteEffects.None, 0);
		}
	}
}