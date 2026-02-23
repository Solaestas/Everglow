using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Everglow.Commons.VFX.Visuals;

namespace Everglow.SpellAndSkull.Projectiles.BlackHole.Dusts;


[Pipeline(typeof(WCSPipeline))]
internal class LightDust : Particle
{
	public int time;
	public int time_max;
	public Color drawColor;
	public override void AI()
	{
		time++;
		if (time > time_max - 20)
		{
			velocity *= 0.9f;
			scale *= 0.9f;
		}
		if (time > time_max)
			Kill();
		else
		{
			velocity = velocity.RotatedBy(0.05f);
		}
	}
	public override void Draw()
	{
		Texture2D tex = ModAsset.Ball.Value;
		for (int i = 0; i < 8; i++)
		{
			Color c = drawColor;
			c.A = 0;
			float a = (8 - i) / 8f;
			Ins.Batch.Draw(tex, position - velocity * i * 0.4f, null, c * 1.2f * a, velocity.ToRotation(), tex.Size() / 2, new Vector2(1.2f, 0.8f) * scale * i * 0.5f, 0);
		}
	}
}
