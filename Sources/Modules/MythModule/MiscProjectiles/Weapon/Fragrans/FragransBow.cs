namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Fragrans
{
	class FragransBow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360000;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        private float K = 10;
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 v = Main.MouseWorld - player.Center;
            Projectile.rotation = (float)(Math.Atan2(v.Y, v.X));
            Projectile.position = player.Center + new Vector2(-21, -32) + new Vector2(10, 0).RotatedBy(Projectile.rotation);
            if (!Main.mouseLeft)
            {
                Projectile.NewProjectile(null, Projectile.Center, v / v.Length() * 30f, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransArrow>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0f);
                Projectile.Kill();
            }
        }
        private Effect ef;
        /*public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Fragrans/FragransArrow").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                Color color = new Color(255, 255, 255, 0);
                float Fad = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color color2 = new Color((int)(color.R * Fad * Fad), (int)(color.G * Fad * Fad), (int)(color.B * Fad), (int)(color.A * Fad));
                Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }*/
    }
}
