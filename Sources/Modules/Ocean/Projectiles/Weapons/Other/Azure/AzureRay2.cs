using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Weapons.Other.Azure;

// Token: 0x020005F4 RID: 1524
public class AzureRay2 : ModProjectile
{
	public static Texture2D[] projectileTexture = new Texture2D[714];
	// Token: 0x06002156 RID: 8534 RVA: 0x0000D493 File Offset: 0x0000B693
	public override void SetStaticDefaults()
	{
		//base.DisplayName.SetDefault("蔚蓝射线");
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x001AC3AC File Offset: 0x001AA5AC
	public override void SetDefaults()
	{
		Projectile.width = 12;
		Projectile.height = 24;
		Projectile.hostile = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = true;
		Projectile.alpha = 255;
		Projectile.penetrate = (Main.masterMode ? 10 : 1);
		Projectile.extraUpdates = 1;
		Projectile.timeLeft = 1200;
		CooldownSlot = 1;
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x001791F4 File Offset: 0x001773F4
	public override void AI()
	{
		Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.5f / 255f, (float)(255 - Projectile.alpha) * 0f / 255f, (float)(255 - Projectile.alpha) * 0.2f / 255f);
		if (Projectile.ai[0] == 0f)
		{
			Projectile.ai[0] = 1f;
			SoundEngine.PlaySound(SoundID.Item12, Projectile.position);
		}
		if (Projectile.alpha > 0)
		{
			Projectile.alpha -= 25;
		}
		if (Projectile.alpha < 0)
		{
			Projectile.alpha = 0;
		}
		Projectile.velocity.RotatedBy(Math.Sin(Projectile.timeLeft / 11f) * 0.3f);

		float num = 90f;
		float num2 = 1.5f;
		if (Projectile.ai[1] == 0f)
		{
			Projectile.localAI[0] += num2;
			if (Projectile.localAI[0] > num)
			{
				Projectile.localAI[0] = num;
				return;
			}
		}
		else
		{
			Projectile.localAI[0] -= num2;
			if (Projectile.localAI[0] <= 0f)
			{
				Projectile.Kill();
			}
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.penetrate--;
		if (Projectile.penetrate <= 0)
		{
			Projectile.Kill();
		}
		else
		{
			Projectile.ai[0] += 0.1f;
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			int num2 = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Azure.AzureRayBump>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[num2].rotation = Main.rand.Next(0, 62800) / 10000f;
			Main.projectile[num2].scale = Main.rand.Next(20000, 25000) / 10000f;
		}
		return false;
	}
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(20, 20, 255, 0));
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Color color = Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0));
		int num = 0;
		int num2 = 0;
		float num3 = (float)(projectileTexture[Projectile.type].Width - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
		SpriteEffects effects = SpriteEffects.None;
		if (Projectile.spriteDirection == -1)
		{
			effects = SpriteEffects.FlipHorizontally;
		}
		Rectangle value = new Rectangle((int)Main.screenPosition.X - 500, (int)Main.screenPosition.Y - 500, Main.screenWidth + 1000, Main.screenHeight + 1000);
		if (Projectile.getRect().Intersects(value))
		{
			Vector2 value2 = new Vector2(Projectile.position.X - Main.screenPosition.X + num3 + (float)num2, Projectile.position.Y - Main.screenPosition.Y + (float)(Projectile.height / 2) + Projectile.gfxOffY);
			float num4 = 50f;
			float scaleFactor = 1.5f;
			if (Projectile.ai[1] == 1f)
			{
				num4 = (float)((int)Projectile.localAI[0]);
			}
			for (int i = 1; i <= (int)Projectile.localAI[0]; i++)
			{
				Vector2 value3 = Vector2.Normalize(Projectile.velocity) * (float)i * scaleFactor;
				Color color2 = Projectile.GetAlpha(color);
				color2 *= (num4 - (float)i) / num4;
				color2.A = 0;
				Main.EntitySpriteDraw(projectileTexture[Projectile.type], value2 - value3, null, color2, Projectile.rotation, new Vector2(num3, (float)(Projectile.height / 2 + num)), Projectile.scale, effects, 0f); // TODO, try with Main.spriteBatch and see if it works better
			}
		}
		return false;
	}
}