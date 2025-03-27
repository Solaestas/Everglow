using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

/// <summary>
/// UI element for displaying a headshot of a mission source.
/// The mission source is the source that gives the player the mission, which can be a character, an organization of a specific yggdrasil layer, etc.
/// </summary>
public class UIMissionSourceHeadshot : BaseElement
{
	public int SourceNPC { get; set; } = NPCID.None;

	public override void OnInitialization()
	{
		base.OnInitialization();

		Info.Width.SetValue(80 * MissionContainer.Scale);
		Info.Height.SetValue(80 * MissionContainer.Scale);
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		if (SourceNPC > NPCID.None)
		{
			float timeValue = MathF.Sin((float)Main.timeForVisualEffects * 0.04f);
			sb.Draw(TextureAssets.MagicPixel.Value, HitBox, new Color(0f, 0f, 0f, 0.6f + 0.1f * timeValue));

			float colorValue = 0.8f + 0.1f * timeValue;
			var headshot = ModAsset.AnnaTheGuard.Value;
			sb.Draw(headshot, HitBox, new Color(colorValue, colorValue, colorValue, colorValue));
		}
	}
}