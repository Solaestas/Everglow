using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
	public class MachineFit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MachineFit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "统帅之拳");
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1080;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
            Projectile.extraUpdates = 8;
        }
        private bool initialization = true;
        private double X;
        private float Omega;
        private float b;
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            Projectile.velocity *= 1.01f;
            if (Projectile.timeLeft < 1075)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(12, 12) - Projectile.velocity / Projectile.velocity.Length() * 36f, 16, 16, 6, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), Main.rand.NextFloat(1f, 2.6f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity *= 2.5f;
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) - Projectile.velocity / Projectile.velocity.Length() * 36f, 0, 0, ModContent.DustType<LanternMoon.Dusts.Flame2>(), 0, 0, 0, default(Color), 4f);
                Main.dust[r].noGravity = true;
                Main.dust[r].rotation = Main.rand.NextFloat(0, (float)(MathHelper.TwoPi));
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            if (timeLeft != 0)
            {
                //int uo = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, 164, Projectile.damage, 1, Main.myPlayer, 0f, 0f);
                //Main.projectile[uo].friendly = true;
                //Main.projectile[uo].hostile = false;
            }
            for (int j = 0; j < 30; j++)
            {
                for (int z = 0; z < 2; z++)
                {
                    Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
                    Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                    int dus = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, v2.X, v2.Y, 100, default(Color), 1.8f);
                    Main.dust[dus].noGravity = false;
                    Main.dust[dus].velocity = v2;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dus].scale = 0.5f;

                        Main.dust[dus].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 35f)).RotatedByRandom(MathHelper.TwoPi);
                Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + v1, 0, 0, ModContent.DustType<LanternMoon.Dusts.Flame2 >(), v3.X, v3.Y, 0, default(Color), 9f);
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3;
                Main.dust[r].rotation = Main.rand.NextFloat(0, (float)(MathHelper.TwoPi));
            }
            for (int i = 0; i < 47; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 2.1f));
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 64 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    Player player = Main.player[Projectile.owner];
                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[0] / 100f)));
                }
            }
            for (int i = 0; i < 5; i++)
            {
                Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(0.7f, 2.2f)).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 1.6f));
            }
            //Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Melee.FireWave>(), 0, 0, Projectile.owner, 0f, 0f);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float K = Main.rand.NextFloat(0, (float)MathHelper.TwoPi);
            for (int j = 0; j < 5; j++)
            {
                Vector2 v = new Vector2(0, 15).RotatedBy(Math.PI * j / 2.5 + K);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.MachineFitMissile>(), Projectile.damage, 1, Main.myPlayer, Projectile.ai[0], 0f);
            }
            target.AddBuff(24, 600);
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/MachineFit_Glow").Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, spriteEffects, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}