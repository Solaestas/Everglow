using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles
{
    public class TuskSpice : ModProjectile
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
        Vector2 SP = Vector2.Zero;
        float Omega = 0;
        bool OnPlac = false;
        bool Down = false;
        bool Collid = false;
        int Fra = 0;
        public override void AI()
        {
            if (SP == Vector2.Zero)
            {
                Fra = Main.rand.Next(5);
                SP = Projectile.Center;
            }
            if (!OnPlac)
            {
                Vector2 v = SP + new Vector2(Projectile.ai[0], -320);
                Vector2 v0 = v - Projectile.Center;
                if (Projectile.timeLeft < 360)
                {
                    Projectile.velocity += v0 * (float)Math.Log(v0.Length() / 30f + 1) * 0.005f;
                    if (v0.Length() < 25 && Projectile.velocity.Length() < 2)
                    {
                        OnPlac = true;
                    }
                    float k0 = (float)Math.Log(v0.Length() / 30f + 1);
                    if (k0 > 1.00f)
                    {
                        k0 = 1.00f;
                    }

                    Projectile.velocity *= k0;
                    if (Projectile.velocity.Length() > 20)
                    {
                        Projectile.velocity *= 20f / Projectile.velocity.Length();
                    }
                }

                Projectile.rotation = (float)((Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 2.5d) % MathHelper.TwoPi);
            }
            else
            {
                if (!Down)
                {
                    Omega += (float)(Math.PI - Projectile.rotation) / 23f;
                    Omega *= 0.96f;
                    Projectile.rotation += Omega;
                    Projectile.velocity = new Vector2(0, 0.01f);
                    if (Math.Abs(Math.PI - Projectile.rotation) < 0.05f && Math.Abs(Omega) < 0.05f)
                    {
                        Down = true;
                        Projectile.velocity.Y += 0.25f;
                    }
                }
                else
                {
                    if (!Collid)
                    {
                        Projectile.velocity.Y += 1.5f;
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
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Projectiles/TuskSpice" + Fra.ToString()).Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, colorz, Projectile.rotation, new Vector2(12f, 25f), Projectile.scale, SpriteEffects.None, 0);
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