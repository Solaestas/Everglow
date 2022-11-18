using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Shaders;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Function.Curves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Everglow.Sources.Modules.MEACModule;
using System.Linq.Expressions;
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using static Terraria.ModLoader.PlayerDrawLayer;
using Everglow.Sources.Commons.Core.Utils;

namespace Everglow.Sources.Modules.FoodModule.Projectiles
{
    public class FryingPanNonMelee : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 3000;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        private bool Hit = true;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
            if (Projectile.timeLeft > 2850)
            {
                if (Projectile.ai[1] != 0)
                {
                    return true;
                }
                Projectile.soundDelay = 10;
                if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                {
                    Projectile.velocity.X = oldVelocity.X * -1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                {
                    Projectile.velocity.Y = oldVelocity.Y * -1f;
                }
            }
            return false;
        }
        int hittimes = 49;
        public override void AI()
        {
            if (hittimes > 0)
            {
                foreach (Projectile proj in Main.projectile)
                {

                    if (proj.active && proj != Projectile)
                    {
                        if (Projectile.Hitbox.Intersects(proj.Hitbox))
                        {
                            Vector2 v1 = proj.velocity;
                            Vector2 v2 = Projectile.velocity;


                            float m1 = proj.width * proj.height * proj.knockBack * proj.scale;
                            float m2 = Projectile.width * Projectile.height * Projectile.knockBack * Projectile.scale / 100;

                            Vector2 newvelocity1 = (v1 * (m1 - m2) + 2 * m2 * v2) / (m1 + m2);
                            Vector2 newvelocity2 = (v2 * (m2 - m1) + 2 * m1 * v1) / (m1 + m2);
                            Vector2 dustvelocity = newvelocity1 - v1;


                            if (proj.velocity.Length() <= 4)
                            {
                                proj.velocity = newvelocity1;
                                Projectile.velocity = newvelocity2;

                                Dust dust = Dust.NewDustPerfect(Projectile.Center-(Vector2.Normalize(dustvelocity).RotatedBy(Math.PI / 4) * 32), ModContent.DustType<Dusts.FireDust>(), Vector2.Normalize(dustvelocity) * 25, 125, new Color(250, 150, 20));
                                
                            }
                            else
                            {
                                proj.velocity = Vector2.Normalize(newvelocity1) * v1.Length();
                                Projectile.velocity = newvelocity2;//这里是质心动量守恒的弹性碰撞

                                Dust dust = Dust.NewDustPerfect(Projectile.Center - (Vector2.Normalize(dustvelocity).RotatedBy(Math.PI / 4) * 32), ModContent.DustType<Dusts.FireDust>(), Vector2.Normalize(dustvelocity) * 25, 125, new Color(250, 150, 20));

                            }

                            hittimes--;
                        }
                    }
                }
            }

            Player p = Main.player[Projectile.owner];
            Projectile.rotation += 0.15f * Projectile.velocity.Length();
            if (Projectile.timeLeft <= 2850)
            {
                Projectile.velocity = Projectile.velocity * 0.95f + Vector2.Normalize(p.Center - Projectile.Center) * 0.5f;
                Projectile.tileCollide = false;
                if ((p.Center - Projectile.Center).Length() < 40 && Projectile.velocity.Length() < 10)
                {
                    Projectile.timeLeft = 0;
                }
            }


        }



    }
}

