namespace Everglow.Myth.MiscItems.VFXs;
internal class IceParticlePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.IceParticle;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_iceParticle.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Point.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(IceParticlePipeline), typeof(BloomPipeline))]
internal class IceParticleDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public override void Update()
	{
		ai[1] *= 0.99f;
		position += velocity;
		velocity *= 0.98f;
		velocity += new Vector2(Main.windSpeedCurrent * 0.1f, 0.01f * scale);
		scale *= 0.96f;
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[1]);
		if (Collision.SolidCollision(position, 0, 0))
		{
			velocity *= -0.2f;
			timer += 10;
		}
		var tile = Main.tile[(int)(position.X / 16), (int)(position.Y / 16)];
		if(position.Y % 1 < tile.LiquidAmount / 256f)
		{
			timer += 120;
		}
		if(scale < 0.5f)
		{
			timer += 20;
		}
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.3f;
		Lighting.AddLight(position, c * 0.1f, c * 0.1f, c * 0.15f);
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color lightColor = Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f));
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner,new Color(0, 0,pocession, 0.0f), lightColor.ToVector3()),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5),new Color(0, 1, pocession, 0.0f), lightColor.ToVector3()),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5),new Color(1, 0 ,pocession, 0.0f), lightColor.ToVector3()),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1),new Color(1, 1, pocession, 0.0f), lightColor.ToVector3())
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
