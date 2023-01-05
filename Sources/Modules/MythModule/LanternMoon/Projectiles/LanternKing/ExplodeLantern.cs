namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    public class ExplodeLantern : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("超级爆炸灯笼");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3600;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(1f, 1f, 1f, 0.5f));
        }
        private bool initialization = true;
        private bool Appear = false;
        private bool Boom = false;
        public override void AI()
        {
            if (initialization)
            {
                Fy = Main.rand.Next(4);
                initialization = false;
            }
            if (!Appear)
            {
                toAp -= 1;
                Vector2 v = new Vector2(Main.rand.Next(0, 100), 0).RotatedByRandom(Math.PI * 2d);
                //int Z = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + v, 0, 0, ModContent.DustType<Dusts.LanternDust>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(0.18f, 0.25f));
                //int Zo = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + v, 0, 0, ModContent.DustType<Dusts.LanternDust2>(), 0f, 0f, 0, default(Color), Main.rand.NextFloat(0.18f, 0.25f));
            }
            else
            {
                Projectile.velocity *= 1.14f;
                if (num < 1)
                {
                    num += 0.05f;
                }
                else
                {
                    num = 1f;
                }
                float Sc = Projectile.velocity.Length() / 10f;
                if (Sc > 1)
                {
                    Sc = 1;
                    Projectile.hostile = true;
                }
                if (Projectile.velocity.Length() > 150)
                {
                    Projectile.Kill();
                }
                for (int g = 0; g < Projectile.velocity.Length() / 4f; g++)
                {
                    int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) - Projectile.velocity / Projectile.velocity.Length() * g * 4, 0, 0, ModContent.DustType<Dusts.Flame>(), 0, 0, 0, default(Color), 4f * Sc);
                    Main.dust[r].noGravity = true;
                    if (Main.rand.Next(100) > 60)
                    {
                        int r0 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + new Vector2(0, Main.rand.NextFloat(0, 36f)).RotatedByRandom(MathHelper.TwoPi) - Projectile.velocity / Projectile.velocity.Length() * g * 4, 0, 0, ModContent.DustType<Dusts.Flame3>(), 0, 0, 0, default(Color), 7f * Sc);
                        Main.dust[r0].noGravity = true;
                    }
                }
            }
            if (toAp >= 0)
            {
                Rli = (float)Math.Sin((toAp + 60) / 180d * Math.PI);
            }
            num4 += 0.01f;
            if (toAp <= 0)
            {
                toAp = 0;
                Appear = true;
                Projectile.tileCollide = true;
            }
            //Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.8f / 255f * Projectile.scale * num1, (float)(255 - Projectile.alpha) * 0.2f / 255f * Projectile.scale * num1, (float)(255 - Projectile.alpha) * 0f / 255f * Projectile.scale * num1);
        }
        private float num = 0;
        private float num4 = 0;
        private int Fy = 0;
        private int fyc = 0;
        private int toAp = 120;
        private float Rli = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Appear)
            {
                Projectile.Kill();
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            //MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
            //mplayer.Shake = 15;
            //Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸>(), (int)Projectile.Center.X, (int)Projectile.Center.Y);
            /*for (int k = 0; k <= 10; k++)
            {
                float a = (float)Main.rand.Next(0, 720) / 360 * 3.141592653589793238f;
                float m = (float)Main.rand.Next(0, 50000);
                float l = (float)Main.rand.Next((int)m, 50000) / 1800;
                int num4 = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, (float)((float)l * Math.Cos((float)a)) * 0.36f, (float)((float)l * Math.Sin((float)a)) * 0.36f, ModContent.ProjectileType<Projectiles.LanternKing.火山溅射>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.Projectile[num4].scale = (float)Main.rand.Next(7000, 13000) / 10000f;
            }*/
            //Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.FireBallWave>(), 0, 0, Projectile.owner, 0f, 0f);
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
        private int Dto = 0;
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
            if (Appear)
            {
                Color colorT = new Color(1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 1f * num * (float)(Math.Sin(num4) + 2) / 3f, 0.5f * num * (float)(Math.Sin(num4) + 2) / 3f);
                Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, nuM)), colorT, Projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)nuM / 2f), Projectile.scale, SpriteEffects.None, 1f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/LanternKing/LanternFire").Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 30 * Fy, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
            }
            else
            {
                if (toAp >= 0)
                {

                }
            }
            Dto += 1;
            for (int j = 0; j < 6; j++)
            {
                Vector2 v = new Vector2(0, 30).RotatedBy(j / 3d * Math.PI + Dto / 40d);
                v.Y *= 0.2f;
                float S = 1f / v.Length() + 0.2f;
                if (Projectile.velocity.Length() > 0.01f)
                {
                    S /= (Projectile.velocity.Length() * 100f);
                }
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/LanternKing/LightEffect").Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + v, new Rectangle?(new Rectangle(0, 0, 256, 256)), new Color(Rli * S, 0, 0, 0), 0, new Vector2(128, 128f), 0.7f * (S + 0.2f), SpriteEffects.None, 1f);
            }
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.Kill();
        }
    }
}