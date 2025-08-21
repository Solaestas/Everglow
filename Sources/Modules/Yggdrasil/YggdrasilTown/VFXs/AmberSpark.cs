namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class AmberSparkPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.AmberSpark;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_AmberSpark.Value);
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

[Pipeline(typeof(AmberSparkPipeline), typeof(BloomPipeline))]
public class AmberSparkDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public AmberSparkDust()
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
		velocity *= 0.98f;

		scale *= 0.995f;
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}

		bool collide = false;
		if (Collision.SolidCollision(position + new Vector2(velocity.X, 0), 0, 0))
		{
			collide = true;
			velocity.X *= -0.5f;
		}
		if (Collision.SolidCollision(position + new Vector2(0, velocity.Y), 0, 0))
		{
			collide = true;
			velocity.Y *= -0.5f;
		}
		if (!collide)
		{
			velocity += new Vector2(0, 0.44f);
		}
		var tile = Main.tile[(int)(position.X / 16), (int)(position.Y / 16)];
		if (position.Y % 1 < tile.LiquidAmount / 256f)
		{
			timer += 120;
		}
		if (scale < 0.5f)
		{
			timer += 20;
		}
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.1f;
		Lighting.AddLight(position, c * 0.5f, c * 0.2f, 0);
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, new Color(0, 0, pocession, 0.0f), new Vector3(0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), new Color(0, 1, pocession, 0.0f), new Vector3(0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), new Color(1, 0, pocession, 0.0f), new Vector3(0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), new Color(1, 1, pocession, 0.0f), new Vector3(0)),
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}