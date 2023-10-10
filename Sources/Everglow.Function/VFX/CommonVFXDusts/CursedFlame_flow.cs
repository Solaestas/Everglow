using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.VFX.CommonVFXDusts;

public abstract class FlowDraw : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public FlowDraw() { }
	public FlowDraw(Vector2 position, Vector2 velocity, params float[] ai)
	{
		this.position = position;
		this.velocity = velocity;
		this.ai = ai;//可以认为params传入的都是右值，可以直接引用
	}
}

public class CursedFlame_flowPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.CursedFlame_flow;
		
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uNoise"].SetValue(ModAsset.Noise_melting.Value);
		effect.Parameters["uTransform"].SetValue(model * projection);
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.Trail_6.Value);
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(CursedFlame_flowPipeline), typeof(HeatMapRenderPipeline_cursedFlame), typeof(BloomPipeline))]
public class CursedFlame_flowDust : FlowDraw
{
	public List<Vector2> oldPos = new List<Vector2>();
	public float timer;
	public float maxTime;
	public CursedFlame_flowDust() { }
	public CursedFlame_flowDust(int maxTime, Vector2 position, Vector2 velocity, params float[] ai) : base(position, velocity, ai)
	{
		this.maxTime = maxTime;
	}

	public override void Update()
	{
		if (oldPos.Count == 0)
		{
			for (int x = 0; x < 12; x++)
			{
				position += velocity;
				oldPos.Add(position);
				if (oldPos.Count > 12)
					oldPos.RemoveAt(0);
				velocity *= 0.99f;
				if (timer > maxTime)
					Active = false;
				velocity = velocity.RotatedBy(ai[1]);
			}
		}
		else
		{
			position += velocity;
			oldPos.Add(position);
			if (oldPos.Count > 17)
				oldPos.RemoveAt(0);
			velocity *= 0.99f;
			timer++;
			if (timer > maxTime)
				Active = false;
			velocity = velocity.RotatedBy(ai[1]);
		}
		ai[1] += ai[3];
		if (Math.Abs(ai[1]) > 0.06f)
		{
			ai[1] *= 0.9f;
			ai[3] *= -1;
		}
		float delC = ai[2] * 0.05f * (float)Math.Sin((maxTime - timer) / 40d * Math.PI);
		Lighting.AddLight((int)(position.X / 16), (int)(position.Y / 16), 0.45f * delC, 0.85f * delC, 0f);
		if (Collision.SolidCollision(position, 0, 0))
			timer += 4;
	}

	public override void Draw()
	{
		Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
		int len = pos.Length;
		if (len <= 2)
			return;
		float timeValue = (float)Main.timeForVisualEffects * 0.002f;
		var bars = new List<Vertex2D>();
		for (int i = 1; i < len; i++)
		{
			Vector2 normal = oldPos[i] - oldPos[i - 1];
			normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);
			float width = MathF.Sin(MathHelper.Pi * (i - 0) / (len - 1));
			var drawColor = new Color(1f, 1f, timer / maxTime, width / 3);
			bars.Add(oldPos[i] + normal * ai[2], drawColor, new Vector3(0.7f, ai[0], i / 80f - timeValue));
			bars.Add(oldPos[i] - normal * ai[2], drawColor, new Vector3(0.3f, ai[0] + 0.2f, i / 80f - timeValue));
		}
		if(bars.Count > 2)
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}