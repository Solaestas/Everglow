using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Weapons.Other;

public class OceanBlue : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// // base.DisplayName.SetDefault("海蓝");
	}
	public override void SetDefaults()
	{
		base.Projectile.width = 28;
		base.Projectile.tileCollide = false;
		base.Projectile.height = 28;
		base.Projectile.friendly = true;
		base.Projectile.penetrate = 6;
		base.Projectile.timeLeft = 200;
		base.Projectile.DamageType = DamageClass.Melee;
		base.Projectile.aiStyle = 27;
		base.Projectile.scale = 1.5f;
	}
	public override void AI()
	{
		Projectile.velocity *= 0.99f;
		float num = base.Projectile.Center.X;
		float num2 = base.Projectile.Center.Y;
		if (Projectile.timeLeft > 120 && Projectile.timeLeft < 193)
		{
			int num3 = Dust.NewDust(base.Projectile.Center - base.Projectile.velocity * 4f - new Vector2(4, 4), 0, 0, 59, 0, 0, 0, default(Color), 1.6f);
			Main.dust[num3].noGravity = true;
			Main.dust[num3].velocity = new Vector2(0, 0);
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.23f * Projectile.scale / 255f, (float)(255 - base.Projectile.alpha) * 2.55f / 255f * Projectile.scale);
		}
		if (Projectile.timeLeft <= 120)
		{
			int num3 = Dust.NewDust(base.Projectile.Center - base.Projectile.velocity * 4f - new Vector2(4, 4), 0, 0, 59, 0, 0, 0, default(Color), 1.6f * Projectile.timeLeft / 120f);
			Main.dust[num3].noGravity = true;
			Main.dust[num3].velocity = new Vector2(0, 0);
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale * Projectile.timeLeft / 120f, (float)(255 - base.Projectile.alpha) * 0.23f * Projectile.scale / 255f * Projectile.timeLeft / 120f, (float)(255 - base.Projectile.alpha) * 2.55f / 255f * Projectile.scale * Projectile.timeLeft / 120f);
		}
		float num20 = base.Projectile.Center.X;
		float num30 = base.Projectile.Center.Y;
		float num4 = 400f;
		bool flag = false;
		for (int j = 0; j < 200; j++)
		{
			if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
			{
				float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
				float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
				float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
				if (num7 < num4)
				{
					num4 = num7;
					num20 = num5;
					num30 = num6;
					flag = true;
				}
				if (num7 < 50)
				{
					Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), Projectile.knockBack, Projectile.direction, Main.rand.Next(200) > 150 ? true : false);
					Projectile.penetrate--;
				}
			}
		}
		if (flag)
		{
			float num8 = 20f;
			Vector2 vector1 = new Vector2(base.Projectile.position.X + (float)base.Projectile.width * 0.5f, base.Projectile.position.Y + (float)base.Projectile.height * 0.5f);
			float num9 = num20 - vector1.X;
			float num10 = num30 - vector1.Y;
			float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
			num11 = num8 / num11;
			num9 *= num11;
			num10 *= num11;
			base.Projectile.velocity.X = (base.Projectile.velocity.X * 40f + num9) / 41f;
			base.Projectile.velocity.Y = (base.Projectile.velocity.Y * 40f + num10) / 41f;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}
	public override Color? GetAlpha(Color lightColor)
	{
		if (Projectile.timeLeft > 60)
		{
			return new Color?(new Color(255, 255, 255, 0));
		}
		else
		{
			return new Color?(new Color(1 * Projectile.timeLeft / 60f, 1 * Projectile.timeLeft / 60f, 1 * Projectile.timeLeft / 60f, 0));
		}
	}
	// Token: 0x060028C0 RID: 10432 RVA: 0x00208E28 File Offset: 0x00207028
	public override void Kill(int timeLeft)
	{
	}
}
