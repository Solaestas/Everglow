using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.VFX.CommonVFXDusts;

public class IchorSplashPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.IchorSplash;
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uNoise"].SetValue(ModAsset.Noise_cell.Value);
		Texture2D FlameColor = ModAsset.HeatMap_ichorSplash.Value;
		Ins.Batch.BindTexture<Vertex2D>(FlameColor);
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearClamp, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(IchorSplashPipeline), typeof(BloomPipeline))]
public class IchorSplash : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public List<Vector2> oldPos = new List<Vector2>();
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float alpha;
	public IchorSplash() { }

	public override void Update()
	{
		position += velocity * 0.001f;
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			Active = false;
			return;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			Active = false;
			return;
		}
		oldPos.Add(position);
		if (oldPos.Count > 15)
			oldPos.RemoveAt(0);
		velocity.Y += 0.14f;
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[1]);
		scale += 0.4f;
		if (Collision.SolidCollision(position, 0, 0))
		{
			velocity *= 0.2f;
			if(velocity.Length() < 0.02f)
			{
				Active = false;
			}
		}
		if (Main.tile[(int)(position.X / 16f), (int)(position.Y / 16f)].LiquidAmount > 0)
		{
			scale += 0.02f;
			alpha += 0.004f;
			velocity *= 0.9f;
			if(MathF.Abs(velocity.X) > 2)
			{
				velocity.X *= 0.8f;
			}
			velocity += new Vector2(Main.rand.NextFloat(0.5f), 0).RotatedByRandom(6.283) + new Vector2(0, 1.2f - Math.Abs(velocity.X) * alpha * 2f);
			position += velocity * 0.5f;
			timer -= 0.4f;
		}
		else
		{
			position += velocity;
		}
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.04f;
		Lighting.AddLight(position, c * 0.8f, c * 0.4f, 0);
	}

	public override void Draw()
	{
		Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
		float pocession = timer / maxTime;
		int len = pos.Length;
		var bars = new List<Vertex2D>();
		for (int i = 1; i < len; i++)
		{
			Vector2 normal = oldPos[i] - oldPos[i - 1];
			normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);
			float width = scale * (float)Math.Sin(i / (double)len * Math.PI);
			Vector2 pointUp = oldPos[i] + normal * width;
			Vector2 pointDown = oldPos[i] - normal * width;
			Vector2 widthUp = new Vector2(normal.X * width, 0);
			Vector2 widthDown = -new Vector2(normal.X * width, 0);

			if (pointUp.X <= 320 || pointUp.X >= Main.maxTilesX * 16 - 320)
			{
				Active = false;
				return;
			}
			if (pointUp.Y <= 320 || pointUp.Y >= Main.maxTilesY * 16 - 320)
			{
				Active = false;
				return;
			}

			if (pointDown.X <= 320 || pointDown.X >= Main.maxTilesX * 16 - 320)
			{
				Active = false;
				return;
			}
			if (pointDown.Y <= 320 || pointDown.Y >= Main.maxTilesY * 16 - 320)
			{
				Active = false;
				return;
			}

			bars.Add(oldPos[i] + normal * width + widthUp, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0 + ai[0], (i + 15 - len) / 17f, pocession));
			bars.Add(oldPos[i] - normal * width + widthDown, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0.6f + ai[0], (i + 15 - len) / 17f, pocession));
		}
		if (len <= 2)
		{
			for (int i = 1; i < 3; i++)
			{
				var lightColorWithPos = new Color(1f, 1f, 1f, 0);
				bars.Add(position, lightColorWithPos, new Vector3(0, (i + 15 - len) / 75f + timer / 15000f, pocession));
				bars.Add(position, lightColorWithPos, new Vector3(1, (i + 15 - len) / 75f + timer / 15000f, pocession));
			}
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}