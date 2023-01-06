using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class RainbowCircle : ModProjectile
    {
        public override void SetStaticDefaults()
        {//TODO: Localization Needed
            DisplayName.SetDefault("Rainbow Circle");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "彩色环");
        }
        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            //ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            //ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        private int neartokill = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.itemTime = 5;
            player.itemAnimation = 5;
            Projectile.position = player.Center - new Vector2(150, 150);
            if (Main.mouseLeftRelease && neartokill == 0 && Projectile.timeLeft < 9990)
            {
                neartokill = 1;
            }
            if (neartokill == 1)
            {
                Projectile.timeLeft = 10;
                int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center - new Vector2(0, 200), Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.Light>(), 0, 0, player.whoAmI, Rt + Gt / 1000f, Bt + Ct / 1000f);
                int AddEffect = (int)(100 * (player.GetDamage(DamageClass.Generic).Additive) * Main.rand.NextFloat(0.85f, 1.15f) * Stre * Stre);
                if (AddEffect > 250)
                {
                    AddEffect = 250;
                }
                Main.projectile[h].damage = AddEffect;
                bool Cri = false;
                if (Main.rand.Next(100) < player.GetCritChance(DamageClass.Magic) + 4)
                {
                    Main.projectile[h].damage *= 2;
                    Cri = true;
                }
                CombatText.NewText(new Rectangle((int)player.Center.X - 10, (int)player.Center.Y - 10, 20, 20), new Color(Rt, Gt, Bt, 0), "⭐" + Main.projectile[h].damage.ToString() + "⭐", Cri);
                neartokill = 2;
            }
            if (neartokill == 0)
            {
                if (Stre < 1)
                {
                    Stre += 0.007f;
                }
                else
                {
                    Stre = 1;
                }
            }
            if (neartokill == 2)
            {
                if (Stre > 0)
                {
                    Stre -= 0.1f;
                }
                else
                {
                    Stre = 0;
                }
            }
            add += 0.01f;
            Projectile.alpha = (int)(255 - Stre * 255);
        }
        public override void PostDraw(Color lightColor)
        {
        }
        private float Stre = 0;
        private int Rt = 0;
        private int Gt = 0;
        private int Bt = 0;
        private float Ct = 0;
        private int[] Rx = new int[15];
        private int[] Gx = new int[15];
        private int[] Bx = new int[15];
        private Vector2[] Vx = new Vector2[15];
        float add = 0;
        public override Color? GetAlpha(Color lightColor)
        {
            int C = 255 - Projectile.alpha;
            return new Color(C / 2, C / 2, C / 2, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Niddle = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/RainbowNeedle").Value;
            Texture2D Niddle2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/RainbowNeedle2").Value;
            Vector2 Vms = Main.MouseWorld;
            Vector2 Vdr = Vms - Projectile.Center;
            if (Main.mouseLeft)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(Vdr.Y, Vdr.X) - Math.PI / 2d));
            }
            player.heldProj = Projectile.whoAmI;
            Vdr = Vdr / Vdr.Length() * 7;
            for (int g = 0; g < 15; g++)
            {
                Vx[g] = new Vector2(0, 7).RotatedBy((add * Math.Sqrt(g + 1) + g) * (0.5f - (g % 2)) * 2f);
            }
            Texture2D Blue = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/RainbowCircle").Value;
            Color[] colorTex = new Color[Blue.Width * Blue.Height];
            Blue.GetData(colorTex);
            for (int g = 0; g < 15; g++)
            {
                Vector2 vg = Vx[g];
                int Xg = (int)vg.X + 15;
                int Yg = (int)vg.Y + 15;
                float Rg = colorTex[Xg + Yg * Blue.Width].R / 155f;
                float Gg = colorTex[Xg + Yg * Blue.Width].G / 155f;
                float Bg = colorTex[Xg + Yg * Blue.Width].B / 155f;
                Rx[g] = (int)(Rg * 155 * Stre);
                Gx[g] = (int)(Gg * 155 * Stre);
                Bx[g] = (int)(Bg * 155 * Stre);
            }
            Vector2 vk = Vdr;
            int Xk = (int)vk.X + 15;
            int Yk = (int)vk.Y + 15;
            float Rk = colorTex[Xk + Yk * Blue.Width].R / 155f;
            float Gk = colorTex[Xk + Yk * Blue.Width].G / 155f;
            float Bk = colorTex[Xk + Yk * Blue.Width].B / 155f;
            Rt = (int)(Rk * 155 * Stre);
            Gt = (int)(Gk * 155 * Stre);
            Bt = (int)(Bk * 155 * Stre);
            Ct = (float)(((Math.Atan2(Vdr.Y, Vdr.X) + Math.PI) / MathHelper.TwoPi) * 7d);
            Lighting.AddLight(Projectile.Center + vk * 10, new Vector3(Rk, Gk, Bk) * Stre);
            Vector2 vn = new Vector2(0, 7).RotatedBy(Main.time * 0.08);
            int Xn = (int)vn.X + 15;
            int Yn = (int)vn.Y + 15;
            float Rn = colorTex[Xn + Yn * Blue.Width].R / 455f;
            float Gn = colorTex[Xn + Yn * Blue.Width].G / 455f;
            float Bn = colorTex[Xn + Yn * Blue.Width].B / 455f;
            Lighting.AddLight(Projectile.Center + vn * 10, new Vector3(Rn, Gn, Bn) * Stre);
            Vector2 vl = new Vector2(0, 7).RotatedBy(Main.time * -0.06);
            int Xl = (int)vl.X + 15;
            int Yl = (int)vl.Y + 15;
            float Rl = colorTex[Xl + Yl * Blue.Width].R / 575f;
            float Gl = colorTex[Xl + Yl * Blue.Width].G / 575f;
            float Bl = colorTex[Xl + Yl * Blue.Width].B / 575f;
            Lighting.AddLight(Projectile.Center + vl * 10, new Vector3(Rl, Gl, Bl) * Stre);
            Vector2 vm = new Vector2(0, 7).RotatedBy(Main.time * 0.03);
            int Xm = (int)vm.X + 15;
            int Ym = (int)vm.Y + 15;
            float Rm = colorTex[Xm + Ym * Blue.Width].R / 625f;
            float Gm = colorTex[Xm + Ym * Blue.Width].G / 625f;
            float Bm = colorTex[Xm + Ym * Blue.Width].B / 625f;
            Lighting.AddLight(Projectile.Center + vm * 10, new Vector3(Rm, Gm, Bm) * Stre);
            Main.EntitySpriteDraw(Niddle, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(Rk * 255 * Stre), (int)(Gk * 255 * Stre), (int)(Bk * 255 * Stre), 0), (float)Math.Atan2(Vdr.Y, Vdr.X), new Vector2(150f, 150f), 1, SpriteEffects.None, 0);
            for (int g = 0; g < 15; g++)
            {
                Main.EntitySpriteDraw(Niddle2, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(Rx[g] * 0.5 * Stre), (int)(Gx[g] * 0.5 * Stre), (int)(Bx[g] * 0.5 * Stre), 0), (float)(Math.Atan2(Vx[g].Y, Vx[g].X)), new Vector2(150f, 150f), 1, SpriteEffects.None, 0);
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Legendary/RainbowLight").Value;
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
            SpriteEffects S = SpriteEffects.None;
            if (Math.Sign(Vdr.X) == -1)
            {
                player.direction = -1;
            }
            else
            {
                player.direction = 1;
            }
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition + Vdr * 4f, null, color, (float)(Math.Atan2(Vdr.Y, Vdr.X) + Math.PI / 4d), new Vector2(36f, 36f), Projectile.scale, S, 0f);
            return true;
        }
    }
}
