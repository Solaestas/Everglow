using Everglow.Commons.Enums;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords.VFX;

[Pipeline(typeof(WCSPipeline))]
internal class StabLightDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Color color;
	public Vector2 Center;
	public Vector2 Velocity;
	float ai0 = 0.3f;
	public int maxTimeleft = 30;
	public int timeleft = 30;
	public float alpha = 1;
	public float scale = 0.5f;
	public override void Update()
	{
		float t = maxTimeleft * 3 / 2f;
		timeleft--;
		if (timeleft < t)
		{
			ai0 -= 0.3f / t;
			Velocity *= 0.94f;
		}
		if (timeleft < 0)
			Kill();
		Center += Velocity;
	}
	public override void Draw()
	{
		Color drawColor = color;
		Texture2D tex = ModAsset.Point.Value;
		//drawColor.A = 0;
		Ins.Batch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, Center, new Rectangle(0, 0, 3, 3), Color.White, 0, Vector2.zeroVector, 10, SpriteEffects.None);
		//Main.NewText(Center);
		//Main.NewText(Main.LocalPlayer.Center,Color.Red);

		/*
        for (int i = 0; i < 5; i++)
        {
            Ins.Batch.Draw(tex, Center - Velocity * i / 30 , null, drawColor * alpha * (1 - i / 5f) * 1.2f, Velocity.ToRotation(), tex.Size() / 2, new Vector2(1.2f, 0.8f) * ai0 * scale, SpriteEffects.None);
        }*/
	}
}