using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class StarDancer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Dancer");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "星之舞者");
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.tileCollide = false;
            Projectile.frame = 0;
            Projectile.DamageType = DamageClass.Melee;
        }
        private int lz = 0;
        private float Ome = 0;
        private Effect ef2;
        private Effect ef3;
        int DOpen = 0;
        public override void AI()
        {
            if (DOpen == 0)
            {
                DOpen = Projectile.damage;
                Projectile.damage = 0;
            }
            Projectile.damage = (int)(DOpen * Ome * 2.5);
            lz += 1;
            Player p = Main.player[Projectile.owner];
            Vector2 v = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - p.Center;
            v = v / v.Length();
            Projectile.velocity = v * 15f;
            Vector2 vT0 = Main.MouseWorld - p.Center;
            p.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(vT0.Y, vT0.X) - Math.PI / 2d));
            Projectile.position = p.position + v + new Vector2(-(Projectile.width / 2 - p.width / 2), -24);
            Projectile.spriteDirection = p.direction;
            Projectile.rotation += Ome;
            if (Projectile.timeLeft > 20)
            {
                if (Ome < 0.4f)
                {
                    Ome += 0.005f;
                }
            }
            else
            {
                Ome *= 0.9f;
            }
            if (Projectile.timeLeft < 22 && Main.mouseLeft && !p.dead && p.HeldItem.type == ModContent.ItemType<Clubs.StarDancer>())
            {
                Projectile.timeLeft = 22;
            }
            if (p.dead)
            {
                Projectile.Kill();
            }
            if (lz % (int)(1.6 / (Ome + 0.000001)) == 1)
            {
                Projectile.friendly = true;
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            }
            else
            {
                Projectile.friendly = false;
            }
            p.ChangeDir(Projectile.direction);
            p.heldProj = Projectile.whoAmI;

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            ef2 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague").Value;
            float k0 = Projectile.velocity.Y / (float)Projectile.velocity.X;
            k0 *= (float)Main.screenWidth / (float)Main.screenHeight;
            ef2.Parameters["k0"].SetValue(k0);
            Vector2 v0 = Projectile.Center + v - Main.screenPosition + new Vector2(32, -32).RotatedBy(Projectile.rotation);
            Vector2 v1 = new Vector2(32, -32).RotatedBy(Projectile.rotation);
            Vector2 vc = Projectile.Center + v * 15 - Main.screenPosition - p.velocity;
            float x0 = v0.X / (float)Main.screenWidth;
            float y0 = v0.Y / (float)Main.screenHeight;
            float xc = vc.X / (float)Main.screenWidth;
            float yc = vc.Y / (float)Main.screenHeight;
            float b0 = y0 - k0 * x0;
            float rot = (float)(Math.Atan2(v1.Y, v1.X) + 1.5708 * (Main.player[Projectile.owner].direction + 1));
            ef2.Parameters["b0"].SetValue(b0);
            ef2.Parameters["x1"].SetValue(x0);
            ef2.Parameters["y1"].SetValue(y0);
            ef2.Parameters["xc"].SetValue(xc);
            ef2.Parameters["yc"].SetValue(yc);
            ef2.Parameters["rot"].SetValue(rot);
            ef2.Parameters["DMax"].SetValue(0.065f * Main.GameViewMatrix.Zoom.X);
            ef2.Parameters["Ome"].SetValue(Ome - 0.01f);
            ef2.Parameters["uImage1"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ClubShader").Value);
            ef2.Parameters["uImage2"].SetValue(texture);


            ef3 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2").Value;
            v0 = Projectile.Center + v - Main.screenPosition + new Vector2(-32, 32).RotatedBy(Projectile.rotation);
            v1 = new Vector2(-32, 32).RotatedBy(Projectile.rotation);
            x0 = v0.X / (float)Main.screenWidth;
            y0 = v0.Y / (float)Main.screenHeight;
            rot = (float)(Math.Atan2(v1.Y, v1.X) + 1.5708 * (Main.player[Projectile.owner].direction + 1));
            ef3.Parameters["b0"].SetValue(b0);
            ef3.Parameters["x1"].SetValue(x0);
            ef3.Parameters["y1"].SetValue(y0);
            ef3.Parameters["xc"].SetValue(xc);
            ef3.Parameters["yc"].SetValue(yc);
            ef3.Parameters["rot"].SetValue(rot);
            ef3.Parameters["DMax"].SetValue(0.065f * Main.GameViewMatrix.Zoom.X);
            ef3.Parameters["Ome"].SetValue(Ome - 0.01f);
            ef3.Parameters["uImage1"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ClubShader").Value);
            ef3.Parameters["uImage2"].SetValue(texture);
            if (Projectile.timeLeft > 6)
            {
                if (!Filters.Scene["ClubVague"].IsActive())
                {
                    Filters.Scene.Activate("ClubVague");
                }
                if (!Filters.Scene["ClubVague2"].IsActive())
                {
                    Filters.Scene.Activate("ClubVague2");
                }
            }
            else
            {
                if (Filters.Scene["ClubVague"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVague");
                }
                if (Filters.Scene["ClubVague2"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVague2");
                }
            }
            float li = Main.rand.NextFloat(-32f, 32f);
            /*if (lz % 8 == 1)
            {
                Gore.NewGore(null, Projectile.Center - new Vector2(4, 4) + new Vector2(li, li).RotatedBy(Projectile.rotation), new Vector2(li, li).RotatedBy(Math.PI / 2 + Projectile.rotation) * 0.3f, Main.rand.Next(16, 18), 1f);
            }
            if (lz % 3 == 1)
            {
                int num25 = Dust.NewDust(base.Projectile.Center - new Vector2(4, 4) + new Vector2(li, li).RotatedBy(Projectile.rotation), 0, 0, 150, 0, 0, 0, default(Color), 1.8f);
                Main.dust[num25].noGravity = true;
                Main.dust[num25].velocity = new Vector2(li, li).RotatedBy(Math.PI / 2 + Projectile.rotation) * 0.3f;
            }
            if (lz % 3 == 0)
            {
                int num25 = Dust.NewDust(base.Projectile.Center - new Vector2(4, 4) + new Vector2(li, li).RotatedBy(Projectile.rotation), 0, 0, 58, 0, 0, 0, default(Color), 1.8f);
                Main.dust[num25].noGravity = true;
                Main.dust[num25].velocity = new Vector2(li, li).RotatedBy(Math.PI / 2 + Projectile.rotation) * 0.3f;
            }*/
            if (lz % 15 == 1)
            {
                float X = Main.rand.NextFloat(-250, 250);
                float Y = Main.rand.NextFloat(-250, 250);
                Vector2 v2 = (new Vector2(Main.screenPosition.X + Main.mouseX + Main.rand.NextFloat(-24, 24), Main.screenPosition.Y + Main.mouseY + Main.rand.NextFloat(-24, 24)) - new Vector2(Projectile.Center.X + X, Projectile.Center.Y - 1000f + Y));
                v2 = v2 / v2.Length() * 13f;
                int o = Projectile.NewProjectile(Projectile.InheritSource(Projectile), p.Center + v2 + new Vector2(X, Y - 500), v2 + new Vector2(0, 10), 9, Projectile.damage * 2, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, 0f, 0f);
            }
            p.ChangeDir(Projectile.direction);
            p.heldProj = Projectile.whoAmI;
        }
        public override void Kill(int timeLeft)
        {
            if (Filters.Scene["ClubVague"].IsActive())
            {
                Filters.Scene.Deactivate("ClubVague");
            }
            if (Filters.Scene["ClubVague2"].IsActive())
            {
                Filters.Scene.Deactivate("ClubVague2");
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int Heig = texture.Height;
            int y = Heig * Projectile.frame;
            Player p = Main.player[Projectile.owner];
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
            ef2.Parameters["color0"].SetValue(new Vector4(color.R / 255f * Ome, color.G / 255f * Ome, color.B / 255f * Ome, -color.A / 255f));
            ef3.Parameters["color0"].SetValue(new Vector4(color.R / 255f * Ome, color.G / 255f * Ome, color.B / 255f * Ome, -color.A / 255f));
            if (Projectile.timeLeft <= 6)
            {
                if (Filters.Scene["ClubVague"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVague");
                }
                if (Filters.Scene["ClubVague2"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVague2");
                }
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, Heig, 64)), color, Projectile.rotation, new Vector2(32, 32), Projectile.scale, effects, 0f);
            for (int i = 0; i < 5; i++)
            {
                float alp = Ome / 0.4f;
                Color color2 = new Color((int)(color.R * (5 - i) / 5f * alp), (int)(color.G * (5 - i) / 5f * alp), (int)(color.B * (5 - i) / 5f * alp), (int)(color.A * (5 - i) / 5f * alp));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color2, Projectile.rotation - i * 0.75f * Ome, new Vector2(32, 32), Projectile.scale, effects, 0f);
            }
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Texture2D texture = ModContent.Request<Texture2D>("MythMod/Projectiles/Melee/Clubs/StarDancerGlow").Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(32f, 32f), 1f, effects, 0f);
            Color color = new Color(55, 55, 55, 0);
            for (int i = 0; i < 25; i++)
            {
                float alp = Ome / 0.4f;
                Color color2 = new Color((int)(color.R * (25.1 - i) / 25f * alp), (int)(color.G * (25.1 - i) / 25f * alp), (int)(color.B * (25.1 - i) / 25f * alp), (int)(color.A * (25.1 - i) / 25f * alp));
                if (2.2 * (25.1 - i) < 2)
                {
                    break;
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color2, Projectile.rotation - i * 0.06f * Ome * 2.5f, new Vector2(32, 32), Projectile.scale, effects, 0f);
            }
        }
    }
}
