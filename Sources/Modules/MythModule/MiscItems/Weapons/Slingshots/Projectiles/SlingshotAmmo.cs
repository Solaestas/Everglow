using Everglow.Sources.Commons.Function.Vertex;
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
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 3600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
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
            Projectile.velocity.Y += 0.17f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            AmmoHit();
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            AmmoHit();
        }
        public void AmmoHit()
        {
            Projectile.velocity = Projectile.oldVelocity;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + Projectile.velocity, Vector2.Zero, ModContent.ProjectileType<NormalHit>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 1f, Main.rand.NextFloat(6.283f));
            for (int x = 0; x < 5; x++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 0, 0f, 0f, 0, default(Color), 0.7f);
            }
            TimeTokill = 15;
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TimeTokill > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Projectile.ai[0] = TimeTokill;
            float DrawC = Math.Clamp((Projectile.velocity.Length() - 12) / 24f, 0, 1f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            int TrueL = 1;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                TrueL++;
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
                if (normalDir.Length() < 0.2f)
                {
                    normalDir = Projectile.velocity / Projectile.velocity.Length();
                }
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var color = Color.Lerp(new Color(DrawC, DrawC, DrawC, 0), new Color(0, 0, 0, 0), factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }
            if (bars.Count > 2)
            {
                Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count - 2);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
