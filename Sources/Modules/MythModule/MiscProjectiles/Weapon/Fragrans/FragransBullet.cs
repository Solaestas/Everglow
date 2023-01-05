using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Fragrans
{
	class FragransBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Bullet");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "月光子弹");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 720;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        private float K = 10;
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            for (int y = 0; y < 6; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscDusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 11.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 3; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscDusts.Fragrans.Fragrans>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(0.6f, 0.8f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 7.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 50 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 8, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[0] / 100f) * 1.0f));
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.Fragrans>()] < 1)
            {
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.Fragrans>(), 0, 0, player.whoAmI, 0, 0);
                player.AddBuff(ModContent.BuffType<MiscBuffs.Fragrans.MoonAndFragrans>(), 300);
            }
            else
            {
                MiscProjectiles.Weapon.Fragrans.Fragrans.Reset = 300;
                if (target.type == NPCID.TargetDummy)
                {
                    MiscProjectiles.Weapon.Fragrans.Fragrans.Dummy = true;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.5);
            if (Projectile.timeLeft < 19)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.velocity.Length() > 0.1f)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 6)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }
            if (Main.rand.Next(6) == 0)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, ModContent.DustType<MiscDusts.Fragrans.FragransDust>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.2f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = Projectile.velocity * Main.rand.NextFloat(0.4f, 0.9f);
            }
        }
        private Effect ef;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int num = texture2D.Height / Main.projFrames[Projectile.type];
            int y = num * Projectile.frame;
            Vector2 drawOrigin = new Vector2((float)texture2D.Width / 2f, (float)num / 2f);
            Vector2 v = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY + Projectile.height / 2f);
            Main.spriteBatch.Draw(texture2D, v, new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                Color color = new Color(255, 255, 255, 0);
                float Fad = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color color2 = new Color((int)(color.R * Fad * Fad), (int)(color.G * Fad * Fad), (int)(color.B * Fad), (int)(color.A * Fad));
                Main.spriteBatch.Draw(texture2D, drawPos, new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), color2, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
