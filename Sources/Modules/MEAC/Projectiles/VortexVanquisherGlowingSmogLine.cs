using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.MEAC.Projectiles;

public class VortexVanquisherGlowingSmogLinePipeline : Pipeline//这个绘制层在火焰之之后，被火焰覆盖
{
	public override void Load()
	{
		effect = ModAsset.VortexVanquisherGlowingSmogLine;
		effect.Value.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_flame_0.Value);
		effect.Value.Parameters["uLine"].SetValue(ModAsset.TrailV.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.VortexVanquisherGlowingSmogLine_Heatmap.Value);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointClamp, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(VortexVanquisherGlowingSmogLinePipeline))]
public class VortexVanquisherGlowingSmogLine_front : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public List<Vector2> oldPos = new List<Vector2>();
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float alpha;
	public VortexVanquisherGlowingSmogLine_front() { }

	public override void Update()
	{
		oldPos.Add(position);
		if (oldPos.Count > 200)
			oldPos.RemoveAt(0);
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity *= 0.9f;
		position += velocity;
	}

	public override void Draw()
	{
		Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
		float fx = timer / maxTime;
		int len = pos.Length;
		if (len <= 2)
			return;
		var bars = new List<Vertex2D>();
		for (int i = 1; i < len; i++)
		{
			Vector2 normal = oldPos[i] - oldPos[i - 1];
			normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);
			var lightColorWithPos = new Color(1f, 1f, 1f, 0);
			float width = (float)Math.Sin(MathF.Pow((i - 1) / (float)(len - 2), 0.2f) * Math.PI);
			bars.Add(oldPos[i] + normal * scale, lightColorWithPos, new Vector3(0, (i + 15 - len) / 75f + timer / 15000f, fx - width * 0.4f));
			bars.Add(oldPos[i] - normal * scale, lightColorWithPos, new Vector3(1, (i + 15 - len) / 75f + timer / 15000f, fx - width * 0.4f));
		}
		if (bars.Count > 0)
		{
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		}
	}
}