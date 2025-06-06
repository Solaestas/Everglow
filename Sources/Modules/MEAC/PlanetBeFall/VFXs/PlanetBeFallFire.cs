using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.MEAC.PlanetBeFall.VFXs;

public class PlanetBeFallFirePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.PlanetBeFallFire;
		effect.Value.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_perlin.Value);
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.PlanetBeFallColor.Value);
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

[Pipeline(typeof(PlanetBeFallFirePipeline), typeof(BloomPipeline))]
public class PlanetBeFallFireDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public PlanetBeFallFireDust()
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
		velocity *= 0.9f;
		velocity += new Vector2(Main.windSpeedCurrent * 0.1f, -0.1f);
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
		if (Collision.SolidCollision(position, 0, 0))
		{
			timer++;
		}
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.02f;
		Lighting.AddLight(position, c * 10, c * 10, 0);
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		float timeValue = (float)(Main.time * 0.002);
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		var bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, new Color(0, 0, pocession,  0.1f), new Vector3(ai[0], timeValue, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), new Color(0, 1, pocession,  0.1f), new Vector3(ai[0], timeValue + 0.4f, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), new Color(1, 0, pocession,  0.1f), new Vector3(ai[0] + 0.4f, timeValue, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), new Color(1, 1, pocession, 0.1f), new Vector3(ai[0] + 0.4f, timeValue + 0.4f, 0)),
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}