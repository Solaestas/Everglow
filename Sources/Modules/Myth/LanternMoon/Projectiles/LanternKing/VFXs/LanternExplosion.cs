using ReLogic.Content;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing.VFXs;


internal class ExplosionFlamePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModContent.Request<Effect>("Everglow/Myth/LanternMoon/Projectiles/LanternKing/VFXs/ExplosionFlame", AssetRequestMode.ImmediateLoad);
		effect.Value.Parameters["uNoise"].SetValue(ModAsset.ExplosionHive.Value);
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.ExplosionFlame_Color.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D FlameColor = ModAsset.ExplosionTexture.Value;

		Ins.Batch.BindTexture<Vertex2D>(FlameColor);
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}
	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(ExplosionFlamePipeline), typeof(BloomPipeline))]
internal class LanternExplosion : ShaderDraw
{
	public float timer;
	public float maxTime;
	public Vector2[] FireBallVelocity = new Vector2[12];
	public LanternExplosion() { }
	public LanternExplosion(int maxTime, Vector2 position, Vector2 velocity, params float[] ai) : base(position, velocity, ai)
	{
		this.maxTime = maxTime;
	}

	public override void Update()
	{
		position += velocity;

		velocity *= 0f;
		timer++;
		if (timer > maxTime)
			Active = false;

		float delC = ai[1] * ai[1] * ai[1] * 0.17f * (float)Math.Sin((maxTime - timer) / maxTime * Math.PI);
		Lighting.AddLight((int)(position.X / 16), (int)(position.Y / 16), 0.85f * delC, 0.15f * delC, 0.1f * delC);
	}

	public override void Draw()
	{
		float timerValue = timer / maxTime;
		int times = FireBallVelocity.Length;
		var bars = new List<Vertex2D>();
		var drawcRope = new Color(timerValue * timerValue * timerValue * 2, 0.5f, 1, 150 / 255f);
		for (int i = 10; i < times; i++)
		{
			float size = ai[1];
			float iProgress = i / 12f;
			Vector2 maxDis = FireBallVelocity[i] * timer * 0.9f;
			Vector2 normalize = -Vector2.Normalize(FireBallVelocity[i]);
			for (int x = 0;x < 25;x++)
			{
				float progress = x / 24f;
				float sinProgress = MathF.Sin(progress * MathF.PI);
				if (x == 0)
				{
					bars.Add(new Vertex2D(position + maxDis * sinProgress + normalize.RotatedBy(progress * MathHelper.TwoPi) * (sinProgress * (1 - iProgress) + iProgress) * MathF.Sqrt(timer) * (8 - FireBallVelocity[i].Length()) * size, drawcRope, new Vector3(progress * (timer / 350f + 1.2f), 0, 1)));
					bars.Add(new Vertex2D(position + maxDis * sinProgress, drawcRope, new Vector3(progress * (timer / 350f + 1.2f), timer / 90f + 0.1f, 1)));
				}
				bars.Add(new Vertex2D(position + maxDis * sinProgress + normalize.RotatedBy(progress * MathHelper.TwoPi) * (sinProgress * (1 - iProgress) + iProgress) * MathF.Sqrt(timer) * (8 - FireBallVelocity[i].Length()) * size, drawcRope, new Vector3(progress * (timer / 350f + 1.2f), 0, timerValue * timerValue + (12 - i) / 24f)));
				bars.Add(new Vertex2D(position + maxDis * sinProgress, drawcRope, new Vector3(progress * (timer / 350f + 1.2f), timer / 90f + 0.1f, timerValue * timerValue + (12 - i) / 24f)));
				if(x == 24)
				{
					bars.Add(new Vertex2D(position + maxDis * sinProgress + normalize.RotatedBy(progress * MathHelper.TwoPi) * (sinProgress * (1 - iProgress) + iProgress) * MathF.Sqrt(timer) * (8 - FireBallVelocity[i].Length()) * size, drawcRope, new Vector3(progress * (timer / 350f + 1.2f), 0, 1)));
					bars.Add(new Vertex2D(position + maxDis * sinProgress, drawcRope, new Vector3(progress * (timer / 350f + 1.2f), timer / 90f + 0.1f, 1)));
				}
			}
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}