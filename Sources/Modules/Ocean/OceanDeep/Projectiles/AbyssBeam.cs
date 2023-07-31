using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using Everglow.Ocean.MiscImplementation;
using Everglow.Ocean.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Everglow.Ocean.OceanDeep.Projectiles
{
    public class AbyssBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("深渊剑气");
		}
		public override void SetDefaults()
		{
            base.Projectile.width = 36;
            base.Projectile.height = 36;
            base.Projectile.aiStyle = 27;
            base.Projectile.friendly = true;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.ignoreWater = true;
            base.Projectile.penetrate = 1;
            base.Projectile.extraUpdates = 3;
            base.Projectile.timeLeft = 600;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 1;
		}
		public override void AI()
		{
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.16f / 255f, (float)(255 - base.Projectile.alpha) * 0.06f / 255f, (float)(255 - base.Projectile.alpha) * 0.03f / 255f);
			if (base.Projectile.localAI[1] > 7f)
			{
				int num = Dust.NewDust((base.Projectile.Center - base.Projectile.velocity * 4f - new Vector2(4, 4)), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Flame>(), base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 0, Color.White, 1.3f);
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
			Main.EntitySpriteDraw(texture2D, base.Projectile.Center - Main.screenPosition, null, base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, Utils.Size(texture2D) / 2f, base.Projectile.scale, SpriteEffects.None, 0f);
			Main.EntitySpriteDraw(texture2D, base.Projectile.Center - Main.screenPosition - Projectile.velocity * 2.5f, null, base.Projectile.GetAlpha(lightColor) * 0.667f, base.Projectile.rotation, Utils.Size(texture2D) / 2f, base.Projectile.scale, SpriteEffects.None, 0f);
			Main.EntitySpriteDraw(texture2D, base.Projectile.Center - Main.screenPosition - Projectile.velocity * 5f, null, base.Projectile.GetAlpha(lightColor) * 0.333f, base.Projectile.rotation, Utils.Size(texture2D) / 2f, base.Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 22; i++)
            {
                int num = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Flame>(), base.Projectile.velocity.X * 0f, base.Projectile.velocity.Y * 0f, 0, Color.White, 1.5f);
                Main.dust[num].velocity = new Vector2(0,Main.rand.NextFloat(0f,6f)).RotatedByRandom(MathHelper.TwoPi);
                Main.dust[num].noGravity = true;
            }
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, 2f, 2f, ModContent.ProjectileType<Everglow.Ocean.OceanDeep.Projectiles.AbyssBomb>(), Projectile.damage, 0, Main.myPlayer, 0f, 0f);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(1f, 1f, 1f, 0.75f));
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 14; i++)
            {
                int num = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 127, base.Projectile.velocity.X * 0f, base.Projectile.velocity.Y * 0f, 0, Color.White, 1.2f);
                Main.dust[num].noGravity = true;
            }
            if (target.type == 488)
            {
                return;
            }
            float num1 = (float)damageDone * 0.04f;
            if ((int)num1 == 0)
            {
                return;
            }
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
            {
                return;
            }
            Main.player[Main.myPlayer].lifeSteal -= num1;
            int owner = base.Projectile.owner;
            target.AddBuff(24, 600);
        }
	}
}
