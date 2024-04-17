using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.IIID.VFXs;
public class Spark_RockCrackPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.Spark_RockCrack;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.Spark_RockCrack_Heatmap.Value);
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
[Pipeline(typeof(Spark_RockCrackPipeline), typeof(BloomPipeline))]
public class Spark_RockCrackDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public bool noGravity;
	public Spark_RockCrackDust() { }
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
		if(maxTime - timer < 50)
		{
			scale *= 0.9f;
		}
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
		if(scale < 0.5f)
		{
			timer += 20;
		}
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.1f;
		Lighting.AddLight(position, c * 0.66f, c * 0.49f, 0.09f * c);
	}

	public override void Draw()
	{
		float pocession = ai[0] / 3f + 1 / 6f;
		Vector2 toCorner = new Vector2(0, scale * 0.2f).RotatedBy(velocity.ToRotation() - MathHelper.PiOver2);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner + velocity * 6,new Color(0, 0,pocession, 0.0f), new Vector3(0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5) + velocity * 6,new Color(0, 1, pocession, 0.0f), new Vector3(0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5),new Color(1, 0 ,pocession, 0.0f), new Vector3(0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1),new Color(1, 1, pocession, 0.0f), new Vector3(0))
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
