using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.VFX.CommonVFXDusts;
internal class JungleSporePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.JungleSpore;
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_JungleSpore.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Point.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}
	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(JungleSporePipeline), typeof(BloomPipeline))]
internal class JungleSporeDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public JungleSporeDust() { }
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
		velocity *= Math.Max(0, 1 - scale * 0.01f);
		velocity += new Vector2(Main.windSpeedCurrent * 0.04f, 0.001f * scale);
		scale *= 0.995f;
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[1]);
		if (Collision.SolidCollision(position, 0, 0))
		{
			velocity *= -0.2f;
			timer += 5;
		}
		var tile = Main.tile[(int)(position.X / 16), (int)(position.Y / 16)];
		if(position.Y % 1 < tile.LiquidAmount / 256f)
		{
			timer += 5;
		}
		if(scale < 0.5f)
		{
			timer += 20;
		}
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.01f;
		Lighting.AddLight(position, c * 0.1f, c * 0.8f, 0);
	}

	public override void Draw()
	{
		float pocession = 1 - timer / maxTime;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner,new Color(0, 0,ai[0], pocession), new Vector3(0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5),new Color(0, 1, ai[0], pocession), new Vector3(0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5),new Color(1, 0 ,ai[0], pocession), new Vector3(0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1),new Color(1, 1, ai[0], pocession), new Vector3(0))
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
