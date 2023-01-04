using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class MeteorClub : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MeteorClub");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "陨石棍");
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
        private Effect ef4;
        private Effect ef5;
        int DOpen = 0;
        public override void AI()
        {
            if (DOpen == 0)
            {
                DOpen = Projectile.damage;
                Projectile.damage = 0;
            }
            Projectile.damage = (int)(Ome * 2.5 * DOpen);
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
                    Ome += 0.004f;
                }
            }
            else
            {
                Ome *= 0.9f;
            }
            if (Projectile.timeLeft < 22 && Main.mouseLeft && !p.dead && p.HeldItem.type == ModContent.ItemType<Clubs.MeteorClub>())
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


            ef4 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVagueF").Value;
            v0 = Projectile.Center + v - Main.screenPosition + new Vector2(-32, 32).RotatedBy(Projectile.rotation);
            v1 = new Vector2(-32, 32).RotatedBy(Projectile.rotation);
            x0 = v0.X / (float)Main.screenWidth;
            y0 = v0.Y / (float)Main.screenHeight;
            rot = (float)(Math.Atan2(v1.Y, v1.X) + 1.5708 * (Main.player[Projectile.owner].direction + 1));
            ef4.Parameters["b0"].SetValue(b0);
            ef4.Parameters["x1"].SetValue(x0);
            ef4.Parameters["y1"].SetValue(y0);
            ef4.Parameters["xc"].SetValue(xc);
            ef4.Parameters["yc"].SetValue(yc);
            ef4.Parameters["rot"].SetValue(rot);
            ef4.Parameters["DMax"].SetValue(0.065f * Main.GameViewMatrix.Zoom.X);
            ef4.Parameters["Ome"].SetValue(Ome - 0.01f);
            ef4.Parameters["uImage1"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ClubShaderFlame").Value);
            ef4.Parameters["uImage2"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ClubShaderFlame2").Value);


            ef5 = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2F").Value;
            v0 = Projectile.Center + v - Main.screenPosition + new Vector2(32, -32).RotatedBy(Projectile.rotation);
            v1 = new Vector2(32, -32).RotatedBy(Projectile.rotation);
            x0 = v0.X / (float)Main.screenWidth;
            y0 = v0.Y / (float)Main.screenHeight;
            rot = (float)(Math.Atan2(v1.Y, v1.X) + 1.5708 * (Main.player[Projectile.owner].direction + 1));
            ef5.Parameters["b0"].SetValue(b0);
            ef5.Parameters["x1"].SetValue(x0);
            ef5.Parameters["y1"].SetValue(y0);
            ef5.Parameters["xc"].SetValue(xc);
            ef5.Parameters["yc"].SetValue(yc);
            ef5.Parameters["rot"].SetValue(rot);
            ef5.Parameters["DMax"].SetValue(0.065f * Main.GameViewMatrix.Zoom.X);
            ef5.Parameters["Ome"].SetValue(Ome - 0.01f);
            ef5.Parameters["uImage1"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ClubShaderFlame").Value);
            ef5.Parameters["uImage2"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ClubShaderFlame2").Value);
            if (Projectile.timeLeft > 6)
            {
                if (!Filters.Scene["ClubVagueF"].IsActive())
                {
                    Filters.Scene.Activate("ClubVagueF");
                }
                if (!Filters.Scene["ClubVague2F"].IsActive())
                {
                    Filters.Scene.Activate("ClubVague2F");
                }
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
                if (Filters.Scene["ClubVagueF"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVagueF");
                }
                if (Filters.Scene["ClubVague2F"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVague2F");
                }
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
            int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center - new Vector2(4, 4) + v * 15 - p.velocity + new Vector2(li, li).RotatedBy(Projectile.rotation + Math.PI / 2d), new Vector2(li, li).RotatedBy(Math.PI + Projectile.rotation) * Main.rand.NextFloat(0.18f, 0.3f) * Ome * 2.5f, ModContent.ProjectileType<MiscProjectiles.Typeless.Spark>(), Projectile.damage / 4, Projectile.knockBack * 0.5f, p.whoAmI, p.GetCritChance(DamageClass.Melee), 0);
            Main.projectile[h].scale = Ome * 2.5f;

            p.ChangeDir(Projectile.direction);
            p.heldProj = Projectile.whoAmI;
        }
        public override void Kill(int timeLeft)
        {
            if (Filters.Scene["ClubVagueF"].IsActive())
            {
                Filters.Scene.Deactivate("ClubVagueF");
            }
            if (Filters.Scene["ClubVague2F"].IsActive())
            {
                Filters.Scene.Deactivate("ClubVague2F");
            }
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
            ef4.Parameters["color0"].SetValue(new Vector4(2.5f * Ome, 2.5f * Ome, 2.5f * Ome, 1));
            ef5.Parameters["color0"].SetValue(new Vector4(2.5f * Ome, 2.5f * Ome, 2.5f * Ome, 1));
            if (Projectile.timeLeft <= 6)
            {
                if (Filters.Scene["ClubVagueF"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVagueF");
                }
                if (Filters.Scene["ClubVague2F"].IsActive())
                {
                    Filters.Scene.Deactivate("ClubVague2F");
                }
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Clubs/Projectiles/MeteorClubGlow").Value;
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
