namespace Everglow.Myth.TheFirefly.Projectiles.PylonPostEffect;

internal abstract class ShaderDraw : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public ShaderDraw() { }
	public ShaderDraw(Vector2 position, Vector2 velocity, params float[] ai)
	{
		this.position = position;
		this.velocity = velocity;
		this.ai = ai;//可以认为params传入的都是右值，可以直接引用
	}
}

internal class WaveOfEffectPylonHit_CorruptPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.WaveOfEffectPylonHit_Corrupt;
		effect.Value.Parameters["uNoise"].SetValue(ModAsset.HiveCyberNoise.Value);
		effect.Value.Parameters["uPowder"].SetValue(ModAsset.NoiseSand.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D FlameColor = ModAsset.WaveOfEffectPylonHit_Corrupt_Color.Value;
		Ins.Batch.BindTexture<Vertex2D>(FlameColor);
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.NonPremultiplied, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(WaveOfEffectPylonHit_CorruptPipeline), typeof(BloomPipeline))]
internal class WaveOfEffectPylonHit_CorruptWave : ShaderDraw
{
	/// <summary>
	/// ai[0]x相位
	/// ai[1]波速
	/// ai[2]宽度
	/// </summary>
	public float timer;
	public float maxTime;
	public float radius;
	public WaveOfEffectPylonHit_CorruptWave() { }
	public WaveOfEffectPylonHit_CorruptWave(int maxTime, Vector2 position, Vector2 velocity, params float[] ai) : base(position, velocity, ai)
	{
		this.maxTime = maxTime;
	}

	public override void Update()
	{
		position += velocity;
		radius += ai[1] * ((maxTime - timer) / maxTime);
		timer++;
		if (timer > maxTime)
			Active = false;


		float delC = ai[2] * 0.05f * (float)Math.Sin((maxTime - timer) / maxTime * Math.PI);
		Lighting.AddLight((int)(position.X / 16), (int)(position.Y / 16), 0.015f * delC, 0, 0.45f * delC);
	}

	public override void Draw()
	{
		float fx = timer / maxTime;
		int len = (int)(radius / 3f);
		if (len <= 2)
			return;
		var bars = new Vertex2D[len * 2 + 2];
		for (int i = 0; i < len + 1; i++)
		{
			Vector2 normal = new Vector2(0, 1).RotatedBy(i / (double)len * Math.PI * 2);
			Vector2 radiousDraw = normal * radius;

			var drawcRope = new Color(fx * fx * fx * 2 - 0.1f, 0.5f, 1, 150 / 255f);
			float width = ai[2];
			float texCoordWidth = 0.37f;
			if (width > radiousDraw.Length())
			{
				texCoordWidth *= radiousDraw.Length() / width;
				width = radiousDraw.Length();
			}

			bars[2 * i] = new Vertex2D(position + radiousDraw, drawcRope, new Vector3(ai[0] + timer / maxTime, i / (float)len * 2, 0.8f - fx));
			bars[2 * i + 1] = new Vertex2D(position + radiousDraw - normal * width, drawcRope, new Vector3(texCoordWidth + ai[0] + timer / maxTime, i / (float)len * 2, 0.8f - fx));
		}

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
internal class WaveOfEffectPylonHit_CrimsonPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.WaveOfEffectPylonHit_Crimson;
		effect.Value.Parameters["uNoise"].SetValue(ModAsset.HiveCyberNoise.Value);
		effect.Value.Parameters["uPowder"].SetValue(ModAsset.NoiseSand.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D FlameColor = ModAsset.WaveOfEffectPylonHit_Crimson_Color.Value;
		Ins.Batch.BindTexture<Vertex2D>(FlameColor);
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.NonPremultiplied, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(WaveOfEffectPylonHit_CrimsonPipeline), typeof(BloomPipeline))]
internal class WaveOfEffectPylonHit_CrimsonWave : ShaderDraw
{
	/// <summary>
	/// ai[0]x相位
	/// ai[1]波速
	/// ai[2]宽度
	/// </summary>
	public float timer;
	public float maxTime;
	public float radius;
	public WaveOfEffectPylonHit_CrimsonWave() { }
	public WaveOfEffectPylonHit_CrimsonWave(int maxTime, Vector2 position, Vector2 velocity, params float[] ai) : base(position, velocity, ai)
	{
		this.maxTime = maxTime;
	}

	public override void Update()
	{
		position += velocity;
		radius += ai[1] * ((maxTime - timer) / maxTime);
		timer++;
		if (timer > maxTime)
			Active = false;


		float delC = ai[2] * 0.05f * (float)Math.Sin((maxTime - timer) / maxTime * Math.PI);
		Lighting.AddLight((int)(position.X / 16), (int)(position.Y / 16), 0.015f * delC, 0, 0.45f * delC);
	}

	public override void Draw()
	{
		float fx = timer / maxTime;
		int len = (int)(radius / 3f);
		if (len <= 2)
			return;
		var bars = new Vertex2D[len * 2 + 2];
		for (int i = 0; i < len + 1; i++)
		{
			Vector2 normal = new Vector2(0, 1).RotatedBy(i / (double)len * Math.PI * 2);
			Vector2 radiousDraw = normal * radius;

			var drawcRope = new Color(fx * fx * fx * 2 - 0.1f, 0.5f, 1, 150 / 255f);
			float width = ai[2];
			float texCoordWidth = 0.37f;
			if (width > radiousDraw.Length())
			{
				texCoordWidth *= radiousDraw.Length() / width;
				width = radiousDraw.Length();
			}

			bars[2 * i] = new Vertex2D(position + radiousDraw, drawcRope, new Vector3(ai[0] + timer / maxTime, i / (float)len * 2, 0.8f - fx));
			bars[2 * i + 1] = new Vertex2D(position + radiousDraw - normal * width, drawcRope, new Vector3(texCoordWidth + ai[0] + timer / maxTime, i / (float)len * 2, 0.8f - fx));
		}

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}