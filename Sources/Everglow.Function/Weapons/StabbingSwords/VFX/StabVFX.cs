using Everglow.Commons;
using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.VFX;
public class Draw3DPipieline : Pipeline
{

	public override void BeginRender()
	{
		Ins.Batch.Begin(BlendState.AlphaBlend);

	}
	public override void EndRender()
	{

		var camPos = new Vector3(Main.screenWidth / 2 + Main.screenPosition.X, Main.screenHeight / 2 + Main.screenPosition.Y, 0);
		var matrix = Matrix.CreateLookAt(camPos, new Vector3(camPos.X, camPos.Y, 1), Vector3.Down);
		matrix *= Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 2f, Main.graphics.GraphicsDevice.Viewport.AspectRatio, 1, 2000);



		effect.Value.Parameters["uTransform"].SetValue(matrix);
		effect.Value.CurrentTechnique.Passes[0].Apply();

		Ins.Batch.End();
	}
	public override void Load()
	{
		effect = ModContent.Request<Effect>("Everglow/Myth/Effects/DrawPrim3D", ReLogic.Content.AssetRequestMode.ImmediateLoad);
		Ins.Batch.RegisterVertex<Vertex3D_2>();
	}
}
[Pipeline(typeof(Draw3DPipieline))]
public class StabVFX : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;
	public Vector2 pos;
	public Vector2 vel;
	public float scale = 30;
	public Color color;
	public int timeleft = 20;
	public float alpha = 1f;
	public float width = 10;
	public int maxtime = 20;
	public StabVFX()
	{
		alpha = 0.6f;
		timeleft = maxtime = 10;
		randomRot = Main.rand.NextFloatDirection() * 0.5f;
	}

	public static Vector3 RotatedBy(Vector3 v, Vector3 u, float ang)//v以u为轴旋转
	{
		float cos = (float)Math.Cos(ang);
		return v * cos + Vector3.Dot(v, u) * u * (1 - cos) + Vector3.Cross(u, v) * (float)Math.Sin(ang);
	}
	float speed = 1f;
	float randomRot = 0;
	public override void Update()
	{
		if (timeleft < maxtime * 2f / 3f)
		{
			alpha *= 0.75f;
			speed *= 0.9f;
		}
		width = 5;
		timeleft--;
		if (timeleft <= 0)
			Kill();
		pos -= vel * speed * 3f;

		scale += 0.6f;
	}
	public override void Draw()
	{
		List<Vertex3D_2> vertices = new();
		Color c = color * alpha;
		c.A = 0;
		float ssc = Main.Transform.M11;
		for (int i = 0; i <= 30; i++)
		{
			float a = i * MathHelper.TwoPi / 30f;
			Vector3 v = RotatedBy(Vector3.unitZ, new Vector3(vel.X, vel.Y, 0), a);
			Vector3 rAix = Vector3.Cross(new Vector3(vel.X, vel.Y, 0), Vector3.unitZ);
			v = RotatedBy(v, rAix, randomRot);
			Vector3 p = new Vector3(pos.X, pos.Y, 525 / ssc) + v * scale;
			vertices.Add(new(p, new Vector3(i / 30f, 0, 0), c));
			vertices.Add(new(p - new Vector3(vel.X, vel.Y, 0) * width * 2, new Vector3(i / 30f, 1, 0), c));
		}
		Ins.Batch.Draw(ModAsset.Trail_0.Value, vertices, PrimitiveType.TriangleStrip);
	}
}


