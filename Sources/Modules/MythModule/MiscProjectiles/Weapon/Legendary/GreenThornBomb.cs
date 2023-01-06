//using MythMod.Common.Players;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class GreenThornBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Thorn Bomb");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "青刺球炸弹");
            Main.projFrames[Projectile.type] = 5;
        }
        private float num = 0;
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = -1;
        }
        float timer = 0;
        static float j = 0;
        static float m = 0.15f;
        static float n = 0;
        private bool x = false;
        private bool HitBoom = false;
        private bool stick = false;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            Projectile.rotation = (float)System.Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f + num;
            if (stick && md.active)
            {
                Projectile.velocity = md.velocity;
            }
            if (stick && !md.active)
            {
                stick = false;
            }
            if (Projectile.timeLeft <= 250 && !x)
            {
                num += 0.15f;
            }
            if (x)
            {
                num += m;
                if (m > 0)
                {
                    m -= 0.005f;
                }
                else
                {
                    m = 0;
                }
            }
            if (Projectile.velocity.Y < 15f && !x && !stick)
            {
                Projectile.velocity.Y += 0.2f;
            }
            if (Projectile.timeLeft <= 20)
            {
                if (Projectile.timeLeft % 5 == 2)
                {
                    if (Projectile.frame < 4)
                    {
                        Projectile.frame += 1;
                    }
                    else
                    {
                        Projectile.Kill();
                    }
                }
                S += 0.05f;
                Projectile.scale += 0.05f;
            }
            //Dust.NewDust(new Vector2((float)Projectile.Center.X, (float)Projectile.Center.Y) + new Vector2(0, -10).RotatedBy(Projectile.rotation), 0, 0, 6, 0f, 0f, 0, default(Color), 1f);
        }
        private float S = 0;
        public override void PostDraw(Color lightColor)
        {
            Texture2D Tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/GreenThornBomb").Value;
            for (int i = 0; i < S * 8; i++)
            {
                Main.spriteBatch.Draw(Tex, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 60, 38, 60), new Color(S, S, S, 0), Projectile.rotation, new Vector2(19f, 19f), Projectile.scale, SpriteEffects.None, 0f);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            stick = true;
            v = Projectile.position - target.position;
            Projectile.friendly = false;
            md = target;
            if (!HitBoom)
            {
                Projectile.timeLeft = 20;
            }
        }
        private NPC md = Main.npc[0];
        private Vector2 v = new Vector2(0, 0);
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.Length() > 0.5f)
                {
                    if (Projectile.velocity.X != oldVelocity.X)
                    {
                        Projectile.velocity.X = -oldVelocity.X;
                    }
                    if (Projectile.velocity.Y != oldVelocity.Y)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y;
                    }
                }
                else
                {
                    Projectile.velocity.Y *= 0;
                    Projectile.velocity.X *= 0;
                    x = true;
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
			//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
			//mplayer.Shake = 3;
			ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
			mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);
			float Str = 1;
            Player player = Main.LocalPlayer;
            if ((player.Center - Projectile.Center).Length() > 100)
            {
                Str = 100 / (player.Center - Projectile.Center).Length();
            }
			mplayer.DirFlyCamPosStrength = Str;
			SoundEngine.PlaySound(SoundID.Item36, Projectile.Center);
            for (int j = 0; j < 15; j++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(6.2f, 7f)).RotatedByRandom(Math.PI * 2) * Main.rand.Next(1500, 2000) / 1000f;
                int zi = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + v * 3f, v, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.Cyanline>(), (int)((double)Projectile.damage) / 2, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            for (int i = 0; i < 47; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 188, v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0.8f, 5f));
            }
            for (int i = 0; i < 90; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2.9f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<MiscDusts.Poison>(), 0f, 0f, 100, Color.White, (float)(4f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v;
            }
            for (int i = 0; i < 60; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) + v * 20f, Projectile.width, Projectile.height, ModContent.DustType<MiscDusts.Poison>(), 0f, 0f, 100, Color.White, (float)(12f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v * 0.1f;
                Main.dust[num5].rotation = Main.rand.NextFloat(0, (float)(MathHelper.TwoPi));
            }
            for (int i = 0; i < 27; i++)
            {
                Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 3.2f));
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 150 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 2.35f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    Player player2 = Main.player[Projectile.owner];
                    player2.dpsDamage += (int)(Projectile.damage * (1 + Projectile.ai[0] / 100f) * 1.6f);
                }
            }
        }
    }
}
