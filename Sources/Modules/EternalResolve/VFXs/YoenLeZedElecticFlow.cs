using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.EternalResolve.Buffs;
using Everglow.EternalResolve.Common;
using ReLogic.Content;

namespace Everglow.EternalResolve.VFXs;

internal class YoenLeZedElecticFlowPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModContent.Request<Effect>("Everglow/EternalResolve/VFXs/YoenLeZedElecticFlow", AssetRequestMode.ImmediateLoad);
		effect.Value.Parameters["uNoise"].SetValue(ModAsset.HiveCyberNoise.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D FlameColor = ModAsset.YoenLeZedElecticFlow_Color.Value;
		Ins.Batch.BindTexture<Vertex2D>(FlameColor);
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(YoenLeZedElecticFlowPipeline))]
public abstract class YoenLeZedElecticFlow : ShaderDraw
{
	public List<Vector2> oldPos = new List<Vector2>();
	public float timer;
	public float maxTime;
	public YoenLeZedElecticFlow() { }
	public YoenLeZedElecticFlow(int maxTime, Vector2 position, Vector2 velocity, params float[] ai) : base(position, velocity, ai)
	{
		this.maxTime = maxTime;
	}

	public override void Update()
	{
		position += velocity;
		position += Main.player[(int)ai[3]].velocity;
		oldPos.Add(position);
		if (oldPos.Count > 15)
			oldPos.RemoveAt(0);
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[1]);

		ai[2] += 0.4f;
		if (Collision.SolidCollision(position, 0, 0))
		{
			velocity *= 0.2f;
			if(velocity.Length() < 0.02f)
			{
				Active = false;
			}
		}
		for(int x = 0;x < 5;x++)
		{
			int randomHitNPC = Main.rand.Next(Main.npc.Length);
			NPC npc = Main.npc[randomHitNPC];
			if (npc != null)
			{
				if (npc.active)
				{
					if (!npc.buffImmune[ModContent.BuffType<OnElectric>()])
					{
						if ((npc.Center - position).Length() < 40)
						{
							Main.npc[randomHitNPC].AddBuff(ModContent.BuffType<OnElectric>(), 180);
						}
					}
				}
			}
		}
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
			Color light = Lighting.GetColor((int)(oldPos[i].X / 16f), (int)(oldPos[i].Y / 16f));

			var drawcRope = new Color(Math.Min(fx * fx * fx + 0.2f - i / (float)len, 0.8f), light.R / 255f, light.G / 255f, light.B / 255f);
			float width = ai[2] * (float)Math.Sin(i / (double)len * Math.PI);
			bars[2 * i - 1] = new Vertex2D(oldPos[i] + normal * width, drawcRope, new Vector3(0 + ai[0], (i + 15 - len) / 60f + timer / 1500f * velocity.Length(), light.A / 255f));
			bars[2 * i] = new Vertex2D(oldPos[i] - normal * width, drawcRope, new Vector3(0.2f + ai[0], (i + 15 - len) / 60f + timer / 1500f * velocity.Length(), light.A / 255f));
		}
		bars[0] = new Vertex2D((bars[1].position + bars[2].position) * 0.5f, Color.White, new Vector3(0.5f, 0, 0));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
[Pipeline(typeof(YoenLeZedElecticFlowPipeline))]
internal class YoenLeZedElecticFlowDust : YoenLeZedElecticFlow
{
	public override void Update()
	{
		if (Main.tile[(int)(position.X / 16f), (int)(position.Y / 16f)].LiquidAmount > 0)
		{
			if(velocity.Length() < 30)
			{
				velocity = Vector2.Normalize(velocity) * 32f;
			}
			if(Main.rand.NextBool(4))
			{
				timer--;
			}
			if (Main.rand.NextBool(2))
			{
				velocity = velocity.RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
			}
			Vector2 newPos = position + velocity * 2f;
			if(Main.tile[(int)(newPos.X / 16f), (int)(newPos.Y / 16f)].LiquidAmount <= 0)
			{
				velocity.Y *= -1;
			}
		}
		base.Update();
	}
}
[Pipeline(typeof(YoenLeZedElecticFlowPipeline))]
internal class YoenLeZedElecticFlowDust_split : YoenLeZedElecticFlow
{
	public override void Update()
	{
		base.Update();
		if (ai[4] > 0)
		{
			if (Main.rand.NextBool(7))
			{
				GenerateVFX(1);
			}
		}
	}
	public void GenerateVFX(int Frequency)
	{
		float mulVelocity = 1f;
		for (int g = 0; g < Frequency; g++)
		{
			Vector2 afterVelocity = velocity;
			afterVelocity.RotatedBy(Main.rand.NextFloat(-2.3f, 2.3f));
			Vector2 afterPosition = position;
			float ai3 = ai[3];
			float ai4 = ai[4] - 1;
			float mulWidth = 1f;
			var yoenLeZedElecticFlowDust_split = new YoenLeZedElecticFlowDust_split
			{
				velocity = afterVelocity * Main.rand.NextFloat(1.5f, 1.6f) * mulVelocity,
				Active = true,
				Visible = true,
				position = afterPosition,
				maxTime = Main.rand.Next(4, 8),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(26.6f, 28f) * mulWidth, ai3, ai4 }
			};
			Ins.VFXManager.Add(yoenLeZedElecticFlowDust_split);
		}
	}
}
[Pipeline(typeof(YoenLeZedElecticFlowPipeline))]
internal class YoenLeZedElecticFlowDust_split_withoutPlayer : YoenLeZedElecticFlow
{
	public override void Update()
	{
		position += velocity;
		oldPos.Add(position);
		if (oldPos.Count > 15)
			oldPos.RemoveAt(0);
		timer++;
		if (timer > maxTime)
			Active = false;
		velocity = velocity.RotatedBy(ai[1]);

		ai[2] += 0.4f;
		if (Collision.SolidCollision(position, 0, 0))
		{
			velocity *= 0.2f;
			if (velocity.Length() < 0.02f)
			{
				Active = false;
			}
		}
		for (int x = 0; x < 5; x++)
		{
			int randomHitNPC = Main.rand.Next(Main.npc.Length);
			NPC npc = Main.npc[randomHitNPC];
			if (npc != null)
			{
				if (npc.active)
				{
					if (!npc.friendly)
					{
						if (!npc.buffImmune[ModContent.BuffType<OnElectric>()])
						{
							if (!npc.HasBuff(ModContent.BuffType<OnElectric>()))
							{
								if ((npc.Center - position).Length() < 40)
								{
									Main.npc[randomHitNPC].AddBuff(ModContent.BuffType<OnElectric>(), (int)(maxTime - timer) / 2);
								}
							}
						}
					}
				}
			}
		}
	}
}