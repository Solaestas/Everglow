using Everglow.Commons.Enums;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.CustomTiles.GameInteraction;

[Pipeline(typeof(WCSPipeline))]
public class PlayerCollisionIndicator : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public float Timer;
	public float MaxTime;
	public Rectangle HitBox;
	public Vector2 Pos;

	public override void Update()
	{
		Timer++;
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		float fade = 1f;
		if(Timer > 1)
		{
			fade = 0.2f;
		}
		Color drawC = new Color(1f, 1f, 1f, 0) * fade;
		Texture2D texture = ModAsset.LightPoint2.Value;
		Ins.Batch.Draw(texture, Pos, null, drawC, 0, texture.Size() * 0.5f, 0.25f, SpriteEffects.None);
		if(HitBox != default)
		{
			Ins.Batch.Draw(texture, HitBox, new Rectangle(24, 24, 4, 4), drawC);
		}
	}
}