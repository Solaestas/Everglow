using ReLogic.Graphics;
using Terraria.Audio;

namespace MythMod.Projectiles.Ranged.Slingshots
{
    internal class WoodSlingshot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 90;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
        }

        private bool Release = true;
        private int PdamF = 0;
        private Vector2 oldPo = Vector2.Zero;
        private int addi = 0;
        public override void AI()
        {
            addi++;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (addi == 24 && Main.mouseLeft)
            {
                SoundEngine.PlaySound(new SoundStyle("MythMod/Sounds/Slingshot2"), Projectile.Center);
            }
            Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
            if (Main.mouseLeft && Release)
            {
                if (Projectile.timeLeft > 80)
                {
                    PdamF = Projectile.damage;
                }
                Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 0.25);
                Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0) * 15f + new Vector2(0, -4);
                oldPo = Projectile.Center;
                Projectile.Center = oldPo;
                Projectile.velocity *= 0;
                Projectile.timeLeft = 5 + Energy;
                if (Energy < 120)
                {
                    if (addi % 2 == 1)
                    {
                        Energy++;
                    }
                    Energy++;
                }
                else
                {
                    Energy = 120;
                }
            }
            Common.GlobalProjectiles.MythModGlobalProjectile.OutEnergy[Projectile.whoAmI] = Energy;
            float DrawRot = 0;
            if (Projectile.Center.X < player.MountedCenter.X)
            {
                player.direction = -1;
                DrawRot = Projectile.rotation - (float)(Math.PI / 4d);
            }
            else
            {
                player.direction = 1;
                DrawRot = Projectile.rotation - (float)(Math.PI * 0.25);
            }
            Vector2 v4 = new Vector2(14, -14).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
            Vector2 v3 = new Vector2(14, -14).RotatedBy(DrawRot) + Vector2.Normalize(v4) * Energy / 3f;
            if (player.direction == -1)
            {
                v4 = new Vector2(14, -14).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
                v3 = new Vector2(14, -14).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(v4) * Energy / 3f;
            }
            if (!Main.mouseLeft && Release)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.friendly = true;
                Projectile.tileCollide = true;
                Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0) * 15f + new Vector2(0, -4);
                SoundEngine.PlaySound(new SoundStyle("MythMod/Sounds/SlingshotShoot2"), Projectile.Center);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + v3, -Vector2.Normalize(v4) * (float)(Energy / 5f + 8f) * (Projectile.ai[0]) / 8f, ModContent.ProjectileType<Projectiles.Ranged.Slingshots.SlingshotAmmo>(), (int)(Projectile.damage * (1 + Energy / 40f)), Projectile.knockBack, player.whoAmI);
                Release = false;
            }
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[1] -= 1f;
            }
            if (!Main.mouseLeft && !Release)
            {
                if (Projectile.ai[1] > 0)
                {
                    Projectile.ai[1] -= 1f;
                    Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0) * 15f + new Vector2(0, -4);
                }
                else
                {
                    Projectile.Kill();
                }
            }
        }
        private bool Nul = false;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        private int Energy = 0;
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Color drawColor = Lighting.GetColor((int)(Projectile.Center.X / 16.0), (int)(Projectile.Center.Y / 16.0));
            SpriteEffects se = SpriteEffects.None;
            float DrawRot = 0;
            if (Projectile.Center.X < player.MountedCenter.X)
            {
                player.direction = -1;
                DrawRot = Projectile.rotation - (float)(Math.PI / 4d);
            }
            else
            {
                player.direction = 1;
                DrawRot = Projectile.rotation - (float)(Math.PI * 0.25);
            }
            Vector2 v0 = Main.MouseWorld - player.MountedCenter;
            Vector2 v1 = new Vector2(9, -16).RotatedBy(DrawRot);
            Vector2 v2 = new Vector2(16, -9).RotatedBy(DrawRot);
            Vector2 v4 = new Vector2(14, -14).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
            Vector2 v3 = new Vector2(14, -14).RotatedBy(DrawRot) + Vector2.Normalize(v4) * Energy / 3f;
            Vector2 v6 = new Vector2(14, -14).RotatedBy(DrawRot) + Vector2.Normalize(v4) * Energy * 0.3125f;
            if (player.direction == -1)
            {
                v1 = new Vector2(9, -16).RotatedBy(DrawRot + Math.PI / 2d);
                v2 = new Vector2(16, -9).RotatedBy(DrawRot + Math.PI / 2d);
                v4 = new Vector2(14, -14).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
                v3 = new Vector2(14, -14).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(v4) * Energy / 3f;
                v6 = new Vector2(14, -14).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(v4) * Energy * 0.3125f;
            }

            Vector2 v5 = player.MountedCenter - (Projectile.Center + v6);
            if (Projectile.Center.X < player.MountedCenter.X)
            {
                se = SpriteEffects.FlipVertically;


                if (Main.mouseLeft)
                {
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - (float)(Math.PI * 0.75)));
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v5.Y, v5.X) - (float)(Math.PI * 1.5)));
                }
                DrawRot = Projectile.rotation - (float)(Math.PI / 4d);
            }
            else
            {
                player.direction = 1;

                if (Main.mouseLeft)
                {
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - (float)(Math.PI * 0.25)));
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v5.Y, v5.X) + (float)(Math.PI * 0.5)));
                }
                DrawRot = Projectile.rotation - (float)(Math.PI * 0.25);
            }

            /*List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();


            Vx.Add(new VertexBase.CustomVertexInfo(Projectile.Center + v1 - Main.screenPosition, drawColor, new Vector3(0, 0, 0)));
            Vx.Add(new VertexBase.CustomVertexInfo(Projectile.Center + v2 - Main.screenPosition, drawColor, new Vector3(0, 1, 0)));
            Vx.Add(new VertexBase.CustomVertexInfo(Projectile.Center + v3 - Main.screenPosition, drawColor, new Vector3(1, 0.5f, 0)));

            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("MythMod/Projectiles/Ranged/Slingshots/SlingshotString").Value;//
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);*/
            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, null, drawColor, DrawRot, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
            if (Energy > 20)
            {
                string Tx = Energy.ToString();
                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Tx, Main.MouseScreen + new Vector2(0, 15), new Color((Energy - 20f) / 100f, (Energy - 20f) / 100f, (Energy - 20f) / 100f, (Energy - 20f) / 100f), 0, new Vector2(Tx.Length * 2, 1f), 1, SpriteEffects.None, 0);
            }
        }
    }
}
