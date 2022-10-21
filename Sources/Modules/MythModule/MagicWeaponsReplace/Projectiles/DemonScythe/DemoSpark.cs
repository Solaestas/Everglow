using Terraria.Localization;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DemonScythe
{
    public class DemoSpark : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 15;
            Projectile.timeLeft = 200;
            Projectile.alpha = 0;
            Projectile.penetrate = -1; 
            Projectile.scale = 1f;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        Vector2[,] SparkOldPos = new Vector2[27, 40];
        Vector2[] SparkVelocity = new Vector2[27];
        public override void AI()
        {
            int MaxC = (int)(Projectile.ai[0]);
            MaxC = Math.Min(26, MaxC);
            if (Projectile.timeLeft >= 200)
            {
                for (int x = 0; x < MaxC; x++)
                {
                    SparkVelocity[x] = new Vector2(0, Projectile.ai[0] * 2f).RotatedByRandom(6.283) * Main.rand.NextFloat(0.05f,1.2f);
                    SparkOldPos[x, 0] = Projectile.Center;
                }
            }

            for(int x = 0;x < MaxC; x++)
            {
                for (int y = 39;y > 0;y--)
                {
                    SparkOldPos[x, y] = SparkOldPos[x, y - 1];
                }
                if (Collision.SolidCollision(SparkOldPos[x, 0] + new Vector2(SparkVelocity[x].X, 0), 0, 0))
                {
                    SparkVelocity[x].X *= -0.95f;
                }
                if (Collision.SolidCollision(SparkOldPos[x, 0] + new Vector2(0, SparkVelocity[x].Y), 0, 0))
                {
                    SparkVelocity[x].Y *= -0.95f;
                }
                SparkOldPos[x, 0] += SparkVelocity[x];

                if (SparkVelocity[x].Length() > 0.3f)
                {
                    SparkVelocity[x] *= 0.95f;
                }
                SparkVelocity[x].Y += 0.001f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            DrawSpark(Color.White, Math.Min(Projectile.timeLeft / 8f, 20f), Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkDark"));
            DrawSpark(new Color(131, 0, 255, 0), Math.Min(Projectile.timeLeft / 8f, 20f), Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkLight"));



            return false;
        }
        private void DrawSpark(Color c0, float width, Texture2D tex)
        {
            int MaxC = (int)(Projectile.ai[0]);
            MaxC = Math.Min(26, MaxC);
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int x = 0; x < MaxC; x++)
            {

                int TrueL = 0;
                for (int i = 1; i < 40; ++i)
                {
                    if (SparkOldPos[x, i] == Vector2.Zero)
                    {
                        break;
                    }

                    TrueL++;
                }
                for (int i = 1; i < 40; ++i)
                {
                    if (SparkOldPos[x, i] == Vector2.Zero)
                    {
                        break;
                    }

                    var normalDir = SparkOldPos[x, i - 1] - SparkOldPos[x, i];
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    var factor = i / (float)TrueL;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    float x0 = 1 - factor;
                    if (i == 1)
                    {
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
                    }
                    bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
                    bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
                    if(i == 39)
                    {
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
                        bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
                    }
                }
                Texture2D t = tex;
                Main.graphics.GraphicsDevice.Textures[0] = t;

            }
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        public override bool PreKill(int timeLeft)
        {
            return true;
        }
        public override void Kill(int timeLeft)
        {
            
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.98f;

            return false;
        }
    }
}