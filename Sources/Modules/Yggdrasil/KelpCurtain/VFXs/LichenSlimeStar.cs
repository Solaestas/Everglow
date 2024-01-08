namespace Everglow.Yggdrasil.KelpCurtain.VFXs;

public class LichenSlimeStarPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.LichenSlimeStar;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_LichenSlimeStar.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D lightness = Commons.ModAsset.Star.Value;
		Ins.Batch.BindTexture<Vertex2D>(lightness);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointClamp, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(LichenSlimeStarPipeline))]
public class LichenSlimeStar : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public LichenSlimeStar() { }
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
		velocity += new Vector2(Main.windSpeedCurrent * 0.1f, 0.04f * scale * 0.1f);
		scale *= 0.98f;
		timer++;
		if (timer > maxTime)
			Active = false;
		if (Collision.SolidCollision(position + new Vector2(velocity.X, 0), 0, 0))
		{
			velocity.X *= -0.4f;
			timer += 10;
		}
		if (Collision.SolidCollision(position + new Vector2(0, velocity.Y), 0, 0))
		{
			velocity.Y *= -0.4f;
			timer += 10;
		}
		var tile = Main.tile[(int)(position.X / 16), (int)(position.Y / 16)];
		if (position.Y % 1 < tile.LiquidAmount / 256f)
		{
			timer += 120;
		}
		if (scale < 0.05f)
		{
			timer += 20;
		}
		Lighting.AddLight(position, 0.35f * scale, 0.5f * scale, 0.25f * scale);
	}

	public override void Draw()
	{
		Color lightColor = new Color(0.7f, 1f, 0.4f, 0);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + new Vector2(-25 * scale, -25) * scale,lightColor, new Vector3(0, 0, 0)),
			new Vertex2D(position + new Vector2(-25 * scale, 25) * scale,lightColor, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(25 * scale, -25) * scale,lightColor, new Vector3(1, 0, 0)),

			new Vertex2D(position + new Vector2(25 * scale, -25) * scale,lightColor, new Vector3(1, 0, 0)),
			new Vertex2D(position + new Vector2(-25 * scale, 25) * scale,lightColor, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(25 * scale, 25) * scale,lightColor, new Vector3(1, 1, 0)),

			new Vertex2D(position + new Vector2(-25, -25 * scale) * scale,lightColor, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(-25, 25 * scale) * scale,lightColor, new Vector3(1, 1, 0)),
			new Vertex2D(position + new Vector2(25, -25 * scale) * scale,lightColor, new Vector3(0, 0, 0)),

			new Vertex2D(position + new Vector2(25, -25 * scale) * scale,lightColor, new Vector3(0, 0, 0)),
			new Vertex2D(position + new Vector2(-25, 25 * scale) * scale,lightColor, new Vector3(1, 1, 0)),
			new Vertex2D(position + new Vector2(25, 25 * scale) * scale,lightColor, new Vector3(1, 0, 0))
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}