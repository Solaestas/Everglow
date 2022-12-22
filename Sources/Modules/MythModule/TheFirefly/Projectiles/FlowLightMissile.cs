using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    internal class FlowLightMissile : ModProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/FlowLightMissile";

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
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft % 5 == 0)
            {
                player.statMana--;
            }
            Energy+=2;
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            v0 = Vector2.Normalize(v0);
            if (Main.mouseLeft && Release)
            {
                Projectile.ai[0] *= 0.9f;
                Projectile.ai[1] -= 1f;
                Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 7 / 12);
                if(player.direction == -1)
                {
                    Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) - Math.PI * 1 / 12);
                }
                Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0).RotatedBy(Projectile.ai[0] / 0.8d) * (8f - Projectile.ai[0] * 8) + new Vector2(0, 0);
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
            if (Energy >= 450 ||player.statMana <= 0)
            {
                Shoot();
            }
            if (Energy >= 180)
            {
                player.velocity += v0;
                if(player.velocity.Length() > 20f)
                {
                    player.velocity *= 20f / player.velocity.Length();
                }
                Vector2 HitPoint = player.Center + v0 * 105;
                if(Collision.SolidCollision(HitPoint,0,0))
                {
                    HitToAnything();
                }
                foreach (var target in Main.npc)
                {
                    if (target.active)
                    {
                        if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy())
                        {
                            if(Rectangle.Intersect(target.Hitbox, new Rectangle((int)(HitPoint.X), (int)(HitPoint.Y), 0, 0)) != new Rectangle(0,0,0,0))
                            {
                                HitToAnything();
                            }
                        }
                    }
                }
            }
        }
        private void HitToAnything()
        {
            Player player = Main.player[Projectile.owner];
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
            Gsplayer.FlyCamPosition = new Vector2(0, 30).RotatedByRandom(6.283);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BombShakeWave>(), 0, 0, Projectile.owner, 2, 6f);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BeadShakeWave>(), 0, 0, Projectile.owner, 4f);
            foreach (NPC target in Main.npc)
            {
                float Dis = (target.Center - Projectile.Center).Length();

                if (Dis < 350)
                {
                    if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
                    {
                        bool crit = Main.rand.NextBool(33, 100);
                        target.StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(1.70f,2.30f)), 2f, 1, crit);

                        player.addDPS(Math.Max(0, target.defDamage));
                    }
                }
            }
            for (int h = 0; h < 200; h += 3)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.8f, 2.4f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.PureBlue>(), 0, 0, 0, default, 35f * Main.rand.NextFloat(0.7f, 2.9f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3 * 6;
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(3.3f, 14.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 37.5f), 0).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(3.3f, 14.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.0f, 47.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 36; y++)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(3.3f, 10.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(1.8f, 13.5f)).RotatedByRandom(Math.PI * 2d);
            }
            player.velocity *= -1;
            Projectile.Kill();
        }
        private void Shoot()
        {
            SoundEngine.PlaySound(SoundID.Item72, Projectile.Center);
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            v0 = Vector2.Normalize(v0);
            Player player = Main.player[Projectile.owner];

            Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v0 * 62, v0 * (Energy + 20) / 9f, ModContent.ProjectileType<MissileProj>(), (int)(Projectile.damage * (Energy + 450) / 450f), Projectile.knockBack, player.whoAmI, Energy, 0);

            Vector2 newVelocity = v0;
            newVelocity *= 1f - Main.rand.NextFloat(0.3f);
            newVelocity *= Math.Clamp(Energy / 18f, 0.2f, 10f);
            Vector2 basePos = Projectile.Center + newVelocity * 3.7f + v0 * 62;

            for (int j = 0; j < Energy * 2; j++)
            {
                Vector2 v = newVelocity / 27f * j;
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
                int num20 = Dust.NewDust(basePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v1.X, v1.Y, 100, default, Main.rand.NextFloat(0.6f, 1.8f) * 0.4f);
                Main.dust[num20].noGravity = true;
            }
            for (int j = 0; j < Energy * 2; j++)
            {
                Vector2 v = newVelocity / 54f * j;
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
                float Scale = Main.rand.NextFloat(3.7f, 5.1f);
                int num21 = Dust.NewDust(basePos + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v1.X, v1.Y, 100, default, Scale);
                Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
            }
            for (int j = 0; j < 16; j++)
            {
                Vector2 v = newVelocity / 54f * j;
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
                v1 *= 0.2f;
                float Scale = Main.rand.NextFloat(3.7f, 5.1f);
                int num21 = Dust.NewDust(basePos + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<MothSmog>(), v1.X, v1.Y, 100, default, Scale);
                Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
            }
            player.velocity -= newVelocity;

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

            Texture2D TexMain = MythContent.QuickTexture("TheFirefly/Projectiles/FlowLightMissile");
            Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Projectiles/FlowLightMissileGlow");

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
            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + (float)(Projectile.ai[0] / -3d) * player.direction, TexMain.Size() / 2f, 1f, se, 0);
            Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, new Color(Energy,Energy,Energy,0), Projectile.rotation - (float)(Math.PI * 0.25) + (float)(Projectile.ai[0] / -3d) * player.direction, TexMain.Size() / 2f, 1f, se, 0);
        }
    }
}