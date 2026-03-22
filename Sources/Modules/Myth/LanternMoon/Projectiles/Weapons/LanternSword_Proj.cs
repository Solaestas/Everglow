using Everglow.Myth.LanternMoon.VFX;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class LanternSword_Proj : MeleeProj_3D
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MeleeProjectiles;

	public NPC NextTarget = null;

	public int NextTargetAvailableTimer = 0;

	public override void OnSpawn(IEntitySource source)
	{
		EnableSphereCoordDraw = false;
		SlashColor = new Color(0.85f, 0.02f, 0.06f, 0);
	}

	public override void SetCustomDefaults()
	{
		Projectile.width = 82;
		Projectile.height = 82;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 5;
		WeaponLength = 84;
	}

	public override void AI()
	{
		base.AI();
		if (NextTargetAvailableTimer > 0)
		{
			NextTargetAvailableTimer--;
		}
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			if (NextTarget is not null && NextTarget.active && NextTargetAvailableTimer > 0)
			{
				Vector2 des = TeleportDestination(NextTarget);
				if (!Collision.SolidCollision(des - new Vector2(8, 16), 16, 16))
				{
					Vector2 slashVel = Vector2.zeroVector;
					if (NextTarget.velocity.X > 0)
					{
						slashVel = new Vector2(12f, 0);
					}
					else
					{
						slashVel = new Vector2(-12f, 0);
					}
					Owner.Center = TeleportDestination(NextTarget);
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Owner.Center, slashVel, ModContent.ProjectileType<LanternSword_Slash>(), Projectile.damage, 2f, Projectile.owner);
					NextTarget = null;
				}
			}
		}
	}

	public Vector2 TeleportDestination(NPC target)
	{
		if(target.velocity.X > 0)
		{
			return target.Left + new Vector2(-20, 0);
		}
		else
		{
			return target.Right + new Vector2(20, 0);
		}
	}

	public void NewAttackSpecial()
	{
		CurrentAttackType = 0;

		// if (Main.MouseWorld.X > Owner.Center.X)
		// {
		// Owner.direction = 1;
		// }
		// else
		// {
		// Owner.direction = -1;
		// }
		Vector2 mouseDir = Main.MouseWorld - Owner.Center;
		mouseDir = mouseDir.SafeNormalize(Vector2.zeroVector);
		AttackTimer = 0;
		float meleeSpeed = Owner.meleeSpeed;
		Attack_RotativeSwing(mouseDir, meleeSpeed);

		var ss = new SoundStyle(Commons.ModAsset.TrueMeleeSword_Mod);
		if (CurrentAttackType % 2 == 1)
		{
			ss = new SoundStyle(Commons.ModAsset.TrueMeleeSwordSwap_Mod);
		}
		SoundEngine.PlaySound(ss.WithPitchOffset(meleeSpeed - 1f + Main.rand.NextFloat(-0.15f, 0.15f)), Projectile.Center);
		float itemUseTime = Owner.HeldItem.useTime;
		float maxTime = itemUseTime / 0.4f;
		AddSlashEffect(maxTime, itemUseTime, (int)(maxTime + 1));
	}

	public override void HitNPCVFX(float hitRotation, Vector2 hitPos)
	{
		LanternSwordHitStar lSHS = new LanternSwordHitStar();
		lSHS.Active = true;
		lSHS.Visible = true;
		lSHS.Position = hitPos;
		lSHS.Rotation = hitRotation;
		lSHS.Scale = 1f;
		lSHS.MaxTime = 12;
		Ins.VFXManager.Add(lSHS);

		for (int k = 0; k < 8; k++)
		{
			LanternSwordHitSpark lSHSp = new LanternSwordHitSpark();
			lSHSp.Active = true;
			lSHSp.Visible = true;
			lSHSp.Position = hitPos;
			lSHSp.Scale = Main.rand.NextFloat(0.1f, 0.3f);
			lSHSp.MaxTime = Main.rand.NextFloat(30, 60);
			lSHSp.Velocity = new Vector2(Main.rand.NextFloat(6, 48), 0).RotatedBy(hitRotation - MathHelper.PiOver2 + Main.rand.NextFloat(-0.5f, 0.5f));
			lSHSp.ai = [Main.rand.NextFloat(MathHelper.TwoPi)];
			Ins.VFXManager.Add(lSHSp);
		}
	}

	public override Color GetTrailColor(int style, Vector2 worldPos, int index, ref float factor, float extraValue0 = 0, float extraValue1 = 0)
	{
		if (style == 3)
		{
			Color drawColor = new Color(1f, 0.7f, 0.1f, 0);
			drawColor *= factor * extraValue0;
			if (!SelfLuminous)
			{
				Color lightC = Lighting.GetColor(worldPos.ToTileCoordinates());
				drawColor.R = (byte)(lightC.R * drawColor.R / 255f);
				drawColor.G = (byte)(lightC.G * drawColor.G / 255f);
				drawColor.B = (byte)(lightC.B * drawColor.B / 255f);
			}
			drawColor *= 1.7f;
			float rot = (worldPos - Projectile.Center).ToRotation() - (float)Main.time * 0.01f;
			float factorHighlight = factor * 3;
			drawColor *= MathF.Max(MathF.Pow(Math.Max(0, 0.5f + 0.5f * MathF.Cos(rot * 2)), 16) * 0.4f * ReflectionSharpValue * MathF.Pow(extraValue0, 2), factorHighlight);
			drawColor.A = 0;
			return drawColor;
		}
		return base.GetTrailColor(style, worldPos, index, ref factor, extraValue0, extraValue1);
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		base.ModifyHitNPC(target, ref modifiers);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);
		if (target.life <= 0)
		{
			float maxDis = 800;
			foreach (var npc in Main.npc)
			{
				if (npc is not null && npc.active && npc != target && !npc.boss)
				{
					if (npc.type == target.type)
					{
						Vector2 toNPC = Owner.Center - npc.Center;
						if (toNPC.Length() < maxDis)
						{
							maxDis = toNPC.Length();
							NextTarget = npc;
							NextTargetAvailableTimer = 180;
						}
					}
				}
			}
		}
	}

	public float MarkRotation = 0;

	public override void PostDraw(Color lightColor)
	{
		if (NextTarget is not null && NextTarget.active && NextTargetAvailableTimer > 0)
		{
			Vector2 des = TeleportDestination(NextTarget);
			if (!Collision.SolidCollision(des - new Vector2(8, 16), 16, 16))
			{
				Texture2D mark = ModAsset.LanternSword_NextTargetMark.Value;
				float timeValue = (float)Main.timeForVisualEffects % 30;
				float rot = 0;
				Color drawColor = new Color(1f, 0f, 0, 0.5f);
				if (timeValue < 10)
				{
					rot = -timeValue * 0.4f;
				}
				if (timeValue is >= 10 and < 20)
				{
					rot = timeValue * 0.01f + 0.1f;
				}
				MarkRotation = MarkRotation * 0.5f + rot * 0.5f;
				SpriteEffects spd = SpriteEffects.None;
				Vector2 origin = Vector2.zeroVector;
				int rotDir = 1;
				Vector2 moveVec = mark.Size() * 0.5f;
				if (NextTarget.velocity.X < 0)
				{
					spd = SpriteEffects.FlipHorizontally;
					origin = new Vector2(mark.Width , 0);
					rotDir = -1;
					moveVec.X *= -1;
				}
				if (timeValue is >= 12 && timeValue < 18)
				{
					drawColor = Color.White;
					float moveValue = (timeValue - 12) / 6f * MathHelper.Pi;
					moveValue = MathF.Sin(moveValue);
					Main.EntitySpriteDraw(mark, NextTarget.Center - Main.screenPosition - moveVec + new Vector2(16 * moveValue, 0), null, drawColor * 0.3f, MarkRotation * rotDir, origin, 0.75f, spd, 0);
					Main.EntitySpriteDraw(mark, NextTarget.Center - Main.screenPosition - moveVec + new Vector2(-16f * moveValue, 0), null, drawColor * 0.3f, MarkRotation * rotDir, origin, 0.75f, spd, 0);
				}
				Main.EntitySpriteDraw(mark, NextTarget.Center - Main.screenPosition - moveVec, null, drawColor, MarkRotation * rotDir, origin, 0.75f, spd, 0);
			}
		}
		base.PostDraw(lightColor);
	}
}