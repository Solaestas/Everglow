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
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Terraria.DataStructures;
using Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

namespace Everglow.Sources.Modules.FoodModule.Projectiles
{
    public class Canhitproj : GlobalProjectile
    {
        public bool Canhit;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Canhit = true;
            base.OnSpawn(projectile, source);
        }
    }
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.timeLeft > 2940)
            {
                if (Projectile.ai[1] != 0)
                {
                    return;
                }
                if (Projectile.velocity.X != Projectile.velocity.X && Math.Abs(Projectile.velocity.X) > 1f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * -1f;
                }
                if (Projectile.velocity.Y != Projectile.velocity.Y && Math.Abs(Projectile.velocity.Y) > 1f)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y * -1f;
                }
                Projectile.timeLeft = 2940;
            }
            return ;
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (Projectile.timeLeft > 2940)
            {
                if (Projectile.ai[1] != 0)
                {
                    return;
                }
                    Projectile.velocity.X = Projectile.velocity.X * -1f;
                    Projectile.velocity.Y = Projectile.velocity.Y * -1f;
                
                Projectile.timeLeft = 2940;
            }
            return ;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
            if (Projectile.timeLeft > 2940)
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
                Projectile.timeLeft = 2940;
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
                    if (proj.active && proj != Projectile && proj.GetGlobalProjectile<Canhitproj>().Canhit && proj.penetrate != -1)
                    {
                        if (Projectile.Hitbox.Intersects(proj.Hitbox))
                        {
                            Vector2 v1 = proj.velocity;
                            Vector2 v2 = Projectile.velocity;

                            float m1 = proj.width * proj.height * proj.knockBack * proj.scale;
                            float m2 = Projectile.width * Projectile.height * Projectile.knockBack * Projectile.scale / 60;

                            Vector2 newvelocity1 = (v1 * (m1 - m2) + 2 * m2 * v2) / (m1 + m2);
                            Vector2 newvelocity2 = (v2 * (m2 - m1) + 2 * m1 * v1) / (m1 + m2);
                            Vector2 dustvelocity = newvelocity1 - v1;

                            proj.velocity = Vector2.Normalize(newvelocity1) * v1.Length();
                            Projectile.velocity = newvelocity2;//这里是质心动量守恒的弹性碰撞

                            Dust dust = Dust.NewDustPerfect(Projectile.Center - (Vector2.Normalize(dustvelocity).RotatedBy(Math.PI / 4) * 32), ModContent.DustType<Dusts.FireDust>(), Vector2.Normalize(dustvelocity) * 15, 125, new Color(250, 150, 20));


                            int dust1 = Dust.NewDust(Projectile.Center - (Vector2.Normalize(dustvelocity).RotatedBy(Math.PI / 4) * 32), 0, 0, ModContent.DustType<MothSmog>(), Vector2.Normalize(dustvelocity).X * 5, Vector2.Normalize(dustvelocity).Y * 10, 100, default, Main.rand.NextFloat(3.7f, 5.1f));
                            Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50);
                            Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);

                            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
                            proj.GetGlobalProjectile<Canhitproj>().Canhit = false;
                            hittimes--;
                        }
                    }
                }
            }

            Player p = Main.player[Projectile.owner];
            Projectile.rotation += 0.5f;
            if (Projectile.timeLeft <= 2940)
            {
                Projectile.velocity = Projectile.velocity * 0.9f + Vector2.Normalize(p.Center - Projectile.Center) * 0.75f;
                Projectile.tileCollide = false;
                if ((p.Center - Projectile.Center).Length() < 32 && Projectile.velocity.Length() < 1)
                {
                    Projectile.timeLeft = 0;
                }
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }
        }
    }
}

