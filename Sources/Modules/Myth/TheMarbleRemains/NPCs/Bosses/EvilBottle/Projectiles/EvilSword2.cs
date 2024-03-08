using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
    public class EvilSword2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("邪魔剑气");
        }

        public override void SetDefaults()
        {
            base.Projectile.width = 28;
            base.Projectile.height = 28;
            base.Projectile.aiStyle = 27;
            base.Projectile.friendly = false;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.ignoreWater = true;
            base.Projectile.penetrate = -1;
            Projectile.alpha = 255;
            base.Projectile.extraUpdates = 100;
            base.Projectile.timeLeft = 600;
            base.Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = true;
            base.Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            if (Projectile.alpha > 5)
            {
                Projectile.alpha -= 25;
            }
            else
            {
                Projectile.penetrate = 1;
                Projectile.friendly = true;
            }
            if(Projectile.timeLeft > 120)
            {
                int num90 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) - new Vector2(4, 4) - (base.Projectile.velocity * 3f).RotatedBy(((float)Math.Sin(Main.time / 3d) / 3f)), 4, 4, 27, 0f, 0f, 100, default(Color), 1f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity *= 0.2f;
            }
            else
            {
                int num90 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) - new Vector2(4, 4) - (base.Projectile.velocity * 3f).RotatedBy(((float)Math.Sin(Main.time / 3d) / 3f)), 4, 4, 27, 0f, 0f, 100, default(Color), Projectile.timeLeft / 120f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity *= 0.2f;
            }
            float num20 = base.Projectile.Center.X;
            float num30 = base.Projectile.Center.Y;
            float num4 = 1200f;
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
                }
            }
            if (flag && Projectile.timeLeft % 20 < 10)
            {
                float num8 = 20f;
                Vector2 vector1 = new Vector2(base.Projectile.position.X + (float)base.Projectile.width * 0.5f, base.Projectile.position.Y + (float)base.Projectile.height * 0.5f);
                float num9 = num20 - vector1.X;
                float num10 = num30 - vector1.Y;
                Vector2 v = new Vector2(num20, num30) - Projectile.Center;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.Projectile.velocity.X = (base.Projectile.velocity.X * v.Length() * 2 + num9) / (v.Length() * 2 + 1);
                base.Projectile.velocity.Y = (base.Projectile.velocity.Y * v.Length() * 2 + num10) / (v.Length() * 2 + 1);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, base.Projectile.alpha));
        }
        public override void Kill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(153, 300);
        }
    }
}
