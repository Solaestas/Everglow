using Everglow.Commons.MEAC;
using Terraria.Audio;

namespace Everglow.MEAC.Projectiles;

public class VortexVanquisher : MeleeProj, IBloomProjectile
{
	public override void SetDef()
	{
		maxAttackType = 4;
		trailLength = 20;
		longHandle = true;
		shadertype = "Trail";
		AutoEnd = false;
		CanLongLeftClick = true;
	}
	public override string TrailColorTex()
	{
		return "Everglow/MEAC/Images/img_color";
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.1f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.Additive;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 coord0 = default, float DrawScale = 1, Texture2D glowTexture = null)
	{
		base.DrawSelf(spriteBatch, lightColor, diagonal, coord0, DrawScale, glowTexture);
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
		float AddHeadRotation = (float)Math.Atan2(vToMouse.Y, vToMouse.X) + (1 - player.direction) * 1.57f;
		if (player.gravDir == -1)
		{
			if (player.direction == -1)
			{
				if (AddHeadRotation >= 0.57f && AddHeadRotation < 2)
					AddHeadRotation = 0.57f;
			}
			else
			{
				if (AddHeadRotation <= -0.57f)
					AddHeadRotation = -0.57f;
			}
		}
		else
		{
			if (player.direction == -1)
			{
				if (AddHeadRotation >= 2 && AddHeadRotation < 5.71f)
					AddHeadRotation = 5.71f;
			}
			else
			{
				if (AddHeadRotation >= 0.57f)
					AddHeadRotation = 0.57f;
			}
		}

		if (attackType == 0)
		{
			int t = timer;
			if (t < 20)
			{
				useTrail = false;
				LockPlayerDir(Player);
				float targetRot = -MathHelper.PiOver2 - Player.direction * 0.8f;
				mainVec = Vector2.Lerp(mainVec, Player.DirectionTo(MouseWorld_WithoutGravDir) * 150, 0.2f);
				disFromPlayer = MathHelper.Lerp(disFromPlayer, -30, 0.2f);
				Projectile.rotation = mainVec.ToRotation();
			}
			if (t >= 20 && t < 40)
			{
				if (t == 20)
				{
					SoundEngine.PlaySound(SoundID.Item1);
					if (Main.myPlayer == Projectile.owner)
						Projectile.NewProjectile(Player.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec) * 20, ModContent.ProjectileType<DashingLightEff>(), 1, 0, Projectile.owner);
				}
				if (t < 30)
				{
					disFromPlayer += 20;
					isAttacking = true;
				}
				else
				{
					disFromPlayer -= 20;
				}
			}
			if (t > 40)
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
				Tplayer.HeadRotation = -BodyRotation + AddHeadRotation;
			}
		}
		if (attackType == 1)
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
				AttSound(new SoundStyle(
			"Everglow/MEAC/Sounds/TrueMeleeSwing"));
			}
			if (timer > 20 && timer < 35)
			{
				Lighting.AddLight(Projectile.Center + mainVec, 0.9f, 0.6f, 0f);
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.4f;
				mainVec = Vector2Elipse(180, Projectile.rotation, -1.2f);
			}
			if (timer > 40)
				NextAttackType();
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
		if (attackType == 2)
		{
			useTrail = false;
			drawScaleFactor = 0;
			if (timer < 20)
			{
				LockPlayerDir(Player);
				float targetRot = MathHelper.PiOver2 - Player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 180, 0.15f);
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 20)
			{
				for (int i = 0; i < 30; i++)
				{
					int r = 6;
					Vector2 pos = Projectile.Center + Vector2.Lerp(-mainVec, mainVec, i / 30f) - new Vector2(r);
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

			if (timer >= 50)
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
				Tplayer.HeadRotation = -BodyRotation + AddHeadRotation;
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
			if (timer < 120)
			{
				Lighting.AddLight(Projectile.Center + mainVec + Projectile.velocity, 0.9f, 0.6f, 0f);
				if (timer % 10 == 0)
					AttSound(SoundID.Item1);
				CanIgnoreTile = true;
				isAttacking = true;
				Projectile.extraUpdates = 2;
				Projectile.Center += Projectile.velocity;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(MouseWorld_WithoutGravDir - Player.Center) * 180, 0.06f);
				Projectile.rotation += 0.3f * Projectile.spriteDirection;
				mainVec = Projectile.rotation.ToRotationVector2() * 160;
			}

			if (timer > 120)
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

				Vector2 vec = Vector2.Normalize(MouseWorld_WithoutGravDir - Player.Center);
				Projectile.rotation = vec.ToRotation();
				mainVec = vec * 160;
				Player.velocity += Vector2.Normalize(Main.MouseWorld - Player.Center) * 20;
				LockPlayerDir(Player);
				if (Main.myPlayer == Projectile.owner)
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Normalize(mainVec) * 25, ModContent.ProjectileType<DashingLightEff>(), 0, 0, Projectile.owner);
			}
			if (timer < 20)
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
			if (timer > 30)
				Player.velocity *= 0.9f;
			if (timer > 40)
				NextAttackType();
			else if (timer > 1)
			{
				Lighting.AddLight(Projectile.Center, 0.9f, 0.6f, 0f);
				float BodyRotation = (float)Math.Sin((timer - 10) / 30d * Math.PI) * 0.3f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
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

