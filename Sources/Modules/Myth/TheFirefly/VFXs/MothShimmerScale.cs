namespace Everglow.Myth.TheFirefly.VFXs;
public class MothShimmerScalePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.MothShimmerScale;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.MothBlueFireColor.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Noise_turtleCrack.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(MothShimmerScalePipeline))]
public class MothShimmerScaleDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public Vector2 coord;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public float rotation2;
	public float omega;
	public float phi;
	public MothShimmerScaleDust() { }
	public override void Update()
	{
		ai[0] *= 0.99f;
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
		velocity += new Vector2(Main.windSpeedCurrent * 0.1f, 0.012f * scale);
		scale *= 0.98f;
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[0]);
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
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Vector2[] Corner = new Vector2[6];
		for (int x = 0; x < 6; x++)
		{
			Corner[x] = toCorner.RotatedBy(x / 3d * Math.PI);
			Corner[x].Y *= MathF.Sin(phi + (float)(Main.time * 0.03 * omega));
			Corner[x] = Corner[x].RotatedBy(rotation2);
		}
		Color lightColor = new Color(15,45,255, 60);
		float reflectionLight = (1 - pocession) * MathF.Pow((MathF.Sin(phi + (float)(Main.time * 0.03 * omega + 1.57f)) + 1), 4) * 1.6f;
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int x = 0; x < 3; x++)
		{
			bars.Add(new Vertex2D(position, lightColor, new Vector3(coord, reflectionLight)));
			bars.Add(new Vertex2D(position + Corner[2 * x], lightColor, new Vector3(coord, reflectionLight)));

			bars.Add(new Vertex2D(position, lightColor, new Vector3(coord, reflectionLight)));
			bars.Add(new Vertex2D(position + Corner[2 * x + 1], lightColor, new Vector3(coord, reflectionLight)));
		}
		bars.Add(new Vertex2D(position, lightColor, new Vector3(coord, reflectionLight)));
		bars.Add(new Vertex2D(position + Corner[0], lightColor, new Vector3(coord, reflectionLight)));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
