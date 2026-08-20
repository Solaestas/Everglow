using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.Mechanics.Mission.UI.MissionContainer;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail;

public class UIMissionStarLevel : BaseElement
{
	// TODO: Add a Mission Star Level
	public int Stars = 1;

	private bool _mouseOver = false;

	public override void OnInitialization()
	{
		Events.OnMouseHover += e =>
		{
			if (Math.Abs(Main.MouseScreen.X - HitBox.Center.X) < Stars * 50 / 2f)
			{
				Instance.MouseText = TextDefinition.GetMissionLevelTooltip(Stars);
				_mouseOver = true;
			}
			else
			{
				_mouseOver = false;
			}
		};
		Events.OnMouseOut += e =>
		{
			_mouseOver = false;
		};
		base.OnInitialization();
	}

	public override void Draw(SpriteBatch sb)
	{
		for (int k = 0; k < Stars; k++)
		{
			Texture2D star = ModAsset.MissionLevelStar.Value;
			Vector2 pos = Info.HitBox.Center() + new Vector2((k - (Stars - 1) / 2f) * 50, 0);
			Rectangle frame = new Rectangle(0, 0, 21, 20);
			sb.Draw(star, pos, frame, Color.White, 0, frame.Size() * 0.5f, 2f, SpriteEffects.None, 0);
			if (_mouseOver)
			{
				frame.Y += 20;
			}
			sb.Draw(star, pos, frame, Color.White, 0, frame.Size() * 0.5f, 2f, SpriteEffects.None, 0);
		}
	}
}
