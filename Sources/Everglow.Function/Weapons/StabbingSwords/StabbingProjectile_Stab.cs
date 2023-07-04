using Everglow.Commons.MEAC;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;

namespace Everglow.Commons.Weapons.StabbingSwords
{
	public abstract class StabbingProjectile_Stab : ModProjectile, IWarpProjectile
	{
		/// <summary>
		/// 常规颜色
		/// </summary>
		public Color Color = Color.White;
		/// <summary>
		/// 阴影强度
		/// </summary>
		public float Shade = 0f;
		/// <summary>
		/// 重影深度
		/// </summary>
		public float TradeShade = 0f;
		/// <summary>
		/// 重影彩色部分亮度
		/// </summary>
		public float TradeLightColorValue = 0f;
		/// <summary>
		/// 重影大小缩变,小于1
		/// </summary>
		public float FadeScale = 0f;
		/// <summary>
		/// 刀光宽度1
		/// </summary>
		public float DrawWidth = 1f;
		/// <summary>
		/// 重影深度缩变,小于1
		/// </summary>  
		public float FadeTradeShade = 0f;
		/// <summary>
		/// 重影彩色部分亮度缩变,小于1
		/// </summary>
		public float FadeLightColorValue = 0f;
		/// <summary>
		/// 表示刺剑攻击长度,标准长度1
		/// </summary>
		public float MaxLength = 1f;
		public override string Texture => "Everglow/Commons/Weapons/StabbingSwords/StabbingProjectile";
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.netImportant = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 15;
			Projectile.extraUpdates = 5;
			Projectile.tileCollide = false;
		}
		public Vector2 StartCenter = Vector2.zeroVector;
		public Vector2 EndPos = Vector2.zeroVector;
		public int ToKill = 120;
		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 toMouse = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter);
			toMouse.Normalize();
			if (toMouse.HasNaNs())
			{
				toMouse = Vector2.UnitX * player.direction;
			}
			if (toMouse.X != Projectile.velocity.X || toMouse.Y != Projectile.velocity.Y)
			{
				Projectile.netUpdate = true;
			}
			Projectile.velocity = toMouse;
			SoundStyle ss = new SoundStyle("Everglow/Commons/Weapons/StabbingSwords/swordswing");
			SoundEngine.PlaySound(ss, Projectile.Center);
			StartCenter = Projectile.Center;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0;
			Vector2 end = Projectile.Center + Projectile.velocity * 80 * MaxLength;
			if (EndPos != Vector2.zeroVector)
			{
				end = EndPos;
			}
			if (Collision.CanHit(StartCenter, 0, 0, targetHitbox.Center(), 0, 0))
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), StartCenter, end, Projectile.width, ref point))
				{
					return true;
				}
			}
			return false;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			ProduceWaterRipples(new Vector2(Projectile.velocity.Length(), 30));
			if (Projectile.timeLeft <= 1)
			{
				ToKill--;
				if (ToKill > 0)
				{
					Projectile.timeLeft++;
					float value = (Projectile.timeLeft + ToKill) / 135f;
					float BodyRotation = MathF.Sin(value * MathF.PI) * player.direction * 0.5f;
					TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
					Tplayer.HeadRotation = 0;
					Tplayer.HideLeg = true;
					player.headRotation = -BodyRotation;
					Tplayer.HeadRotation = player.headRotation;
					player.fullRotation = BodyRotation;
					player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				}
				else
				{
					TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
					player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
					player.fullRotation = 0;
					player.legRotation = 0;
					Tplayer.HeadRotation = 0;
					Tplayer.HideLeg = false;

					player.legPosition = Vector2.Zero;
				}
			}
			if (ToKill >= 120)
			{
				Projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f + Projectile.velocity * (15 - Projectile.timeLeft) * 2;
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.spriteDirection = Projectile.direction;

			}
			else
			{
				Projectile.extraUpdates = 24;
			}
			Vector2 end = Projectile.Center + Projectile.velocity * 100 * MaxLength;
			if (!Collision.CanHit(StartCenter, 0, 0, end, 0, 0))
			{
				if (EndPos == Vector2.zeroVector)
				{
					EndPos = end;
					HitTile();
				}
			}
		}
		public virtual void HitTile() 
		{
			SoundStyle ss = SoundID.NPCHit4;
			SoundEngine.PlaySound(ss.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), Projectile.Center);	
			for (int g = 0; g < 20; g++)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
				var spark = new FireSpark_MetalStabDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = EndPos,
					maxTime = Main.rand.Next(1, 25),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(10f, 27.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.13f, 0.13f) }
				};
				Ins.VFXManager.Add(spark);
			}
		}
		private void ProduceWaterRipples(Vector2 beamDims)
		{
			Vector2 mainVec = Projectile.velocity;
			var shaderData = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
			float waveSine = 1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
			Vector2 ripplePos = Projectile.Center + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(mainVec.ToRotation());
			Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
			shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, mainVec.ToRotation());
		}
		public override bool PreDraw(ref Color lightColor)
		{
			DrawItem(lightColor);
			return false;
		}
		public virtual void DrawItem(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			if (ToKill > 60)
			{
				Texture2D itemTexture = TextureAssets.Item[player.HeldItem.type].Value;
				Main.spriteBatch.Draw(itemTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathF.PI * 0.25f, itemTexture.Size() / 2f, 1, SpriteEffects.None, 0f);
			}
		}
		public override void PostDraw(Color lightColor)
		{
			DrawEffect(lightColor);
		}
		public virtual void DrawEffect(Color lightColor)
		{
			Vector2 normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 50 * ToKill / 120f * DrawWidth;
			Vector2 start = StartCenter;
			Vector2 end = Projectile.Center + Projectile.velocity * 100 * MaxLength;
			if(EndPos != Vector2.Zero)
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
				Effect effect = ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(0.21f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = ModAsset.Trail_black.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
			Color alphaColor = Color;
			alphaColor.A = 0;
			alphaColor.R = (byte)(alphaColor.R * lightColor.R / 255f);
			alphaColor.G = (byte)(alphaColor.G * lightColor.G / 255f);
			alphaColor.B = (byte)(alphaColor.B * lightColor.B / 255f);

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
				Effect effect = ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(value * 1.1f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = ModAsset.Trail_1.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}

			alphaColor.A = 0;
			alphaColor.R = (byte)(565 * lightColor.R / 255f);
			alphaColor.G = (byte)(565 * lightColor.G / 255f);
			alphaColor.B = (byte)(565 * lightColor.B / 255f);
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
				Effect effect = ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(value * 0.6f + 0.4f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = ModAsset.Trail.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
		public void DrawWarp(VFXBatch sb)
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
			Color alphaColor = Color;
			alphaColor.A = 0;
			alphaColor.R = (byte)(((Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 6.283 + Math.PI) % 6.283) / 6.283 * 255);
			alphaColor.G = 120;

			List<Vertex2D> bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized- Main.screenPosition,new Color(alphaColor.R, 0, 0, 0),new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized- Main.screenPosition,new Color(alphaColor.R, 0, 0, 0),new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized- Main.screenPosition,alphaColor,new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized- Main.screenPosition,alphaColor,new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end - Main.screenPosition,alphaColor,new Vector3(0f + time, 0.5f, 1)),
				new Vertex2D(end - Main.screenPosition,alphaColor,new Vector3(0f + time, 0.5f, 1))
			};
			sb.Draw(ModAsset.Trail_1.Value,bars, PrimitiveType.TriangleStrip);
		}
	}
}
