using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    internal class GlowBeadGun : ModProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/GlowBeadGunTex/GlowBeadGun";
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 350;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }

        private bool Release = true;
        private Vector2 oldPo = Vector2.Zero;
        private int Energy = 0;
        public override void AI()
        {
            Energy++;
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            v0 = Vector2.Normalize(v0);
            if (Main.mouseLeft && Release)
            {
                Projectile.ai[0] *= 0.9f;
                Projectile.ai[1] -= 1f;
                Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 0.25);
                Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0).RotatedBy(Projectile.ai[0] / 0.8d) * (8f - Projectile.ai[0] * 8);
                oldPo = Projectile.Center;
                Projectile.Center = oldPo;
                Projectile.velocity *= 0;
            }
            if (!Main.mouseLeft && Release)
            {
                if (Projectile.ai[1] > 0)
                {
                    Projectile.ai[0] *= 0.9f;
                    Projectile.ai[1] -= 1f;
                    Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0).RotatedBy(Projectile.ai[0] / 4d) * (8f - Projectile.ai[0] * 4);
                }
                else
                {
                    Shoot();
                }
            }
            if(Energy == 180)
            {
                SoundEngine.PlaySound(SoundID.Item36, Projectile.Center);
                Shoot();
            }
        }
        private void Shoot()
        {
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            v0 = Vector2.Normalize(v0);
            Player player = Main.player[Projectile.owner];
            ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
            Gsplayer.FlyCamPosition = new Vector2(0, 2 * Energy).RotatedByRandom(6.283);
            int NumProjectiles = (int)(Energy / 20f) + 1;
            for (int i = 0; i < NumProjectiles; i++)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v0 * 62, v0 * (12 - i) * (Energy + 20) / 180f, ModContent.ProjectileType<Projectiles.GlowStar>(), (int)(Projectile.damage / (i + 19f) * 19f), Projectile.knockBack, player.whoAmI, i * (Energy + 20) / 180f, 0);
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 newVelocity = v0.RotatedByRandom(MathHelper.ToRadians(15));
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);
                Vector2 basePos = Projectile.Center + newVelocity * 3.7f + v0 * 62;
                for (int z = 0; z < 3; z++)
                {
                    Vector2 v = newVelocity * z / 3f;
                    Dust.NewDust(basePos, 0, 0, ModContent.DustType<MothBlue>(), v.X, v.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                    v = newVelocity * (z + 0.5f) / 3f;
                    Dust.NewDust(basePos, 0, 0, ModContent.DustType<MothBlue2>(), v.X, v.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                }
                for (int j = 0; j < 9; j++)
                {
                    Vector2 v = newVelocity / 27f * j;
                    Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
                    int num20 = Dust.NewDust(basePos - new Vector2(8), 0, 0, ModContent.DustType<BlueGlowAppear>(), v1.X, v1.Y, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * 0.4f);
                    Main.dust[num20].noGravity = true;
                }
                for (int j = 0; j < 18; j++)
                {
                    Vector2 v = newVelocity / 54f * j;
                    Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
                    int num21 = Dust.NewDust(basePos - new Vector2(8), 0, 0, ModContent.DustType<BlueParticleDark2>(), v1.X, v1.Y, 100, default(Color), Main.rand.NextFloat(3.7f, 5.1f));
                    Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
                }
            }
            
            Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            if (!Release)
            {
                return;
            }
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Vector2 v0 = Projectile.Center - player.MountedCenter;
            if (Main.mouseLeft)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - Math.PI / 2d));
            }

            Texture2D TexMain = MythContent.QuickTexture("TheFirefly/Projectiles/GlowBeadGunTex/GlowBeadGun");
            Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Projectiles/GlowBeadGunTex/GlowBeadGunGlow");

            Projectile.frame = (int)((Energy % 45) / 5f);
            
           
            Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
            SpriteEffects se = SpriteEffects.None;
            if (Projectile.Center.X < player.Center.X)
            {
                se = SpriteEffects.FlipVertically;
                player.direction = -1;
            }
            else
            {
                player.direction = 1;
            }
            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + (float)(Projectile.ai[0] / -3d) * player.direction, new Vector2(35, 22), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition - new Vector2(0, 6), new Rectangle(0,0,(int)(Energy / 180f * 74f) + 30,TexMainG.Height), new Color(255, 255, 255, 0), Projectile.rotation - (float)(Math.PI * 0.25) + (float)(Projectile.ai[0] / -3d) * player.direction, new Vector2(35, 22), 1f, se, 0);
        }
    }
}
