using Everglow.Myth.Common.VFXPipelines;
using Terraria.GameContent;

namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.CrystalStorm;

[Pipeline(typeof(ScreenReflectPipeline))]
internal class CrystalParticleStorm : Visual
{
	public Vector2 position;
	public Vector2 velocity;

	public int timeLeft;
	public float size;
	public float AI2;

	private float Theta;

	private Vector2 p1;
	private Vector2 p2;
	private Vector2 p3;

	private Vector2 po1;
	private Vector2 po2;
	private Vector2 po3;

	private float Ros;

	public override void OnSpawn()
	{
		timeLeft = Main.rand.Next(387, 399);
		Theta += Main.rand.NextFloat(-3.14f, 3.14f);
		Ros = Main.rand.NextFloat(-0.15f, 0.15f);
		p1 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
		p2 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
		p3 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
	}

	public override void Update()
	{
		float Dy = -position.Y;
		float xCoefficient = Dy * Dy / 600f - 0.4f * Dy + 50;
		Vector2 TrueAim = new Vector2(xCoefficient * (float)Math.Sin(Main.timeForVisualEffects * 0.1 + 0), 0) - position;

		AI2 = (byte)(AI2 * 0.95 + xCoefficient * 0.05);

		if (!Main.mouseRight)
		{
			velocity = velocity * 0.75f + new Vector2(TrueAim.SafeNormalize(new Vector2(0, 0.05f)).X, 0) * 0.25f / AI2 * 500f;
			velocity *= Main.rand.NextFloat(0.85f, 1.15f);
		}
		else
		{
			velocity = velocity * 0.75f + new Vector2(TrueAim.SafeNormalize(new Vector2(0, 0.05f)).X, 0) * 0.25f / AI2 * 500f;
			velocity *= Main.rand.NextFloat(0.85f, 1.15f);
		}

		position += velocity;
		timeLeft -= 1;
		if (timeLeft <= 0)
			Kill();

		Theta += Ros;
		po1 = new Vector2(p1.X, p1.Y * (float)Math.Sin(Theta)) * 90 * size;
		po2 = new Vector2(p2.X, p2.Y * (float)Math.Sin(Theta)) * 90 * size;
		po3 = new Vector2(p3.X, p3.Y * (float)Math.Sin(Theta)) * 90 * size;

		if (timeLeft < 20)
			size *= 0.9f;
		if (timeLeft > 100)
			size *= 1.1f;
	}

	public override void Draw()
	{
		Color colorD;

		var Vx = new List<Vertex2D>();
		colorD = new Color(1, 1, velocity.X / 30f, 1f);
		Vx.Add(new Vertex2D(po1 + position, colorD, new Vector3(0.04f, 0.06f, 0.06f)));
		Vx.Add(new Vertex2D(po2 + position, colorD, new Vector3(0.02f, 0.03f, 0.09f)));
		Vx.Add(new Vertex2D(po3 + position, colorD, new Vector3(0.06f, 0.05f, 0.07f)));

		//gd.Textures[0] = TextureAssets.MagicPixel.Value;
		//gd.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
		Ins.Batch.Draw(TextureAssets.MagicPixel.Value, Vx, PrimitiveType.TriangleList);
	}

	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;
}