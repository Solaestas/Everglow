namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class RollingRockExplosionPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.RollingRockExplosion;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);

		// effect.Parameters["blur"].SetValue(0.1f);
		// effect.Parameters["speed"].SetValue(4);
		// effect.Parameters["peaks"].SetValue(4);
		// effect.Parameters["peakStrength"].SetValue(0.3f);
		// effect.Parameters["ringSpeed"].SetValue(1.5f);
		// effect.Parameters["smoke"].SetValue(0.4f);
		// effect.Parameters["smokeTime"].SetValue(40);
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

[Pipeline(typeof(RollingRockExplosionPipeline))]
public class RollingRockExplosion : Visual
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
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		if (timer < 30f)
		{
			Lighting.AddLight(position, new Vector3(0.03f, 0.4f, 0.6f) * timer / 30f);
		}
		else
		{
			Lighting.AddLight(position, new Vector3(0.03f, 0.4f, 0.6f) * Math.Clamp((120 - timer) / 30f, 0, 5));
		}
	}

	public override void Draw()
	{
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color drawColor = new Color(0.05f, 0.05f, 0.05f, 0f);
		if (timer > maxTime - 20)
		{
			drawColor *= (maxTime - timer) / 20f;
		}
		float timeValue = timer * 0.02f;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, drawColor, new Vector3(0.2f, 0.2f, timeValue)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(0.8f, 0.2f,  timeValue)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0.2f, 0.8f, timeValue)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(0.8f, 0.8f,  timeValue)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}