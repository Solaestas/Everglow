using Terraria.GameContent;

namespace Everglow.Yggdrasil.CorruptWormHive.VFXs;

[Pipeline(typeof(WCSPipeline))]
internal class BrokenCrystal : Visual
{
	public Vector2 position;
	public Vector2 velocity;
	public int timeLeft;
	public float size;
	public float omega;
	public float rotation;

	private float Theta;

	private Vector2 VS1;
	private Vector2 VS2;
	private Vector2 VS3;

	private Vector2 p1;
	private Vector2 p2;
	private Vector2 p3;

	private Vector2 po1;
	private Vector2 po2;
	private Vector2 po3;
	private float RamdomC;

	private float Ros;

	public override void OnSpawn()
	{
		timeLeft = Main.rand.Next(387, 399);
		RamdomC = 0;
		Theta += Main.rand.NextFloat(-3.14f, 3.14f);
		Ros = Main.rand.NextFloat(-0.15f, 0.15f);
		p1 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
		p2 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
		p3 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
		base.OnSpawn();
	}

	public override void Update()
	{
		position += velocity;
		timeLeft -= 1;
		if (timeLeft <= 0)
			Kill();
		if (timeLeft <= 30)
			velocity *= 0.98f;
		velocity = velocity.RotatedBy(omega);
		omega += Main.rand.NextFloat(-0.05f, 0.05f);
		if (Math.Abs(omega) > 0.15)
			omega *= 0.98f;
		RamdomC += 3;

		Theta += Ros;
		po1 = new Vector2(p1.X, p1.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
		po2 = new Vector2(p2.X, p2.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
		po3 = new Vector2(p3.X, p3.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
		rotation -= omega * 0.66f;
		velocity *= 0.99f;
		size *= 0.99f;
		if (RamdomC > 300)
			base.Update();
	}

	public override void Draw()
	{
		var Vy = new List<Vertex2D>();
		Color colorD = Color.White;
		Vector2 v1 = po1 + position;
		Vector2 v2 = po2 + position;
		Vector2 v3 = po3 + position;
		if (VS1 == Vector2.Zero)
			VS1 = v1 - Main.screenPosition;
		if (VS2 == Vector2.Zero)
			VS2 = v2 - Main.screenPosition;
		if (VS3 == Vector2.Zero)
			VS3 = v3 - Main.screenPosition;
		Vy.Add(new Vertex2D(v1, colorD, new Vector3(VS1.X / Main.screenTarget.Width, VS1.Y / Main.screenTarget.Height, 0)));
		Vy.Add(new Vertex2D(v2, colorD, new Vector3(VS2.X / Main.screenTarget.Width, VS2.Y / Main.screenTarget.Height, 0)));
		Vy.Add(new Vertex2D(v3, colorD, new Vector3(VS3.X / Main.screenTarget.Width, VS3.Y / Main.screenTarget.Height, 0)));

		GraphicsDevice gd = Main.graphics.GraphicsDevice;

		var Co0 = new Color(255, 0, 15);
		int DrawBase = (int)(300 - RamdomC);
		var Vx = new List<Vertex2D>();
		colorD = new Color(DrawBase / 4, DrawBase / 180, DrawBase / 180, 155);
		Vx.Add(new Vertex2D(po1 + position, colorD, new Vector3(0, 0, 0)));
		Vx.Add(new Vertex2D(po2 + position, colorD, new Vector3(0, 0, 0)));
		Vx.Add(new Vertex2D(po3 + position, colorD, new Vector3(0, 0, 0)));
		gd.Textures[0] = TextureAssets.MagicPixel.Value;
		gd.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
	}

	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;
}