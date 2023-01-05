namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    public class DarkLanternBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("爆炸灯笼");
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 3600;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(1f, 1f, 1f, 0.5f));
        }
        private float y = 0;
        private bool initialization = true;
        private bool Boom = false;
        public override void AI()
        {
            if (initialization)
            {
                num1 = -(int)(Projectile.ai[0]);
                num3 = Main.rand.NextFloat(0.3f, 1.8f);
                num4 = Main.rand.NextFloat(0.3f, 1800f);
                x = Main.rand.NextFloat(0.3f, 1800f);
                Projectile.timeLeft = (int)(Projectile.ai[0]) + 600;
                Fy = Main.rand.Next(4);
                y = 5;
                initialization = false;
            }
            num1 += 1;
            num2 -= 1;
            if (y > 1)
            {
                y -= 0.25f;
            }
            else
            {
                y = 1;
            }
            num4 += 0.01f;
            if (num1 > 0 && num1 <= 120)
            {
                num = num1 / 120f;
            }
            Projectile.velocity *= 0f;
            if (Projectile.timeLeft < 90)
            {
                Projectile.scale += 0.05f;
                num += 0.1f;
            }
            if (Projectile.timeLeft < 3)
            {
                Projectile.scale += 0.05f;
                Projectile.hostile = true;
                num += 0.5f;
            }
            //Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.8f / 255f * Projectile.scale * num1, (float)(255 - Projectile.alpha) * 0.2f / 255f * Projectile.scale * num1, (float)(255 - Projectile.alpha) * 0f / 255f * Projectile.scale * num1);
        }
        private float num = 0;
        private int num1 = 0;
        private int num2 = -1;
        private float num3 = 0.8f;
        private float num4 = 0;
        private float num5 = 0;
        private int Fy = 0;
        private int fyc = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int nuM = texture2D.Height;
            fyc += 1;
            if (fyc == 8)
            {
                fyc = 0;
                Fy += 1;
            }
            if (Fy > 3)
            {
                Fy = 0;
            }
            Color colorT = new Color(1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 0.5f * num * (float)(Math.Sin(num4) + 2) / 3f);
            x += 0.01f;
            float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
            float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.05f, 0f, 0) * 0.4f, 0, new Vector2(128f, 128f), K * 12f * y, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.05f, 0f, 0) * 0.4f, (float)(Math.PI * 0.5), new Vector2(128f, 128f), K * 6f * y, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/LanternKing/LanternFire").Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 30 * Fy, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
            for (float k = 0; k < num; k += 0.5f)
            {
                if (k > 0.5)
                {
                    Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, nuM)), new Color(1f, 1f, 1f, 0), Projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)nuM / 2f), Projectile.scale, SpriteEffects.None, 1f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, nuM)), colorT, Projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)nuM / 2f), Projectile.scale, SpriteEffects.None, 1f);
                }
            }
            return false;
        }
        private float x = 0;
        public override void Kill(int timeLeft)
        {
            /*MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Shake = 15;*/
            //Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸>(), (int)Projectile.Center.X, (int)Projectile.Center.Y);
            /*for (int k = 0; k <= 10; k++)
            {
                float a = (float)Main.rand.Next(0, 720) / 360 * 3.141592653589793238f;
                float m = (float)Main.rand.Next(0, 50000);
                float l = (float)Main.rand.Next((int)m, 50000) / 1800;
                int num4 = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, (float)((float)l * Math.Cos((float)a)) * 0.36f, (float)((float)l * Math.Sin((float)a)) * 0.36f, ModContent.ProjectileType<Projectiles.LanternKing.火山溅射>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.Projectile[num4].scale = (float)Main.rand.Next(7000, 13000) / 10000f;
            }*/
            //Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.LanternKing.FireBallWave>(), 0, 0, Projectile.owner, 0f, 0f);
            for (int i = 0; i < 90; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2.9f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.Flame>(), 0f, 0f, 100, Color.White, (float)(4f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v;
            }
            for (int i = 0; i < 60; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) + v * 45f, Projectile.width, Projectile.height, ModContent.DustType<Dusts.Flame>(), 0f, 0f, 100, Color.White, (float)(12f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v * 0.1f;
                Main.dust[num5].rotation = Main.rand.NextFloat(0, (float)(MathHelper.TwoPi));
            }
            for (int i = 0; i < 60; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(25f, 80f)).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, v.X, v.Y, 0, Color.White, 1f);
                Main.dust[num5].velocity = v;
            }
        }
    }
}