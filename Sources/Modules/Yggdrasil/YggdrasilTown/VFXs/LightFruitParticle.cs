namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class LightFruitParticlePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.LightFruitParticle;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.AmberFlame_heatMap.Value);
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
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

[Pipeline(typeof(LightFruitParticlePipeline), typeof(BloomPipeline))]
public class LightFruitParticleDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public LightFruitParticleDust()
	{
	}

	public override void Update()
	{
		position += velocity;
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			timer = maxTime;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			timer = maxTime;
		}
		velocity *= 0.995f;
		scale = MathF.Sin(timer / maxTime * MathHelper.Pi) * ai[0];
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color lightColor = new Color(1f, 1f, 1f, 0);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, new Color(0, 0f, pocession, 0.0f), lightColor.ToVector3()),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), new Color(0, 1f, pocession, 0.0f), lightColor.ToVector3()),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), new Color(1, 0f, pocession, 0.0f), lightColor.ToVector3()),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), new Color(1, 1f, pocession, 0.0f), lightColor.ToVector3()),
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}