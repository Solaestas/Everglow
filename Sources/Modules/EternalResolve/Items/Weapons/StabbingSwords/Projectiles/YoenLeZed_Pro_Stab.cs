using Everglow.Commons.Coroutines;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.Commons.Weapons.StabbingSwords.VFX;
using Everglow.EternalResolve.Buffs;
using SteelSeries.GameSense;
using Terraria.Audio;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class YoenLeZed_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			Color = new Color(100, 180, 255);
			base.SetDefaults();
			TradeShade = 0.2f;
			Shade = 0.2f;
			FadeTradeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.8f;
			MaxLength = 1.15f;
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
        public void GenerateVFX(int Frequency)
        {
            float mulVelocity = Main.rand.NextFloat(0.75f, 1.5f);
            for (int g = 0; g < Frequency; g++)
            {
                float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(8f, 16f));
                Vector2 afterVelocity = Projectile.velocity;
                var electric = new ElectricCurrent
                {
                    velocity = afterVelocity * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
                    maxTime = size * size / 8f,
                    scale = size,
                    ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size / 2, 0 }
                };
                Ins.VFXManager.Add(electric);
            }
        }
        public void SplitVFX(int Frequency)
        {
            float mulVelocity = 1f;
            for (int g = 0; g < Frequency; g++)
            {
                float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(4f, 10f));
                Vector2 afterVelocity = Projectile.velocity*10;
                var electric = new ElectricCurrent
                {
                    velocity = afterVelocity * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * MathF.Sqrt(Main.rand.NextFloat(1f)) * 10,
                    maxTime = size * size / 8f,
                    scale = size,
                    ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size, Main.rand.NextFloat(0.2f, Main.rand.NextFloat(0.2f, 0.4f)) }
                };
                Ins.VFXManager.Add(electric);
            }
        }
        public override void AI()
		{
            //GenerateVFX(1);
            if(Main.rand.NextBool(2))
			SplitVFX(1);
			Lighting.AddLight(Projectile.Center,new Vector3(0.5f,0.8f,1f)*(ToKill/120f));
			base.AI();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Dust d = Dust.NewDustDirect(target.Center, 0, 0, ModContent.DustType<ElectricMiddleDust>(), 0, 0);
			d.scale = Main.rand.NextFloat(0.85f, 1.15f) * 0.1f;
			target.AddBuff(ModContent.BuffType<OnElectric>(), 270);
			base.OnHitNPC(target, hit, damageDone);
		}
		public override void HitTile()
		{
			base.HitTile();
			float scale = (260 - (EndPos - StartCenter).Length()) / 9f;
			for (int g = 0; g < 20; g++)
			{
				float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(8f, 16f));
				Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(12, 15)).RotatedByRandom(6.283);
				var electric = new ElectricCurrent
				{
					velocity = afterVelocity * scale * 0.06f,
					Active = true,
					Visible = true,
					position = EndPos + Vector2.Normalize(StartCenter - EndPos) * 20,
					maxTime = size * size / 8f,
					scale = size,
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size / 2, 0 }
				};
				Ins.VFXManager.Add(electric);
			}
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), EndPos, Vector2.zeroVector, ModContent.ProjectileType<YoenLeZed_Pro_Stab_HitTile>(), 1, 0, Projectile.owner, scale);
		}
	}
}