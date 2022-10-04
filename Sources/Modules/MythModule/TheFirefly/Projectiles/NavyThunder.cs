using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    internal class NavyThunder : ModProjectile, IWarpProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/NavyThunderTex/FlameSkull";

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 84;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }

        private bool Release = true;
        private Vector2 oldPo = Vector2.Zero;
        private int addi = 0;

        public override void AI()
        {
            addi++;
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            if (Main.mouseLeft && Release)
            {
                Player player = Main.player[Projectile.owner];
                Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 32, -22 * player.gravDir * (float)(1.2 + Math.Sin(Main.time / 18d) / 8d))) * 0.3f;
                Projectile.spriteDirection = player.direction;
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
                    if (Projectile.timeLeft > 21)
                    {
                        Projectile.timeLeft = 20;
                    }
                    for (int j = 0; j < 16; j++)
                    {
                        Vector2 v1 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale;
                        int dust1 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<MothSmog>(), v1.X, v1.Y, 100, default(Color), Main.rand.NextFloat(3.7f, 5.1f) * 0.13f);
                        Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 0.3);
                        Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
                    }
                }
            }
            if (Main.player[Projectile.owner].itemTime == 2)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.Center);
            Player player = Main.player[Projectile.owner];
            Vector2 v0 = new Vector2(Math.Sign((Main.MouseWorld - Main.player[Projectile.owner].MountedCenter).X), 0.6f * player.gravDir);
            Vector2 ShootCenter = Projectile.Center + new Vector2(0, 16f * player.gravDir);
            ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
            Gsplayer.FlyCamPosition = new Vector2(0, 2).RotatedByRandom(6.283);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), ShootCenter, v0 * 3, ModContent.ProjectileType<Projectiles.NavyThunderBomb>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0, 0);
            Vector2 newVelocity = v0;
            newVelocity *= 1f - Main.rand.NextFloat(0.3f);
            newVelocity *= 2f;

            for (int j = 0; j < 30; j++)
            {
                Vector2 v = newVelocity / 27f * j;
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
                int num20 = Dust.NewDust(ShootCenter, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v1.X, v1.Y, 100, default(Color), Main.rand.NextFloat(0.6f, 1.8f) * 0.4f);
                Main.dust[num20].noGravity = true;
            }
            for (int j = 0; j < 30; j++)
            {
                Vector2 v = newVelocity / 54f * j;
                Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
                float Scale = Main.rand.NextFloat(3.7f, 5.1f);
                int num21 = Dust.NewDust(ShootCenter + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v1.X, v1.Y, 100, default(Color), Scale);
                Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
            }
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

            Texture2D TexMain = MythContent.QuickTexture("TheFirefly/Projectiles/NavyThunderTex/FlameSkull");
            Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Projectiles/NavyThunderTex/FlameSkullGlow");

            Projectile.frame = (int)((addi % 25) / 5f);
            Rectangle DrawRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);

            Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
            SpriteEffects se = SpriteEffects.None;
            if (player.direction == 1)
            {
                se = SpriteEffects.FlipHorizontally;
            }

            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, DrawRect, drawColor, Projectile.rotation, new Vector2(27, 42), 1f, se, 0);
            Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition, DrawRect, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(27, 42), 1f, se, 0);
        }

        private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }

        public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex)
        {
            float Wid = 6f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for (int x = 0; x < 3; x++)
            {
                float Value0 = (float)(Main.time / 291d + 20) % 1f;
                float Value1 = (float)(Main.time / 291d + 20.03) % 1f;
                vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));
            }

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }

        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
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

            Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Projectiles/NavyThunderTex/FlameSkullWarp");

            Projectile.frame = (int)((addi % 25) / 5f);
            Rectangle DrawRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);

            Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
            SpriteEffects se = SpriteEffects.None;
            if (player.direction == 1)
            {
                se = SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition, DrawRect, new Color(0.3f, 0.3f, 0.2f, 0), Projectile.rotation, new Vector2(27, 42), 1f, se, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}