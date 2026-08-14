using Everglow.Commons.UI.UIElements;
using Spine;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements;

/// <summary>
/// 沙漏计时器<see cref="MissionContainer"/>
/// </summary>
public class UIMissionHourglassTimer : UIBlock
{
	public float Timer;

	public float MaxTime;

	public bool OnSelect = false;

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		if (MaxTime <= 0)
		{
			return;
		}
		float duration = 1 - (float)(Main.time * 0.001f) % 1;
		int top_liquid_height = (int)(57 * duration);
		int bottom_liquid_height = 57 - top_liquid_height;

		Texture2D tex = ModAsset.TimeFunnel.Value;
		Vector2 pos = Info.TotalHitBox.Center();
		int width = tex.Width / 6;
		Rectangle frame_bound = new Rectangle(0, 0, width, tex.Height);
		if (OnSelect)
		{
			frame_bound = new Rectangle(310, 0, width, tex.Height);
		}
		Rectangle frame_liquid_top = new Rectangle(64, 1 + bottom_liquid_height, 58, top_liquid_height);
		Rectangle frame_liquid_bottom = new Rectangle(65, 57 + top_liquid_height, 56, bottom_liquid_height);
		Rectangle frame_liquid_surface_top = new Rectangle(126, 1 + bottom_liquid_height, 58, 2);
		Rectangle frame_liquid_surface_bottom = new Rectangle(126, 57 + top_liquid_height, 58, 2);
		Rectangle frame_liquid_post = new Rectangle(92, 43, 2, 2);
		Rectangle frame_shadow = new Rectangle(width * 3, 0, width, tex.Height);

		Color liquidColor = new Color(1f, 1f, 1f, 0.75f);

		sb.Draw(tex, pos, frame_bound, Color.White, 0f, frame_bound.Size() / 2f, 1f, SpriteEffects.None, 0f);
		sb.Draw(tex, pos, frame_shadow, Color.White, 0f, frame_bound.Size() / 2f, 1f, SpriteEffects.None, 0f);
		sb.Draw(tex, pos, frame_liquid_top, liquidColor, 0f, new Vector2(29, top_liquid_height), 1f, SpriteEffects.None, 0f);
		sb.Draw(tex, pos, frame_liquid_bottom, liquidColor, 0f, new Vector2(28, -57 + bottom_liquid_height), 1f, SpriteEffects.None, 0f);
		sb.Draw(tex, pos, frame_liquid_surface_top, Color.White, 0f, new Vector2(29, top_liquid_height), 1f, SpriteEffects.None, 0f);
		sb.Draw(tex, pos, frame_liquid_surface_bottom, Color.White, 0f, new Vector2(29, -57 + bottom_liquid_height), 1f, SpriteEffects.None, 0f);
		sb.Draw(tex, new Rectangle((int)pos.X - 1, (int)pos.Y, 2, 56 - bottom_liquid_height), frame_liquid_post, liquidColor);
		DrawReflection(sb);
	}

	private void DrawReflection(SpriteBatch sb)
	{
		Texture2D tex = ModAsset.TimeFunnel.Value;
		Vector2 pos = Info.TotalHitBox.Center();
		int width = tex.Width / 6;
		Rectangle frame_reflection = new Rectangle(width * 4, 0, width, tex.Height);
		sb.Draw(tex, pos, frame_reflection, Color.White, 0f, frame_reflection.Size() / 2f, 1f, SpriteEffects.None, 0f);
	}
}
