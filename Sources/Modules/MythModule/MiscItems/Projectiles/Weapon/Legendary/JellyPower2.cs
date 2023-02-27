using Terraria.Audio;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
	public class JellyPower2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("JellyPower");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "啫喱喷流");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 9;
            Projectile.extraUpdates = 4;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 12000;
            Projectile.hostile = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.03f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Weapons.Slingshots.Projectiles.KSSlingshotHit>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, 0.3f, 2 - Projectile.ai[0]);
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                Projectile.velocity *= 0.95f;
            }
            if (Projectile.ai[0] == 0 && Projectile.penetrate >= 6)
            {
                Player player = Main.player[Projectile.owner];
                for (int g = 0; g < 3; g++)
                {
                    int h = Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.3f, 0.6f), Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 1);
                    Main.projectile[h].penetrate = 3;
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/JellyPower2").Value;
            Texture2D t2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/JellyPowerL2").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            Player player = Main.player[Projectile.owner];
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                Color color = new Color(165, 165, 165, 105);
                Color color2 = new Color((int)(color.R * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.G * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.B * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.A * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
                Main.spriteBatch.Draw(t2, drawPos, null, new Color(color2.R * 2 / (k + 4) * Projectile.penetrate / 9f / 255f, color2.R * 2 / (k + 4) * Projectile.penetrate / 9f / 255f, color2.R * 2 / (k + 4) * Projectile.penetrate / 9f / 255f, 0), Projectile.rotation, new Vector2(30), Projectile.scale * Projectile.penetrate / 9f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
