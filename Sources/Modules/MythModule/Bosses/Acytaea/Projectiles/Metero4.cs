﻿using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles
{
    internal class Metero4 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 7;
            Projectile.timeLeft = 110;
            Projectile.tileCollide = false;//ss
            Projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }

        private float ka = 0;
        private Vector2 AIMpos;

        public override void AI()
        {
            Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            AIMpos = new Vector2(-70, 0).RotatedBy(Projectile.timeLeft / 18f * Projectile.ai[0]);
            ka = 1;
            if (Projectile.timeLeft < 60f)
            {
                ka = Projectile.timeLeft / 60f;
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.2f / 50f * ka, 0, 0);
            if (Projectile.timeLeft > 20)
            {
                Projectile.velocity *= 0.98f;
                Vector2 v0 = AIMpos + player.Center;
                Vector2 v1 = Vector2.Normalize(v0 - Projectile.Center);
                v1 = (v0 - Projectile.Center + v1 * 60f) / 1440f;
                Projectile.velocity += v1;
            }
            if (Projectile.timeLeft == 20)
            {
                Projectile.velocity *= 0.0001f;
                for (int a = 0; a < 20; a++)
                {
                    Vector2 va = new Vector2(0, 3).RotatedBy(a / 10d * Math.PI + Math.Sin(a / 5d * Math.PI) / 40d);
                    Projectile.NewProjectile(null, Projectile.Center + va, va, ModContent.ProjectileType<Metero3>(), Projectile.damage, 3, player.whoAmI, -1);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        private int TrueL = 1;

        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            int width = 60;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft;
            }
            TrueL = 0;
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

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 0, w)));
            }
            List<Vertex2D> Vx = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
                Vx.Add(bars[1]);
                Vx.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx.Add(bars[i]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 1]);

                    Vx.Add(bars[i + 1]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 3]);
                }
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Projectiles/Metero").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
    }
}