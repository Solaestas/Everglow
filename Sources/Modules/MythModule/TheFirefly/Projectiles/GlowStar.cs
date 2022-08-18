using Terraria.Localization;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class GlowStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 1000;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.993f;
            float k0 = 1f / (Projectile.ai[0] + 2) * 2;
            Vector2 v0 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * Projectile.scale * 0.3f;
            Dust.NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<BlueGlowAppear>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * Projectile.scale * 0.4f * k0);
            if(Projectile.ai[1] > 0)
            {
                Projectile.ai[1] -= 1;
                if (Projectile.ai[1] == 1)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("TheFirefly/Projectiles/GlowStar");
            float k1 = (100f + Projectile.ai[0] * 25) * 0.3f;
            float k0 = (1000 - Projectile.timeLeft) / k1;
            float k2 = 1f;
            if (Projectile.timeLeft <= 1000 - k1)
            {
                k0 = 1;
            }
            if(Projectile.timeLeft < 200)
            {
                k2 = Projectile.timeLeft / 200f;
            }
            Color c0 = new Color(k0 * k0 * 0.3f, k0 * k0 * 0.8f, k0 * 0.8f + 0.2f, 0);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 12;
            float k3 = Projectile.ai[1] / 60f;
            if(Projectile.ai[1] > 0)
            {
                width *= k3;
            }
            width *= (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 3.5f * k2;
            int TrueL = 0;
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
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(factor, 0, w)));
            }
            Texture2D t = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/MothGreyLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c0, Projectile.rotation, Light.Size() / 2f, (k0 / 1.8f + 0.2f) / (Projectile.ai[0] + 3) * 3.5f * k2, SpriteEffects.None, 0);
            return false;
        }
        public override bool PreKill(int timeLeft)
        {
            return true;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item36,Projectile.Center);
            float k0 = 1f / (Projectile.ai[0] + 2) * 2;
            for (int j = 0; j < 8 * k0; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * 0.3f;
                int dust0 = Dust.NewDust(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueGlowAppear>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * Projectile.scale * 0.4f * k0);
                Main.dust[dust0].noGravity = true;
            }
            for (int j = 0; j < 16 * k0; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * 0.3f;
                int dust1 = Dust.NewDust(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueParticleDark2>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(3.7f, 5.1f) * k0);
                Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
            }
        }
    }
}