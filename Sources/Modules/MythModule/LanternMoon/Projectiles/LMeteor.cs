using Everglow.Sources.Commons.Function.Vertex;
namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles
{
    class LMeteor : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
        }
        float ka = 0;
        Vector2 AIMpos;
        int TrueL = 1;
        public override ModProjectile Clone(Projectile projectile)
        {
            var clone = base.Clone(projectile) as LMeteor;
            ka = 0;
            AIMpos = Vector2.Zero;
            TrueL = 1;
            return clone;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AIMpos = new Vector2(0, -1000);
            ka = 1;
            if (Projectile.timeLeft < 60f)
            {
                ka = Projectile.timeLeft / 60f;
            }
            Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 1.2f / 50f * ka, 0, 0);
            Projectile.velocity *= 0.96f;
            Vector2 v0 = AIMpos + player.Center;
            Vector2 v1 = Vector2.Normalize(v0 - Projectile.Center);
            v1 = (v0 - Projectile.Center + v1 * 60f) / 480f;
            Projectile.velocity += v1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        public override void PostDraw(Color lightColor)
        {
            List<VertexBase.Vertex2D> bars = new List<VertexBase.Vertex2D>();
            int width = 60;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft;
            }
            TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                TrueL++;
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;        
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new VertexBase.Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 1, w)));
                bars.Add(new VertexBase.Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 0, w)));
            }
            List<VertexBase.Vertex2D> Vx = new List<VertexBase.Vertex2D>();
            if (Vx.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new VertexBase.Vertex2D((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
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
            Texture2D t = MythModule.Common.MythContent.QuickTexture("LanternMoon/Projectiles/LMeteor");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
    }
}
