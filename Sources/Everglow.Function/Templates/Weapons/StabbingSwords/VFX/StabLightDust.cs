using Everglow.Commons.Enums;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords.VFX;

[Pipeline(typeof(WCSPipeline))]
internal class StabLightDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Color EffectColor;
	public Color GlowColor;
	public Vector2 Center;
	public Vector2 Velocity;
	public float Rotation;
	public int MaxTime = 30;
	public int Timeleft = 30;
	public float Scale = 0.5f;

	public override void OnSpawn()
	{
		base.OnSpawn();
	}

	public override void Update()
	{
		float t = MaxTime * 3 / 2f;
		Timeleft--;
		if (Timeleft < t)
		{
			Velocity *= 0.94f;
		}
		if (Timeleft < 0)
		{
			Kill();
		}

		Center += Velocity;
	}

	public override void Draw()
	{
		Color drawColor = EffectColor;
		drawColor = Lighting.GetColor(Center.ToTileCoordinates(), drawColor);
		drawColor.A = 0;
		Texture2D tex = ModAsset.StarSlashGray.Value;
		float timeSize = Timeleft / (float)MaxTime;
		drawColor *= timeSize;
		Ins.Batch.Draw(tex, Center, null, drawColor, Rotation, tex.Size() * 0.5f, new Vector2(timeSize, 2) * Scale, SpriteEffects.None);
		Ins.Batch.Draw(tex, Center, null, GlowColor, Rotation, tex.Size() * 0.5f, new Vector2(timeSize, 2) * Scale, SpriteEffects.None);
	}
}