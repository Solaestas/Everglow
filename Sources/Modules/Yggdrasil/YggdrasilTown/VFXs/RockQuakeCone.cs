namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class RockQuakeConePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.RockQuakeCone;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Ins.Batch.BindTexture<Vertex2D>(Commons.ModAsset.Trail_8.Value);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(RockQuakeConePipeline))]
public class RockQuakeCone : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public float[] ai;

	public override void Update()
	{
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		float progress = timer / maxTime;
		float progressLength = Math.Min(progress * 2f, 1);
		progressLength = MathF.Pow(progressLength, 0.3f);
		Vector2 vel = new Vector2(scale * 0.4f * progressLength, 0).RotatedBy(rotation) * 5;
		float fade = 1f;
		if (progress > 0.7f)
		{
			fade -= (progress - 0.7f) / 0.3f;
		}
		fade = Math.Max(0, fade);
		float colorFade = scale * 0.02f * fade;
		Lighting.AddLight(position + vel, colorFade * 0.6f, colorFade * 0.5f, colorFade * 0.5f);
		timer++;
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
	}

	public override void Draw()
	{
		float progress = timer / maxTime;
		float progressLength = Math.Min(progress * 2f, 1);
		progressLength = MathF.Pow(progressLength, 0.3f);
		int maxLength = 15;
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int y = 0; y < maxLength; y++)
		{
			Vector2 vel = new Vector2(scale * 0.4f * progressLength, 0).RotatedBy(rotation) * y;
			Vector2 width = vel.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 40;
			float fade = y / (float)(maxLength - 1);
			float shapeFade = 1 - fade;
			if(progress > 0.7f)
			{
				fade -= (progress - 0.7f) / 0.3f;
			}
			fade = Math.Max(0, fade);
			bars.Add(position + vel + width, new Color(0.8f, 0.66f, 0.66f, 0) * fade, new Vector3(y * 0.1f, 0, shapeFade));
			bars.Add(position + vel - width, new Color(0.8f, 0.66f, 0.66f, 0) * fade, new Vector3(y * 0.1f, 1, shapeFade));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}