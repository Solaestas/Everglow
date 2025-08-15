namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;
public class AmberFlamePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.AmberFlame;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.AmberFlame_heatMap.Value);
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
[Pipeline(typeof(AmberFlamePipeline), typeof(BloomPipeline))]
public class AmberFlameDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public AmberFlameDust() { }
	public override void Update()
	{
		ai[1] *= 0.99f;
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
		scale *= 0.96f;
		timer++;
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
		velocity = velocity.RotatedBy(ai[1]);
		if (Collision.SolidCollision(position, 0, 0))
		{
			velocity *= -0.2f;
			timer += 10;
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
		float c = pocession * scale * 0.3f;
		Lighting.AddLight(position, c * 0.7f, c * 0.5f, 0);
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color lightColor = Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f));
		var normalVel = Vector2.Normalize(velocity);
		var bars = new List<Vertex2D>()
		{
			new Vertex2D(position + normalVel * 8 * scale + toCorner,new Color(0, 0f,pocession, 0.0f), lightColor.ToVector3()),
			new Vertex2D(position + normalVel * 8 * scale + toCorner.RotatedBy(Math.PI * 0.5),new Color(0, 1f, pocession, 0.0f), lightColor.ToVector3()),

			new Vertex2D(position - normalVel * 8 * scale + toCorner.RotatedBy(Math.PI * 1.5),new Color(1, 0f ,pocession, 0.0f), lightColor.ToVector3()),
			new Vertex2D(position - normalVel * 8 * scale + toCorner.RotatedBy(Math.PI * 1),new Color(1, 1f, pocession, 0.0f), lightColor.ToVector3())
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
