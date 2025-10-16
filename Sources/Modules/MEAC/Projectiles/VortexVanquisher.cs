using Everglow.Commons.MEAC;
using Everglow.Commons.MEAC.Enums;
using Terraria.Audio;

namespace Everglow.MEAC.Projectiles;

public class VortexVanquisher : MeleeProj, IBloomProjectile
{
	public override void SetDef()
	{
		maxAttackType = 4;
		trailLength = 20;
		longHandle = true;
		shaderType = Commons.MEAC.Enums.MeleeTrailShaderType.ArcBladeTransparentedByZ;;
		AutoEnd = false;
		CanLongLeftClick = true;
	}
	public override string TrailColorTex()
	{
		return Commons.ModAsset.MEAC_Color2_Mod;
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.1f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.Additive;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		drawScale = new Vector2(-0.6f, 1.13f);
		glowTexture = ModAsset.VortexVanquisherGlow.Value;
		if (diagonal == new Vector4())
		{
			diagonal = new Vector4(0, 1, 1, 0);
		}
		if (drawScale == new Vector2())
		{
			drawScale = new Vector2(0, 1);
			if (longHandle)
			{
				drawScale = new Vector2(-0.6f, 1);
			}
		}
		Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
		Vector2 drawCenter = Projectile.Center - Main.screenPosition;

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		DrawVertexByTwoLine(tex, lightColor, diagonal.XY(), diagonal.ZW(), drawCenter + mainVec * drawScale.X, drawCenter + mainVec * drawScale.Y);
		DrawVertexByTwoLine(glowTexture, new Color(1f, 1f, 1f, 0), diagonal.XY(), diagonal.ZW(), drawCenter + mainVec * drawScale.X, drawCenter + mainVec * drawScale.Y);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		// TODO 144
		////伤害倍率
		//ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		//float ShakeStrength = 0.2f;
		//if (attackType == 0)
		//{
		//	damage *= 2;
		//	ShakeStrength = 1f;
		//}
		//if (attackType == 4)
		//{
		//	damage *= 4;
		//	ShakeStrength = 2f;
		//	Player player = Main.player[Projectile.owner];
		//	player.velocity *= -0.05f;
		//}
		//Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12f * ShakeStrength, 100)).RotatedByRandom(6.283);
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
		float addHeadRotation = ((float)Math.Atan2(vToMouse.Y, vToMouse.X) + 6.283f) % 6.283f;

		/*if (player.direction == -1)
		{
			if (addHeadRotation >= 2 && addHeadRotation < 5.71f)
				addHeadRotation = 5.71f;
		}
		else
		{
			if (addHeadRotation >= 0.57f)
				addHeadRotation = 0.57f;
		}*/
		float timeMul = 1f / player.meleeSpeed;

		if (attackType == 0)
		{
			int t = timer;
			if(t == 0)
			{
				player.direction = Projectile.spriteDirection;
			}
			if (t < 4 * timeMul)
			{
				LockPlayerDir(player);
				useTrail = false;
				mainVec = Vector2.Lerp(mainVec, Player.DirectionTo(Main.MouseWorld) * 150, 0.9f / timeMul);
				disFromPlayer = MathHelper.Lerp(disFromPlayer, -30, 0.2f);
				Projectile.rotation = mainVec.ToRotation();
			}
			if (t >= 4 * timeMul && t < 24 * timeMul)
			{
				if (t == (int)(5 * timeMul))
				{
					SoundEngine.PlaySound(SoundID.Item1);
					if (Main.myPlayer == Projectile.owner)
						Projectile.NewProjectile(Player.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec) * 20, ModContent.ProjectileType<DashingLightEff>(), 1, 0, Projectile.owner);
				}
				if (t < 14 * timeMul)
				{
					disFromPlayer += 20;
					isAttacking = true;
				}
				else
				{
					disFromPlayer -= 20;
				}
			}
			if (t > 30 * timeMul)
			{
				disFromPlayer = 6;
				NextAttackType();
			}
			else if (timer > 1)
			{
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.3f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				//Tplayer.HeadRotation = -BodyRotation + addHeadRotation * player.gravDir;
			}
		}
		if (attackType == 1)
		{
			if (timer < 20 * timeMul)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 - Player.direction * 1.2f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(180, targetRot, -1.2f), 0.15f / timeMul);
				mainVec += Projectile.DirectionFrom(Player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(8 * timeMul))
			{
				AttSound(new SoundStyle(
			Commons.ModAsset.TrueMeleeSwing_Mod));
			}
			if (timer > 20 * timeMul && timer < 35 * timeMul)
			{
				Lighting.AddLight(Projectile.Center + mainVec, 0.9f, 0.6f, 0f);
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.4f / timeMul;
				mainVec = Vector2Elipse(180, Projectile.rotation, -1.2f);
			}
			if (timer > 40 * timeMul)
				NextAttackType();
			else if (timer > 1)
			{
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				//Tplayer.HeadRotation = -BodyRotation;
			}
		}
		if (attackType == 2)
		{
			useTrail = false;
			drawScaleFactor = 0;
			if (timer < 20 * timeMul)
			{
				LockPlayerDir(Player);
				float targetRot = MathHelper.PiOver2 - Player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 180, 0.15f / timeMul);
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(20 * timeMul))
			{
				for (int i = 0; i < 30; i++)
				{
					int r = 6;
					Vector2 pos = Projectile.Center + Vector2.Lerp(-mainVec, mainVec, i / 30f / timeMul) - new Vector2(r);
					Dust s = Dust.NewDustDirect(pos, r * 2, r * 2, DustID.AmberBolt);
					s.noGravity = true;
					s.velocity *= 0.1f;
				}
				mainVec = Projectile.rotation.ToRotationVector2() * 0.001f;
				Projectile.Center += new Vector2(Projectile.spriteDirection * 120, 0);
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 mVec = MainVec_WithoutGravDir;
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center - Vector2.Normalize(mVec) * 300, Vector2.Normalize(mVec) * 15, ModContent.ProjectileType<DashingLightEff>(), Projectile.damage, 0, Projectile.owner, 1).CritChance = Projectile.CritChance;
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center - Vector2.Normalize(mVec) * 150, Vector2.Normalize(mVec) * 20, ModContent.ProjectileType<VortexVanquisher2>(), 0, 0, Projectile.owner, 1).scale = Projectile.scale * 1.2f;
				}
			}

			if (timer >= 50 * timeMul)
			{
				NextAttackType();
				drawScaleFactor = 1;
			}
			else if (timer > 1)
			{
				Lighting.AddLight(Projectile.Center + mainVec, 0.9f, 0.6f, 0f);
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.3f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				//Tplayer.HeadRotation = -BodyRotation + addHeadRotation;
			}
		}
		if (attackType == 3)
		{
			if (timer == 1)
			{
				AttSound(SoundID.NPCHit4);
				Projectile.velocity = Vector2.Zero;
				LockPlayerDir(Player);
			}
			if (timer < 120 * timeMul)
			{
				Lighting.AddLight(Projectile.Center + mainVec + Projectile.velocity, 0.9f, 0.6f, 0f);
				if (timer % (int)(10 * timeMul) == 0)
					AttSound(SoundID.Item1);
				CanIgnoreTile = true;
				isAttacking = true;
				Projectile.extraUpdates = 2;
				Projectile.Center += Projectile.velocity;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.MouseWorld - Player.Center) * 180, 0.06f);
				Projectile.rotation += 0.3f * Projectile.spriteDirection / timeMul;
				mainVec = Projectile.rotation.ToRotationVector2() * 160;
				Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Vector2.Normalize(Main.MouseWorld - Player.Center).ToRotation() - 1.57f);
			}

			if (timer > 120 * timeMul)
			{
				CanIgnoreTile = false;
				NextAttackType();
				Projectile.extraUpdates = 1;
			}
		}
		if (attackType == 4)
		{
			if (timer == 1)
			{
				useTrail = false;

				Vector2 vec = Vector2.Normalize(Main.MouseWorld - Player.Center);
				Projectile.rotation = vec.ToRotation();
				mainVec = vec * 160;
				Player.velocity += Vector2.Normalize(Main.MouseWorld - Player.Center) * 20;
				LockPlayerDir(Player);
				if (Main.myPlayer == Projectile.owner)
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec) * 25, ModContent.ProjectileType<DashingLightEff>(), 0, 0, Projectile.owner);
			}
			if (timer < 20 * timeMul)
			{
				if (Player.velocity.Length() > 15)
					Player.velocity *= 0.9f;
				if (timer < 10)
				{
					disFromPlayer += 30;
					isAttacking = true;
				}
				else
				{
					disFromPlayer -= 30;
				}
			}
			if (timer > 30 * timeMul)
				Player.velocity *= 0.9f;
			if (timer > 40 * timeMul)
				NextAttackType();
			else if (timer > 1)
			{
				Lighting.AddLight(Projectile.Center, 0.9f, 0.6f, 0f);
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.3f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				//Tplayer.HeadRotation = -BodyRotation;
			}
		}
	}
	public override void LeftLongThump()
	{
		LockPlayerDir(Player);
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Math.Sign(Main.MouseWorld.X - Projectile.Center.X), 0), ModContent.ProjectileType<VortexVanquisherThump>(), Projectile.damage * 2, 0, Projectile.owner); //Original: Projectile.damage * 6
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

}

