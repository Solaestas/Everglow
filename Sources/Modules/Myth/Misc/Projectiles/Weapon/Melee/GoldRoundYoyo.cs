using Everglow.Commons.DataStructures;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons.Yoyos;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee;

public class GoldRoundYoyo : YoyoProjectile
{
	public override void SetDef()
	{
		MaxRopeLength = 380;
		MaxStaySeconds = 120;
		RotatedSpeed = 0.3f;
		Acceleration = 20;
		//ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		//ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
	}
	private float Timer;
	private float TrailWidth;
	private int HitCounter;
	public float Power;
	public override void OnSpawn(IEntitySource source)
	{
		TrailWidth = 18f;
		HitCounter = 0;
		Power = 0;
		base.OnSpawn(source);
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		if (Power > 400)
		{
			Power *= 0.999f;

			foreach (NPC npc in Main.npc)
			{
				if (Main.rand.NextBool(60))
				{
					if (npc != null && npc.active)
					{
						if (!npc.dontTakeDamage && !npc.friendly)
						{
							if (player.ownedProjectileCounts[ModContent.ProjectileType<FocusRay>()] < 5)
							{
								float distance = (Projectile.Center - npc.Center).Length();
								if (distance < 120)
								{
									List<int> checkNPCList = new List<int>();
									foreach (Projectile proj in Main.projectile)
									{
										if (proj != null && proj.active)
										{
											if (proj.owner == Projectile.owner)
											{
												if (proj.type == ModContent.ProjectileType<FocusRay>())
												{
													FocusRay fcR0 = proj.ModProjectile as FocusRay;
													if (fcR0 != null)
													{
														checkNPCList.Add(fcR0.StickTarget.whoAmI);
													}
												}
											}
										}
									}
									if (!checkNPCList.Contains(npc.whoAmI))
									{
										Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), npc.Center, Vector2.zeroVector, ModContent.ProjectileType<FocusRay>(), Projectile.damage * 2, 0, Projectile.owner);
										FocusRay fcR = p0.ModProjectile as FocusRay;
										if (fcR != null)
										{
											fcR.StickTarget = npc;
											fcR.Yoyo = Projectile;
										}
									}
								}
							}
							else
							{
								break;
							}
						}
					}
				}
			}
		}
		Timer++;
		if (Projectile.ai[0] < 0)
		{
			TrailWidth *= 0.8f;
		}
		Lighting.AddLight(Projectile.Center, 0.5f, 0.2f, 0);
		Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 0, 0, DustID.GemTopaz, 0f, 0f, 100, default, 0.7f);
		d.velocity = new Vector2(0, 2).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
		d.position += d.velocity * 2;
		d.noGravity = true;

		var spark = new RayDustDust
		{
			velocity = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotateRandom(MathHelper.TwoPi),
			Active = true,
			Visible = true,
			position = Projectile.Center,
			maxTime = Main.rand.Next(57, 155),
			scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(1f, 7.0f)),
			rotation = Main.rand.NextFloat(6.283f),
			ai = new float[] { 0, 0 }
		};
		Ins.VFXManager.Add(spark);
		base.AI();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		HitCounter++;
		Power++;
		if (HitCounter >= 6)
		{
			HitCounter = 0;
			for (int x = 0; x < 15; x++)
			{
				Vector2 v0 = new Vector2(Main.rand.NextFloat(3f, 12f), 0).RotatedByRandom(MathHelper.TwoPi);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, v0, ModContent.ProjectileType<GoldRound>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
			}
			for(int x = 0; x < 75; x++)
			{
				var spark = new RayDustDust
				{
					velocity = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotateRandom(MathHelper.TwoPi),
					Active = true,
					Visible = true,
					position = Projectile.Center,
					maxTime = Main.rand.Next(57, 255),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 17.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] {  0 }
				};
				Ins.VFXManager.Add(spark);
			}
		}
		for (int i = 0; i < 15; i++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(1.5f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 0, 0, DustID.GemTopaz, 0f, 0f, 100, default, 1.2f);
			d.velocity *= v;
			d.noGravity = true;
		}
		base.OnHitNPC(target, hit, damageDone);
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return base.Colliding(projHitbox, targetHitbox);
	}
	public override void PostDraw(Color lightColor)
	{
		Texture2D texture = ModAsset.GoldRoundYoyoGlow.Value;
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.7f, 0.1f, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawCorona();
		DrawString();
		DrawCorona2();
		DrawBurningLine();
		Texture2D texture = ModAsset.Melee_GoldRoundYoyo.Value;
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	public override void DrawString(Vector2 to = default)
	{
		Player player = Main.player[Projectile.owner];
		Vector2 mountedCenter = player.MountedCenter;
		Vector2 vector = mountedCenter;
		vector.Y += player.gfxOffY;
		if (to != default)
			vector = to;
		float num = Projectile.Center.X - vector.X;
		float num2 = Projectile.Center.Y - vector.Y;
		Math.Sqrt((double)(num * num + num2 * num2));
		float rotation;
		if (!Projectile.counterweight)
		{
			int num3 = -1;
			if (Projectile.position.X + Projectile.width / 2 < player.position.X + player.width / 2)
				num3 = 1;
			num3 *= -1;
			player.itemRotation = (float)Math.Atan2((double)(num2 * num3), (double)(num * num3));
		}
		bool checkSelf = true;
		if (num == 0f && num2 == 0f)
			checkSelf = false;
		else
		{
			float num4 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			num4 = 12f / num4;
			num *= num4;
			num2 *= num4;
			vector.X -= num * 0.1f;
			vector.Y -= num2 * 0.1f;
			num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
			num2 = Projectile.position.Y + Projectile.height * 0.5f - vector.Y;
		}
		Color color = new Color(0, 0.03f, 0.05f, 1f);
		while (checkSelf)
		{
			float num5 = 12f;
			float num6 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			float num7 = num6;
			if (float.IsNaN(num6) || float.IsNaN(num7))
				checkSelf = false;
			else
			{
				if (num6 < 20f)
				{
					num5 = num6 - 8f;
					checkSelf = false;
				}
				num6 = 12f / num6;
				num *= num6;
				num2 *= num6;
				vector.X += num;
				vector.Y += num2;
				num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
				num2 = Projectile.position.Y + Projectile.height * 0.1f - vector.Y;
				if (num7 > 12f)
				{
					float num8 = 0.3f;
					float num9 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
					if (num9 > 16f)
						num9 = 16f;
					num9 = 1f - num9 / 16f;
					num8 *= num9;
					num9 = num7 / 80f;
					if (num9 > 1f)
						num9 = 1f;
					num8 *= num9;
					if (num8 < 0f)
						num8 = 0f;
					num8 *= num9;
					num8 *= 0.5f;
					if (num2 > 0f)
					{
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
					else
					{
						num9 = Math.Abs(Projectile.velocity.X) / 3f;
						if (num9 > 1f)
							num9 = 1f;
						num9 -= 0.5f;
						num8 *= num9;
						if (num8 > 0f)
							num8 *= 2f;
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
				}
				rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;

				color = Color.Lerp(color, new Color(1f, 0.7f, 0.1f, 0), 0.02f);
				Texture2D tex = TextureAssets.FishingLine.Value;
				Main.spriteBatch.Draw(tex, new Vector2(vector.X - Main.screenPosition.X + tex.Width * 0.5f, vector.Y - Main.screenPosition.Y + tex.Height * 0.5f) - new Vector2(6f, 0f), new Rectangle?(new Rectangle(0, 0, tex.Width, (int)num5)), color, rotation, new Vector2(tex.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
			}
		}
	}
	public void DrawCorona()
	{
		Color drawC = new Color(1f, 0.4f, 0.05f, 0) * (0.5f * TrailWidth / 18f);
		float timeValue = (float)Main.time * 0.0015f + MathF.Sin(Projectile.whoAmI);
		var bars = new List<Vertex2D>();

		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 35).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 25).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 15).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 8).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 10).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_spiderNet.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

	}
	public void DrawCorona2()
	{
		float mulColor = (Power - 300f) / 600f;
		if(mulColor > 1)
		{
			mulColor = 1;
		}
		Color drawC = new Color(1f, 0.4f, 0.05f, 0) * (0.5f * TrailWidth / 18f) * mulColor;
		float timeValue = (float)Main.time * 0.00015f + MathF.Sin(Projectile.whoAmI);
		var bars = new List<Vertex2D>();

		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 120).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 240 * (MathF.Sin(i / 6f * MathF.PI + timeValue * 250) * 0.3f + 1)).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue, 0)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_hiveNet.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		bars = new List<Vertex2D>();

		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 120).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, timeValue + 0.4f, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 180 * (MathF.Sin(i / 6f * MathF.PI + timeValue * 250 + MathF.PI) * 0.3f + 1)).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, timeValue + 0.4f, 0)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_hiveNet.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);


		bars = new List<Vertex2D>();
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f + timeValue * 360;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 120).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, 0.5f, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 150).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, 1f, 0)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_6.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		bars = new List<Vertex2D>();
		for (int i = 0; i <= 120; ++i)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			float factor = i / 120f + timeValue * 360;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 150).RotatedBy(MathHelper.TwoPi * factor), drawC, new Vector3(factor, 0f, 0)));
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 210).RotatedBy(MathHelper.TwoPi * factor), Color.Transparent, new Vector3(factor, 1f, 0)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_SolarSpectrum.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
	public void DrawBurningLine()
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
		Effect effect = Commons.ModAsset.Trailing.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		foreach (Projectile proj in Main.projectile)
		{
			if(proj!= null && proj.active)
			{
				if(proj.owner == Projectile.owner)
				{
					if(proj.type == ModContent.ProjectileType<FocusRay>())
					{
						float distanceProj = (Projectile.Center - proj.Center).Length();
						if (distanceProj <= 120)
						{
							Color drawC = new Color(1f, 0.4f, 0.05f, 0) * (0.5f * TrailWidth / 18f);
							float timeValue = (float)Main.time * 0.05f + MathF.Sin(Projectile.whoAmI);
							Vector2 normalized = Vector2.Normalize(Projectile.Center - proj.Center);
							Vector2 drawPoint = proj.Center;
							Vector2 normalizedLeft = normalized.RotatedBy(MathHelper.PiOver2);
							float drawWidth = Math.Clamp(proj.timeLeft / 30f, 0, 1);
							drawWidth *= 15f;
							var bars = new List<Vertex2D>();
							float oldFactor = 0;
							for (int i = 1; i <= 200; ++i)
							{
								drawPoint += normalized * 3;
								float factor = i / 20f;
								float mulWidth = 1;
								float distance = (drawPoint - Projectile.Center).Length();
								if (distance < 8f)
								{
									mulWidth = distance / 8f;
									mulWidth = MathF.Sqrt(mulWidth);
								}
								bars.Add(new Vertex2D(drawPoint + normalizedLeft * drawWidth, drawC, new Vector3(factor + timeValue, 0, mulWidth)));
								bars.Add(new Vertex2D(drawPoint - normalizedLeft * drawWidth, drawC, new Vector3(factor + timeValue, 1, mulWidth)));
								if (distance < 4)
								{
									break;
								}
								oldFactor = factor;
							}
							bars.Add(new Vertex2D(Projectile.Center + normalizedLeft * drawWidth, drawC, new Vector3(oldFactor + timeValue, 0, 0)));
							bars.Add(new Vertex2D(Projectile.Center - normalizedLeft * drawWidth, drawC, new Vector3(oldFactor + timeValue, 1, 0)));
							Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
							Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
							if (bars.Count > 3)
								Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
						}
						else
						{
							if (proj.timeLeft > 1)
							{
								proj.timeLeft = 1;
							}
						}
					}
				}
			}
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
	public float TrailWidthFunction(float factor)
	{
		factor *= 6;
		factor -= 1;
		if (factor < 0)
		{
			return MathF.Pow(MathF.Cos(factor * MathHelper.PiOver2), 0.5f);
		}
		return MathF.Pow(MathF.Cos(factor / 5f * MathHelper.PiOver2), 3);
	}
}
