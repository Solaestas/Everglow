using Terraria.Audio;
namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class DragonScaleHammerProj : MeleeProj
{
	public override void SetDef()
	{
		maxAttackType = 2;
		trailLength = 20;
		shadertype = "Trail";
		longHandle = true;
		Projectile.scale *= 1.1f;
	}
	public override string TrailShapeTex()
	{
		return "Everglow/Yggdrasil/YggdrasilTown/Projectiles/DragonScaleHammerProj_Melee";
	}
	public override string TrailColorTex()
	{
		return "Everglow/Yggdrasil/YggdrasilTown/Projectiles/DragonScaleHammerProj_heatMap";
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.15f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth, float HorizontalHeight, float DrawScale, string GlowPath, double DrawRotation)
	{
		base.DrawSelf(spriteBatch, lightColor, 84, 50, 1f, "");
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 50, 50)).RotatedByRandom(6.283);
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
	public override void Attack()
	{
		Player player = Main.player[Projectile.owner];
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		useTrail = true;
		float timeMul = 1f - GetMeleeSpeed(player) / 100f;

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
			if (timer < 30)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, -1.2f), 0.1f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 20)
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing"));
			if (timer > 30 && timer < 50)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.25f;
				mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
			}

			if (timer > 50 + 20 * timeMul)
			{
				player.fullRotation = 0;
				player.legRotation = 0;
				NextAttackType();
			}
			else
			{
				float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
		}
		if (attackType == 1)
		{
			if (timer < 30)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.1f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == 20)
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing"));
			if (timer > 30 && timer < 50)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.25f;
				mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
			}
			if (timer > 50)
				NextAttackType();
			float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
			player.fullRotation = BodyRotation;
			player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
			player.legRotation = -BodyRotation;
			player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
			Tplayer.HeadRotation = -BodyRotation;
		}
		if (attackType == 2)
		{
			float BodyRotation = 0;
			if (timer < 10)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = +MathHelper.PiOver2 + player.direction * 0.7f;
				mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 100, 0.15f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();

				BodyRotation = (float)Math.Sin((timer - 6) / 8d * Math.PI) * 0.2f * player.direction * player.gravDir;

			}
			if (timer > 10 && timer < 30)
			{
				isAttacking = true;
				Projectile.rotation -= Projectile.spriteDirection * 0.26f;
				mainVec = Projectile.rotation.ToRotationVector2() * 90;

				BodyRotation = -(float)Math.Sin((timer - 22) / 16d * Math.PI) * 0.2f * player.direction * player.gravDir;

			}
			if (timer > 30 && timer < 50)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.25f;
				mainVec = Projectile.rotation.ToRotationVector2() * 130;

				BodyRotation = (float)Math.Sin((timer - 42) / 16d * Math.PI) * 0.2f * player.direction * player.gravDir;

			}
			if (timer < 50)
			{
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}

			if (timer == 1 || timer == 20)
			{
				useTrail = false;
				AttSound(new SoundStyle("Everglow/MEAC/Sounds/TrueMeleeSwing"));
				if (Projectile.owner == Main.myPlayer)
				{
					//寻敌
					NPC target = null;
					float maxdis = 1000;
					foreach (NPC npc in Main.npc)
					{
						if (npc.CanBeChasedBy())
						{
							float dis = Vector2.Distance(Projectile.Center, npc.Center);
							if (dis < maxdis)
							{
								maxdis = dis;
								target = npc;
							}
						}
					}
				}
			}

			if (timer > 55 + 25 * timeMul)
				NextAttackType();
		}
	}
}