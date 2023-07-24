using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using MythMod.MiscImplementation;
using Terraria.World.Generation;
using MythMod.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles
{
    public class AbyssBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("深渊剑气");
		}
		public override void SetDefaults()
		{
            base.projectile.width = 36;
            base.projectile.height = 36;
            base.projectile.aiStyle = 27;
            base.projectile.friendly = true;
            base.projectile.melee = true;
            base.projectile.ignoreWater = true;
            base.projectile.penetrate = 1;
            base.projectile.extraUpdates = 3;
            base.projectile.timeLeft = 600;
            base.projectile.usesLocalNPCImmunity = true;
            base.projectile.localNPCHitCooldown = 1;
		}
		public override void AI()
		{
			Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0.16f / 255f, (float)(255 - base.projectile.alpha) * 0.06f / 255f, (float)(255 - base.projectile.alpha) * 0.03f / 255f);
			if (base.projectile.localAI[1] > 7f)
			{
				int num = Dust.NewDust((base.projectile.Center - base.projectile.velocity * 4f - new Vector2(4, 4)), 0, 0, mod.DustType("Flame"), base.projectile.velocity.X * 0.5f, base.projectile.velocity.Y * 0.5f, 0, Color.White, 1.3f);
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D = Main.projectileTexture[base.projectile.type];
            spriteBatch.Draw(texture2D, base.projectile.Center - Main.screenPosition, null, base.projectile.GetAlpha(lightColor), base.projectile.rotation, Utils.Size(texture2D) / 2f, base.projectile.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture2D, base.projectile.Center - Main.screenPosition - projectile.velocity * 2.5f, null, base.projectile.GetAlpha(lightColor) * 0.667f, base.projectile.rotation, Utils.Size(texture2D) / 2f, base.projectile.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture2D, base.projectile.Center - Main.screenPosition - projectile.velocity * 5f, null, base.projectile.GetAlpha(lightColor) * 0.333f, base.projectile.rotation, Utils.Size(texture2D) / 2f, base.projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 22; i++)
            {
                int num = Dust.NewDust(base.projectile.position, base.projectile.width, base.projectile.height, mod.DustType("Flame"), base.projectile.velocity.X * 0f, base.projectile.velocity.Y * 0f, 0, Color.White, 1.5f);
                Main.dust[num].velocity = new Vector2(0,Main.rand.NextFloat(0f,6f)).RotatedByRandom(MathHelper.TwoPi);
                Main.dust[num].noGravity = true;
            }
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2f, 2f, base.mod.ProjectileType("AbyssBomb"), projectile.damage, 0, Main.myPlayer, 0f, 0f);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(1f, 1f, 1f, 0.75f));
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 14; i++)
            {
                int num = Dust.NewDust(base.projectile.position, base.projectile.width, base.projectile.height, 127, base.projectile.velocity.X * 0f, base.projectile.velocity.Y * 0f, 0, Color.White, 1.2f);
                Main.dust[num].noGravity = true;
            }
            if (target.type == 488)
            {
                return;
            }
            float num1 = (float)damage * 0.04f;
            if ((int)num1 == 0)
            {
                return;
            }
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
            {
                return;
            }
            Main.player[Main.myPlayer].lifeSteal -= num1;
            int owner = base.projectile.owner;
            target.AddBuff(24, 600);
        }
	}
}
