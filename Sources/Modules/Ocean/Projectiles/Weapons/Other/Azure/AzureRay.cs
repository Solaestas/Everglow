using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Weapons.Other.Azure;

// Token: 0x020005F4 RID: 1524
public class AzureRay : ModProjectile
{
	// Token: 0x06002156 RID: 8534 RVA: 0x0000D493 File Offset: 0x0000B693
	public override void SetStaticDefaults()
	{
		//base.DisplayName.SetDefault("蔚蓝射线");
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x001AC3AC File Offset: 0x001AA5AC
	public override void SetDefaults()
	{
		base.Projectile.width = 6;
		base.Projectile.height = 12;
		base.Projectile.hostile = true;
		base.Projectile.ignoreWater = true;
		base.Projectile.tileCollide = true;
		base.Projectile.alpha = 255;
		base.Projectile.penetrate = (Main.masterMode ? 10 : 1);
		base.Projectile.extraUpdates = 1;
		base.Projectile.timeLeft = 1200;
		CooldownSlot = 1;
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x001791F4 File Offset: 0x001773F4
	public override void AI()
	{
		Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.5f / 255f, (float)(255 - base.Projectile.alpha) * 0f / 255f, (float)(255 - base.Projectile.alpha) * 0.2f / 255f);
		if (base.Projectile.ai[0] == 0f)
		{
			base.Projectile.ai[0] = 1f;
			SoundEngine.PlaySound(SoundID.Item12, new Vector2(base.Projectile.position.X, base.Projectile.position.Y));
		}
		if (base.Projectile.alpha > 0)
		{
			base.Projectile.alpha -= 25;
		}
		if (base.Projectile.alpha < 0)
		{
			base.Projectile.alpha = 0;
		}
		base.Projectile.velocity.RotatedBy(Math.Sin(Projectile.timeLeft / 11f) * 0.3f);

		float num = 50f;
		float num2 = 1.5f;
		if (base.Projectile.ai[1] == 0f)
		{
			base.Projectile.localAI[0] += num2;
			if (base.Projectile.localAI[0] > num)
			{
				base.Projectile.localAI[0] = num;
				return;
			}
		}
		else
		{
			base.Projectile.localAI[0] -= num2;
			if (base.Projectile.localAI[0] <= 0f)
			{
				base.Projectile.Kill();
			}
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		base.Projectile.penetrate--;
		if (base.Projectile.penetrate <= 0)
		{
			base.Projectile.Kill();
		}
		else
		{
			base.Projectile.ai[0] += 0.1f;
			if (base.Projectile.velocity.X != oldVelocity.X)
			{
				base.Projectile.velocity.X = -oldVelocity.X;
			}
			if (base.Projectile.velocity.Y != oldVelocity.Y)
			{
				base.Projectile.velocity.Y = -oldVelocity.Y;
			}
		}
		return false;
	}
	// Token: 0x06002159 RID: 8537 RVA: 0x0000D47A File Offset: 0x0000B67A
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(10, 10, 255, 0));
	}
	// Token: 0x06002159 RID: 8537 RVA: 0x0000D47A File Offset: 0x0000B67A
	// Token: 0x0600215A RID: 8538 RVA: 0x00177FCC File Offset: 0x001761CC
	public override bool PreDraw(ref Color lightColor)
	{
		Color color = Lighting.GetColor((int)((double)base.Projectile.position.X + (double)base.Projectile.width * 0.5) / 16, (int)(((double)base.Projectile.position.Y + (double)base.Projectile.height * 0.5) / 16.0));
		int num = 0;
		int num2 = 0;
		float num3 = (float)(TextureAssets.Projectile[base.Projectile.type].Value.Width - base.Projectile.width) * 0.5f + (float)base.Projectile.width * 0.5f;
		SpriteEffects effects = SpriteEffects.None;
		if (base.Projectile.spriteDirection == -1)
		{
			effects = SpriteEffects.FlipHorizontally;
		}
		Rectangle value = new Rectangle((int)Main.screenPosition.X - 500, (int)Main.screenPosition.Y - 500, Main.screenWidth + 1000, Main.screenHeight + 1000);
		if (base.Projectile.getRect().Intersects(value))
		{
			Vector2 value2 = new Vector2(base.Projectile.position.X - Main.screenPosition.X + num3 + (float)num2, base.Projectile.position.Y - Main.screenPosition.Y + (float)(base.Projectile.height / 2) + base.Projectile.gfxOffY);
			float num4 = 50f;
			float scaleFactor = 1.5f;
			if (base.Projectile.ai[1] == 1f)
			{
				num4 = (float)((int)base.Projectile.localAI[0]);
			}
			for (int i = 1; i <= (int)base.Projectile.localAI[0]; i++)
			{
				Vector2 value3 = Vector2.Normalize(base.Projectile.velocity) * (float)i * scaleFactor;
				Color color2 = base.Projectile.GetAlpha(color);
				color2 *= (num4 - (float)i) / num4;
				color2.A = 0;
				Main.spriteBatch.Draw(TextureAssets.Projectile[base.Projectile.type].Value, value2 - value3, null, color2, base.Projectile.rotation, new Vector2(num3, (float)(base.Projectile.height / 2 + num)), base.Projectile.scale, effects, 0f);
			}
		}
		return false;
	}
}
