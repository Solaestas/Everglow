using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Yggdrasil.Common;
using ReLogic.Content;

namespace Everglow.Yggdrasil.CorruptWormHive.VFXs;

internal abstract class ShaderDraw : Visual
{
	public override CallOpportunity DrawLayer => CallOpportunity.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public ShaderDraw() { }
	public ShaderDraw(Vector2 position, Vector2 velocity, params float[] ai)
	{
		this.position = position;
		this.velocity = velocity;
		this.ai = ai;//可以认为params传入的都是右值，可以直接引用
	}
}

internal class DevilFlamePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModContent.Request<Effect>("Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/VFXs/DevilFlame", AssetRequestMode.ImmediateLoad);
		effect.Value.Parameters["uNoise"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/ExampleModule/VFX/Perlin", AssetRequestMode.ImmediateLoad).Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D FlameColor = YggdrasilContent.QuickTexture("CorruptWormHive/VFXs/DeathSickle_Color");
		VFXManager.spriteBatch.BindTexture<Vertex2D>(FlameColor);
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		VFXManager.spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		VFXManager.spriteBatch.End();
	}
}
[Pipeline(typeof(DevilFlamePipeline), typeof(BloomPipeline))]
internal class DevilFlameDust : ShaderDraw
{
	private Vector2 vsadd = Vector2.Zero;
	public List<Vector2> oldPos = new List<Vector2>();
	public float timer;
	public float maxTime;
	public DevilFlameDust() { }
	public DevilFlameDust(int maxTime, Vector2 position, Vector2 velocity, params float[] ai) : base(position, velocity, ai)
	{
		this.maxTime = maxTime;
	}

	public override void Update()
	{
		position += velocity;
		oldPos.Add(position);
		if (oldPos.Count > 15)
			oldPos.RemoveAt(0);
		velocity *= 0.96f;
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[1]);

		for (int f = oldPos.Count - 1; f > 0; f--)
		{
			if (oldPos[f] != Vector2.Zero)
				oldPos[f] += vsadd;
		}
		float delC = ai[2] * 0.05f * (float)Math.Sin((maxTime - timer) / 40d * Math.PI);
		Lighting.AddLight((int)(position.X / 16), (int)(position.Y / 16), 0.25f * delC, 0f, 0.95f * delC);

	}

	public override void Draw()
	{
		Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
		float fx = timer / maxTime;
		int len = pos.Length;
		if (len <= 2)
			return;
		var bars = new Vertex2D[len * 2 - 1];
		for (int i = 1; i < len; i++)
		{
			Vector2 normal = oldPos[i] - oldPos[i - 1];
			normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);
			var drawcRope = new Color(fx * fx * fx * 2, 0.5f, 1, 50 / 255f);
			float width = ai[2] * (float)Math.Sin(i / (double)len * Math.PI);
			bars[2 * i - 1] = new Vertex2D(oldPos[i] + normal * width, drawcRope, new Vector3(0 + ai[0], i / 80f, 0));
			bars[2 * i] = new Vertex2D(oldPos[i] - normal * width, drawcRope, new Vector3(0.05f + ai[0], i / 80f, 0));
		}
		bars[0] = new Vertex2D((bars[1].position + bars[2].position) * 0.5f, Color.White, new Vector3(0.5f, 0, 0));
		VFXManager.spriteBatch.Draw(bars, PrimitiveType.TriangleStrip);
	}
	public override CallOpportunity DrawLayer => CallOpportunity.PostDrawTiles;
}