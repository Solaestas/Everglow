using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Everglow.Yggdrasil.YggdrasilTown.VFXs.TownNPCAttack;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons;

public class QuenchingBladeProj_Smash : MeleeProj
{
	public override string Texture => ModAsset.QuenchingBladeProj_glow_Mod;

	public Vector2 StartPos = Vector2.zeroVector;

	public Queue<Vector2> PlayerOldPos = new Queue<Vector2>();

	public override void SetDef()
	{
		maxAttackType = 0;
		trailLength = 20;
		longHandle = true;
		AutoEnd = true;
		CanLongLeftClick = true;
		selfWarp = false;
		Omega = 0;
	}

	public override void OnSpawn(IEntitySource source)
	{
		StartPos = Main.player[Projectile.owner].Center;
		base.OnSpawn(source);
	}

	public override string TrailShapeTex()
	{
		return Commons.ModAsset.Melee_Mod;
	}

	public override string TrailColorTex()
	{
		return ModAsset.HeatMap_QuenchingBladeProj_Mod;
	}

	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.3f;
	}

	public override BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}

	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		if (diagonal == default(Vector4))
		{
			diagonal = new Vector4(0, 1, 1, 0);
		}
		if (drawScale == default(Vector2))
		{
			drawScale = new Vector2(0, 1);
			if (longHandle)
			{
				drawScale = new Vector2(-0.6f, 1);
			}
			drawScale *= drawScaleFactor;
		}
		Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
		Texture2D texGlow = ModAsset.QuenchingBladeProj_glow.Value;
		Vector2 drawCenter = Projectile.Center - Main.screenPosition;

		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		DrawVertexByTwoLine(tex, lightColor, diagonal.XY(), diagonal.ZW(), drawCenter + mainVec * drawScale.X, drawCenter + mainVec * drawScale.Y);
		DrawVertexByTwoLine(texGlow, new Color(1f, 1f, 1f, 0), diagonal.XY(), diagonal.ZW(), drawCenter + mainVec * drawScale.X, drawCenter + mainVec * drawScale.Y);

		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		// 伤害倍率
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		float ShakeStrength = 0.2f;
		if (attackType == 0)
		{
			modifiers.FinalDamage *= 1.4f;
			ShakeStrength = 1f;
		}

		Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12f * ShakeStrength, 100)).RotatedByRandom(6.283);
	}

	public float Omega;

	public override void AI()
	{
		base.AI();
		Player player = Main.player[Projectile.owner];
		PlayerOldPos.Enqueue(player.Center - StartPos + mainVec);
		if (PlayerOldPos.Count > trailLength)
		{
			PlayerOldPos.Dequeue();
		}
	}

	public override void Attack()
	{
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		if (Main.myPlayer == Projectile.owner && Main.mouseRight && Main.mouseRightRelease)
		{
		}

		useTrail = true;

		Vector2 vToMouse = Main.MouseWorld - player.Top;
		float AddHeadRotation = (float)Math.Atan2(vToMouse.Y, vToMouse.X) + (1 - player.direction) * 1.57f;
		if (player.gravDir == -1)
		{
			if (player.direction == -1)
			{
				if (AddHeadRotation >= 0.57f && AddHeadRotation < 2)
				{
					AddHeadRotation = 0.57f;
				}
			}
			else
			{
				if (AddHeadRotation <= -0.57f)
				{
					AddHeadRotation = -0.57f;
				}
			}
		}
		else
		{
			if (player.direction == -1)
			{
				if (AddHeadRotation >= 2 && AddHeadRotation < 5.71f)
				{
					AddHeadRotation = 5.71f;
				}
			}
			else
			{
				if (AddHeadRotation >= 0.57f)
				{
					AddHeadRotation = 0.57f;
				}
			}
		}
		if (attackType == 0)
		{
			if (timer < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 - Player.direction * 1.2f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.15f);
				mainVec += Projectile.DirectionFrom(Player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 8)
			{
				AttSound(new SoundStyle(Commons.ModAsset.TrueMeleeSwing_Mod));
			}

			if (timer > 20 && timer < 105)
			{
				if (timer < 20)
				{
					useTrail = false;
					LockPlayerDir(Player);
					float targetRot = -MathHelper.PiOver2 - Player.direction * 2f;
					mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.15f);
					mainVec += Projectile.DirectionFrom(Player.Center) * 3;
					Projectile.rotation = mainVec.ToRotation();
				}
				if (timer == 8)
				{
					AttSound(new SoundStyle(Commons.ModAsset.TrueMeleeSwing_Mod));
				}
				if (timer == 24)
				{
					for (int i = 0; i < 200; i++)
					{
						if (!TileCollisionUtils.PlatformCollision(player.position, player.width, player.height))
						{
							player.position.Y += 16;
						}
						else
						{
							break;
						}
					}
					player.velocity *= 0;
					for (int g = 0; g < 36; g++)
					{
						Vector2 newVelocity = new Vector2(0, -Main.rand.NextFloat(15f, 70f)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
						Vector2 pos = player.Bottom - newVelocity;
						var somg = new Georg_Hammer_JumpHit_Smog_Fire
						{
							velocity = newVelocity,
							Active = true,
							Visible = true,
							position = pos,
							maxTime = Main.rand.Next(55, 148),
							scale = Main.rand.NextFloat(10f, 60f),
							ai = new float[] { 0, 0 },
						};
						Ins.VFXManager.Add(somg);
					}
				}
				Lighting.AddLight(Projectile.Center + mainVec, 0.24f, 0.06f, 0f);
				isAttacking = true;
				if (timer < 30)
				{
					Omega += 0.04f;
				}
				if (timer > 40)
				{
					Omega *= 0.5f;
				}
				Projectile.rotation += Projectile.spriteDirection * Omega;
				mainVec = Vector2Elipse(250, Projectile.rotation, -0f, 0, 1000);
				var lineStart = Vector2.zeroVector;
				var lineEnd = Vector2.zeroVector;
				if (timer is > 25 and < 43)
				{
					lineStart = ProjCenter_WithoutGravDir + Vector2Elipse(250, Projectile.rotation + Main.rand.NextFloat(-0.5f, 0.5f), 0, 0, 1000) * Projectile.scale * 0.9f;
					lineEnd = ProjCenter_WithoutGravDir + Vector2Elipse(250, Projectile.rotation + Main.rand.NextFloat(-0.5f, 0.5f), 0, 0, 1000) * Projectile.scale * 1.2f;
					for (int k = 0; k < 8; k++)
					{
						var dustVFX = new FlameDust0
						{
							velocity = Vector2.zeroVector,
							Active = true,
							Visible = true,
							position = Vector2.Lerp(lineEnd, lineStart, MathF.Sqrt(Main.rand.NextFloat())),
							maxTime = Main.rand.Next(6, 20),
							scale = Main.rand.NextFloat(15, 80),
							rotation = MathHelper.PiOver4 * 3,
							MyOwner = player,
							ai = new float[] { Main.rand.Next(3), 0, 0 },
						};
						Ins.VFXManager.Add(dustVFX);
					}
					for (int k = 0; k < 8; k++)
					{
						lineStart = ProjCenter_WithoutGravDir + Vector2Elipse(250, Projectile.rotation + Main.rand.NextFloat(-0.5f, 0.5f), 0, 0, 1000) * Projectile.scale * 0.9f;
						lineEnd = ProjCenter_WithoutGravDir + Vector2Elipse(250, Projectile.rotation + Main.rand.NextFloat(-0.5f, 0.5f), 0, 0, 1000) * Projectile.scale * 1.2f;
						var dustVFX = new FlameDust1
						{
							velocity = new Vector2(0, -4),
							Active = true,
							Visible = true,
							position = Vector2.Lerp(lineEnd, lineStart, MathF.Sqrt(Main.rand.NextFloat())),
							maxTime = Main.rand.Next(6, 20),
							scale = Main.rand.NextFloat(5, 20),
							rotation = MathHelper.PiOver4 * 3,
							MyOwner = player,
							ai = new float[] { Main.rand.Next(3), 0, 0 },
						};
						Ins.VFXManager.Add(dustVFX);
					}
				}
			}
			if (timer > 110)
			{
				Projectile.Kill();
			}
			else if (timer > 1)
			{
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
		}
	}

	public override void DrawTrail(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(PlayerOldPos.ToList()); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			Vector2 vec = SmoothTrailX[x];

			SmoothTrail.Add(Vector2.Normalize(vec) * (vec.Length() + disFromPlayer));
		}
		if (trailVecs.Count != 0)
		{
			Vector2 vec = trailVecs.ToArray()[trailVecs.Count - 1];

			SmoothTrail.Add(Vector2.Normalize(vec) * (vec.Length() + disFromPlayer));
		}
		Vector2 center = Projectile.Center - Vector2.Normalize(mainVec) * disFromPlayer;
		int length = SmoothTrail.Count;
		if (length <= 3)
		{
			return;
		}

		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			if (i == length - 1)
			{
				factor = 0;
			}
			float w = TrailAlpha(factor);
			Color drawColor = Color.White;
			int invisible = 0;

			// Main.NewText(length);
			if (timer < 24)
			{
				invisible = length;
			}
			if (timer < 44 && timer >= 24)
			{
				invisible = (int)Utils.Lerp(length, 0, MathF.Pow((timer - 24) / 20f, 2));
			}
			if (i < invisible)
			{
				w = 0;
			}
			if (!longHandle)
			{
				bars.Add(new Vertex2D(center + trail[i] * 0.15f * Projectile.scale, drawColor, new Vector3(factor, 1, 0f)));
				bars.Add(new Vertex2D(center + trail[i] * Projectile.scale, drawColor, new Vector3(factor, 0, w)));
			}
			else
			{
				bars.Add(new Vertex2D(center + trail[i] * 0.3f * Projectile.scale, drawColor, new Vector3(factor, 1, 0f)));
				bars.Add(new Vertex2D(center + trail[i] * Projectile.scale, drawColor, new Vector3(factor, 0, w)));
			}
		}

		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
		Effect MeleeTrail = Commons.ModAsset.MeleeTrail.Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Melee2.Value;
		MeleeTrail.Parameters["tex1"].SetValue(Commons.ModAsset.HeatMap_Shadow.Value);
		MeleeTrail.CurrentTechnique.Passes[0].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Melee.Value;
		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes[0].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawSelf(Main.spriteBatch, lightColor);
		DrawTrail(lightColor);
		return false;
	}

	public override void End()
	{
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
		player.fullRotation = 0;
		player.legRotation = 0;
		Tplayer.HeadRotation = 0;
		Tplayer.HideLeg = false;
		player.legPosition = Vector2.Zero;
		Projectile.Kill();
		player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.OnFire, 600);
	}

	public override void DrawWarp(VFXBatch spriteBatch)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList()); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			Vector2 vec = SmoothTrailX[x];

			SmoothTrail.Add(Vector2.Normalize(vec) * (vec.Length() + disFromPlayer));
		}
		if (trailVecs.Count != 0)
		{
			Vector2 vec = trailVecs.ToArray()[trailVecs.Count - 1];

			SmoothTrail.Add(Vector2.Normalize(vec) * (vec.Length() + disFromPlayer));
		}
		Vector2 center = Projectile.Center - Vector2.Normalize(mainVec) * disFromPlayer;
		int length = SmoothTrail.Count;
		if (length <= 3)
		{
			return;
		}

		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1f;
			int invisible = 0;
			if (timer < 22)
			{
				invisible = length;
			}
			if (timer < 42 && timer >= 22)
			{
				invisible = (int)Utils.Lerp(length, 0, MathF.Pow((timer - 22) / 20f, 2));
			}
			if (i < invisible)
			{
				w = 0;
			}
			if (i > length - 20)
			{
				w *= (length - i - 1) / 20f;
			}
			float d = trail[i].ToRotation() + 3.14f + 1.57f;
			if (d > 6.28f)
			{
				d -= 6.28f;
			}

			float dir = d / MathHelper.TwoPi;

			float dir1 = dir;
			if (i > 0)
			{
				float d1 = trail[i - 1].ToRotation() + 3.14f + 1.57f;
				if (d1 > 6.28f)
				{
					d1 -= 6.28f;
				}

				dir1 = d1 / MathHelper.TwoPi;
			}

			if (dir - dir1 > 0.5)
			{
				var midValue = (1 - dir) / (1 - dir + dir1);
				var midPoint = midValue * trail[i] + (1 - midValue) * trail[i - 1];
				midPoint.X = 0;
				var oldFactor = (i - 1) / (length - 1f);
				var midFactor = midValue * factor + (1 - midValue) * oldFactor;
				bars.Add(new Vertex2D(center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
				bars.Add(new Vertex2D(center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
			}
			if (dir1 - dir > 0.5)
			{
				var midValue = (1 - dir1) / (1 - dir1 + dir);
				var midPoint = midValue * trail[i - 1] + (1 - midValue) * trail[i];
				midPoint.X = 0;
				var oldFactor = (i - 1) / (length - 1f);
				var midFactor = midValue * oldFactor + (1 - midValue) * factor;
				bars.Add(new Vertex2D(center - Main.screenPosition, new Color(1, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(1, w, 0, 1), new Vector3(midFactor, 0, 1)));
				bars.Add(new Vertex2D(center - Main.screenPosition, new Color(0, w, 0, 1), new Vector3(midFactor, 1, 1)));
				bars.Add(new Vertex2D(center - Main.screenPosition + midPoint * Projectile.scale * 1.1f, new Color(0, w, 0, 1), new Vector3(midFactor, 0, 1)));
			}
			bars.Add(new Vertex2D(center - Main.screenPosition, new Color(dir, w, 0, 1), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(center - Main.screenPosition + trail[i] * Projectile.scale * 1.1f, new Color(dir, w, 0, 1), new Vector3(factor, 0, w)));
		}

		spriteBatch.Draw(Commons.ModAsset.Noise_melting_H.Value, bars, PrimitiveType.TriangleStrip);
	}
}