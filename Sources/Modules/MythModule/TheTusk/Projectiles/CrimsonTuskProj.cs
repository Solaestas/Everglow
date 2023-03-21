using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles
{
    public class CrimsonTuskProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 400;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            Projectile.damage = 0;
        }
        bool Down = false;
        bool Collid = false;
        public override void AI()
        {
            if (Projectile.velocity.Length() < 70f)
            {
                Projectile.velocity *= 1.1f;
            }

            if (!Collid)
            {
                if (Projectile.velocity.Y != 0)
                {
                    Projectile.rotation = (float)((Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 2.5d) % MathHelper.TwoPi);
                }
                if (Collision.SolidCollision(Projectile.Center - Vector2.One * 5f, 10, 10))
                {
                    for (int f = 0; f < 12; f++)
                    {
                        Vector2 vd = new Vector2(0, Main.rand.NextFloat(-7f, -4f)).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f));
                        Dust.NewDust(Projectile.Bottom - new Vector2(4, 4), 0, 0, DustID.Blood, vd.X, vd.Y, 0, default, Main.rand.NextFloat(1f, 2f));
                    }
                    SoundEngine.PlaySound(SoundID.NPCHit18.WithVolumeScale(.8f), Projectile.Center);
                    Collid = true;
                    Projectile.velocity *= 0;
                }
            }
            else
            {
                Projectile.velocity *= 0;
                Projectile.alpha += 5;
                if (Projectile.alpha > 254)
                {
                    Projectile.Kill();
                }
            }
            if (Dam == 0)
            {
                Dam = 60;
                if (Main.expertMode)
                {
                    Dam = 90;
                }
                if (Main.masterMode)
                {
                    Dam = 120;
                }
            }
        }
        int Dam = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {

            Color colorz = Lighting.GetColor((int)(Projectile.Center.X / 16d), (int)(Projectile.Center.Y / 16d));
            colorz = Projectile.GetAlpha(colorz) * ((255 - Projectile.alpha) / 255f);
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Projectiles/CrimsonTuskProj").Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, colorz, Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            if (!Down)
            {
                return;
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            if (Projectile.alpha == 0)
            {
                for (int f = 0; f < Projectile.oldPos.Length; f++)
                {
                    Vector2 vpos = Projectile.oldPos[f] + new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                    Color color = Lighting.GetColor((int)(vpos.X / 16), (int)(vpos.Y / 16));
                    float alpha = 1 - f / (float)Projectile.oldPos.Length;
                    color.R = (byte)(color.R * alpha);
                    color.G = (byte)(color.G * alpha);
                    color.B = (byte)(color.B * alpha);
                    color.A = (byte)(color.A * alpha);
                    Main.spriteBatch.Draw(texture, vpos - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);
                }
            }
        }
    }
}