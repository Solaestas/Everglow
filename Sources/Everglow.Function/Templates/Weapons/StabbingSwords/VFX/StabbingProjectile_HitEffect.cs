using Everglow.Commons.Enums;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords.VFX;

[Pipeline(typeof(WCSPipeline))]
public class StabbingProjectile_HitEffect : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;

	public Vector2 Position;

	public float Rotation;

	public float Scale;

	public Color Color;

	public float Timer = 0;

	public float MaxTime;

	public override void Update()
	{
		Timer++;
		if(Timer >= MaxTime)
		{
			Active = false;
			return;
		}
		base.Update();
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.SparkLight.Value;
		float colorFade = MathF.Max(0, 1 - Timer / MaxTime);
		colorFade = MathF.Pow(colorFade, 0.5f);
		if(Color.A > 0)
		{
			Texture2D tex_black = ModAsset.SparkDark.Value;
			Ins.Batch.Draw(tex_black, Position, null, Color.White * (Color.A / 255f), Rotation, new Vector2(218, 128), Scale * colorFade, SpriteEffects.None);
		}
		Color drawGlow = Color;
		drawGlow.A = 0;
		Ins.Batch.Draw(tex, Position, null, drawGlow, Rotation, new Vector2(218, 128), Scale * colorFade, SpriteEffects.None);
		Lighting.AddLight(Position, Color.ToVector3() * Scale * 4 * colorFade);
	}
}