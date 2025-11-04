using Everglow.Commons.Templates.Weapons;
using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class LanternFlow : TrailingProjectile
{
	public override void SetCustomDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 600;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;
		TrailLength = 400;
		ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 14400;
		TrailColor = new Color(1f, 0.2f, 0f, 0f) * 0.3f;
		TrailBackgroundDarkness = 0.3f;
		TrailWidth = 240f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_2_thick.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_2_black.Value;
		WarpStrength = 1f;
	}

	public NPC OwnerNPC;

	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(1f, 1f, 1f, 0.5f));
	}

	public override void OnSpawn(IEntitySource source)
	{
		if (OwnerNPC == null)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc != null && npc.active)
				{
					if (npc.type == ModContent.NPCType<LanternGhostKing>())
					{
						OwnerNPC = npc;
					}
				}
			}
		}
	}

	public override void Behaviors()
	{
		if (OwnerNPC == null)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc != null && npc.active)
				{
					if (npc.type == ModContent.NPCType<LanternGhostKing>())
					{
						OwnerNPC = npc;
					}
				}
			}
		}
		if (OwnerNPC == null)
		{
			Projectile.active = false;
			return;
		}
		Vector2 toOwner = OwnerNPC.Center - Projectile.Center;
		if (Projectile.timeLeft > 507)
		{
			Projectile.velocity = Projectile.velocity.RotatedBy(MathF.Sin(Projectile.timeLeft * 0.08f) * 0.02f);
		}
		else if (Projectile.timeLeft > 447)
		{
			Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
		}
		else
		{
			Projectile.velocity += Vector2.Normalize(toOwner) * 0.7f;
			if (Projectile.timeLeft < 420)
			{
				Projectile.velocity *= 0.98f;
			}
			if (Projectile.timeLeft < 220)
			{
				TrailColor *= 0.99f;
				TrailBackgroundDarkness *= 0.98f;
				WarpStrength *= 0.98f;
			}
		}
		if (Projectile.timeLeft < 580)
		{
			for (int i = 0; i < SmoothedOldPos.Count / 240; i++)
			{
				int checkOldPosIndex = Main.rand.Next(5, (int)MathF.Min(Timer - 2, SmoothedOldPos.Count - 2));
				checkOldPosIndex = Math.Clamp(checkOldPosIndex, 0, SmoothedOldPos.Count - 1);
				float mulScale = Main.rand.NextFloat(0.5f, 1.2f);
				if (Projectile.timeLeft < 120f)
				{
					mulScale *= Projectile.timeLeft / 120f;
				}
				Vector2 addPos = new Vector2(Main.rand.NextFloat(0f, 90f), 0).RotateRandom(MathHelper.TwoPi);
				var gore2 = new LanternFlow_lantern
				{
					Active = true,
					Visible = true,
					velocity = -Vector2.Normalize(SmoothedOldPos.ToArray()[checkOldPosIndex] - SmoothedOldPos.ToArray()[checkOldPosIndex - 1]) * mulScale * 6 - addPos * 0.01f,
					scale = mulScale,
					position = SmoothedOldPos.ToArray()[checkOldPosIndex] + addPos,
					npcOwner = OwnerNPC,
				};
				Ins.VFXManager.Add(gore2);
			}
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}

	public override void DrawTrail()
	{
		base.DrawTrail();
	}

	public override Color GetTrailColor(int style, Vector2 worldPos, int index, ref float factor, float extraValue0 = 0, float extraValue1 = 0)
	{
		//if(style == 0)
		//{
		//	return new Color(0, 0, 0, 0);
		//}
		return base.GetTrailColor(style, worldPos, index, ref factor, extraValue0, extraValue1);
	}

	public override Vector3 ModifyTrailTextureCoordinate(float factor, float timeValue, float phase, float widthValue)
	{
		float x = factor * 4;
		float y = 1;
		float z = widthValue;
		if (phase == 2)
		{
			y = 0;
		}
		if (phase % 2 == 1)
		{
			y = 0.5f;
		}
		return new Vector3(x, y, z);
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		for (int i = 50; i < Projectile.oldPos.Count(); i++)
		{
			if ((targetHitbox.Center() - (Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) * 0.5f)).Length() < TrailWidth * 0.3f)
			{
				return true;
			}
		}
		return false;
	}
}