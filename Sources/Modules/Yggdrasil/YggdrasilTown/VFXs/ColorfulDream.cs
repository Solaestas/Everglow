namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class ColorfulDreamPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.ColorfulDream;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Point.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}

[Pipeline(typeof(ColorfulDreamPipeline))]
public class ColorfulDreamDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public override void Update()
	{
		position += velocity;
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
		velocity *= 0.9f;
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}

		velocity = velocity.RotatedBy(ai[1]);
	}

	public override void Draw()
	{
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color drawColor = new Color(0.1f, 0.2f, 0.3f, 0f);

		// if (timer > maxTime - 20)
		// {
		// drawColor *= (maxTime - timer) / 20f;
		// }
		float timeValue = timer * 0.02f;
		float max = 1f;
		float min = 0f;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, drawColor, new Vector3(min, min, timeValue)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(max, min,  timeValue)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(min, max, timeValue)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(max, max,  timeValue)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}