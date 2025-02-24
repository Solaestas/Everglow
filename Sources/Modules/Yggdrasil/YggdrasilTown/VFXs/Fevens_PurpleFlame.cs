namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class Fevens_PurpleFlamePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.Fevens_PurpleFlame;
		effect.Value.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_perlin.Value);
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_Fevens_PurpleFlame.Value);
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

[Pipeline(typeof(Fevens_PurpleFlamePipeline))]
public class Fevens_PurpleFlameDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public Fevens_PurpleFlameDust()
	{
	}

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
		if (scale < 160)
		{
			scale += 2f;
		}
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}

		velocity = velocity.RotatedBy(ai[1]);
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		float timeValue = (float)(Main.time * 0.002);
		timeValue = MathF.Sin(timeValue + ai[1] * 60) + 2;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		float light = 1f;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, new Color(0, 0, pocession), new Vector3(ai[0], timeValue, light)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), new Color(0, 1, pocession), new Vector3(ai[0], timeValue + 0.4f, light)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), new Color(1, 0, pocession), new Vector3(ai[0] + 0.4f, timeValue, light)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), new Color(1, 1, pocession), new Vector3(ai[0] + 0.4f, timeValue + 0.4f, light)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}