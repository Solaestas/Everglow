using Everglow.Commons.Coroutines;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.Commons.Weapons.StabbingSwords.VFX;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using SteelSeries.GameSense;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class PrisonFireBayonet_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			Color = new Color(255, 120, 0);
			base.SetDefaults();
			TradeShade = 1f;
			Shade = 0.65f;
			FadeShade = 0.84f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 1.40f;
			DrawWidth = 0.4f;
		}
		public override IEnumerator<ICoroutineInstruction> Generate3DRingVFX(Vector2 velocity)
		{
			yield return new WaitForFrames(40);
			StabVFX v = new SelfLightingStabVFX()
			{
				pos = Projectile.Center + Projectile.velocity * MaxLength * 80 * (1 - ToKill / 135f),
				vel = velocity,
				color = Color.Lerp(Color, Color.White, 0.2f),
				scale = 30,
				maxtime = 10,
				timeleft = 10
			};
			if (EndPos == Vector2.Zero)
			{
				Ins.VFXManager.Add(v);
			}
			yield return new WaitForFrames(40);
			v = new SelfLightingStabVFX()
			{
				pos = Projectile.Center + Projectile.velocity * MaxLength * 80 * (1 - ToKill / 135f),
				vel = velocity,
				color = Color.Lerp(Color, Color.White, 0.4f),
				scale = 15,
				maxtime = 10,
				timeleft = 10
			};
			if (EndPos == Vector2.Zero)
			{
				Ins.VFXManager.Add(v);
			}
		}
		public override void DrawEffect(Color lightColor)
		{
			Vector2 normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 50 * ToKill / 120f * DrawWidth;
			Vector2 start = StartCenter;
			Vector2 end = Projectile.Center + Projectile.velocity * 100 * MaxLength;
			if (EndPos != Vector2.Zero)
			{
				end = EndPos;
			}
			float value = (Projectile.timeLeft + ToKill) / 135f;
			Vector2 middle = Vector2.Lerp(end, start, MathF.Sqrt(value) * 0.5f);
			float time = (float)(Main.time * 0.03);
			float dark = MathF.Sin(value * MathF.PI) * 4;
			List<Vertex2D> bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized,new Color(0, 0, 0, 120) * 0.4f * Shade,new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized,new Color(0, 0, 0, 120)* 0.4f* Shade,new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized,Color.White* 0.4f * dark* Shade,new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized,Color.White* 0.4f * dark* Shade,new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized,Color.White* 0.9f * dark* Shade,new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized,Color.White* 0.9f * dark* Shade,new Vector3(0f + time, 1, 1))
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = Commons.ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(0.21f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_black.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
			Color alphaColor = Color;
			alphaColor.A = 0;
			alphaColor.R = alphaColor.R;
			alphaColor.G = alphaColor.G;
			alphaColor.B = alphaColor.B;

			normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 72 * ToKill / 120f * DrawWidth;
			bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized,alphaColor,new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized,alphaColor,new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized,alphaColor * 1.2f,new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized,alphaColor * 1.2f,new Vector3(0f + time, 1, 1))
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = Commons.ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(value * 1.1f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}

			alphaColor.A = 0;
			alphaColor.R = 255;
			alphaColor.G = 255;
			alphaColor.B = 255;
			normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 24 * ToKill / 120f * DrawWidth;
			bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized,alphaColor,new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized,alphaColor,new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized,alphaColor,new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized,alphaColor,new Vector3(0f + time, 1, 1))
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = Commons.ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(value * 0.6f + 0.4f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
		public override void AI()
		{
			if (Main.rand.NextBool(4))
				GenerateVFX(1);
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.8f, 0) * (ToKill / 120f));
			Vector2 pos = Projectile.position + Projectile.velocity * Main.rand.NextFloat(0.4f, 80f);
			Vector2 vel = Projectile.velocity * Main.rand.NextFloat(0f, 4f) + new Vector2(0, Main.rand.NextFloat(5f)).RotatedByRandom(6.283);
			if (Main.rand.NextBool(4))
			{
				if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
				{
					Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<FlameShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.35f, 2f) * (ToKill / 200f));
					dust.velocity = vel;
				}
			}

			base.AI();
		}
		public void GenerateVFX(int Frequency)
		{
			int x = (int)(Projectile.Center.X / 16f);
			int y = (int)(Projectile.Center.Y / 16f);
			if (Main.tile[x, y].LiquidAmount / 16f> Projectile.Center.Y % 16)
			{
				for (int g = 0; g < Frequency; g++)
				{
					Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, -4f);
					;
					var somg = new VaporDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = Projectile.Center,
						maxTime = Main.rand.Next(10, 90),
						scale = Main.rand.NextFloat(20f, 135f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(-0.05f, -0.01f), 0 }
					};
					Ins.VFXManager.Add(somg);
				}
			}
			else
			{
				for (int g = 0; g < Frequency; g++)
				{
					Vector2 newVelocity = Projectile.velocity * Main.rand.NextFloat(8f);
					var fire = new FireDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = Projectile.Center + Projectile.velocity * Main.rand.NextFloat(70, 100f) * (1 - ToKill / 140f),
						maxTime = Main.rand.Next(6, 25) * (ToKill / 100f),
						scale = Main.rand.NextFloat(10f, 50f) * (ToKill / 100f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
					};
					Ins.VFXManager.Add(fire);
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 225);
		}
	}
}