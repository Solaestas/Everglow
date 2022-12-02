﻿using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    public abstract class SlingshotAmmo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 3600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            SetDef();
        }
        public virtual void SetDef()
        {

        }
        public override void OnSpawn(IEntitySource source)
        {
        }
        /// <summary>
        /// 内部变量,别动
        /// </summary>
        internal int TimeTokill = -1;
        /// <summary>
        /// 碰撞长宽,默认10
        /// </summary>
        internal int HitBoxSize = 10;
        /// <summary>
        /// 撞激弹幕
        /// </summary>
        internal int HitProjType = ModContent.ProjectileType<NormalHit>();
        public override void AI()
        {   
            if (TimeTokill >= 0 && TimeTokill <= 2)
            {
                Projectile.Kill();
            }
            if (TimeTokill <= 15 && TimeTokill > 0)
            {
                Projectile.velocity = Projectile.oldVelocity;
            }
            TimeTokill--;
            if(TimeTokill < 0)
            {
                Projectile.velocity.Y += 0.17f;
            }
            else
            {
                if(TimeTokill < 10)
                {
                    Projectile.damage = 0;
                    Projectile.friendly = false;
                }
                Projectile.velocity *= 0f;
            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TimeTokill = 30;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            AmmoHit();
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[0] *= 0.4f;
            Projectile.damage = (int)(Projectile.damage * 0.7);
            Projectile.velocity *= 0.7f;
            AmmoHit();
        }
        public virtual void AmmoHit()
        {
            Projectile.velocity = Projectile.oldVelocity;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + Projectile.velocity, Vector2.Zero, ModContent.ProjectileType<NormalHit>(), Projectile.damage, Projectile.knockBack, Projectile.owner,Projectile.velocity.Length(), Main.rand.NextFloat(6.283f));
            for (int x = 0; x < 5; x++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 0, 0f, 0f, 0, default(Color), 0.7f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TimeTokill > 0)
            {
                return false;
            }
            else
            {
                Texture2D TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
                if(Projectile.type == ModContent.ProjectileType<StarAmmo>())
                {
                    Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, new Color(255,255,120,0), Projectile.rotation, TexMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
                    return false;
                }
                Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, lightColor, Projectile.rotation, TexMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
                return false;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            DrawTrail();
        }
        public virtual void DrawTrail()
        {
            float DrawC = Projectile.ai[0] * Projectile.ai[0];

            List<Vertex2D> bars = new List<Vertex2D>();
            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    if (i == 1)
                    {
                        return;
                    }
                    break;
                }

                TrueL = i;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                float width = 6;
                if (Projectile.timeLeft <= 30)
                {
                    width *= Projectile.timeLeft / 30f;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);

                var factor = i / (float)TrueL;
                var color = Color.Lerp(new Color(DrawC, DrawC, DrawC, 0), new Color(0, 0, 0, 0), factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 1, 0)));

            }

            if (bars.Count > 2)
            {
                Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
    }
}
