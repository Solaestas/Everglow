using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class Light : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RainbowLight");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "彩色光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 650;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        float damage = 100;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft == 649)
            {
                adding = Main.rand.NextFloat(0, 100f);
                Omega = Main.rand.NextFloat(-0.25f, 0.25f);

            }
            if (Projectile.timeLeft == 630)
            {
                float volum = 0f;
                if (Projectile.damage > 100f)
                {
                    volum = 1f;
                }
                else
                {
                    volum = Projectile.damage / 100f;
                }
                SoundEngine.PlaySound(SoundID.Zombie29.WithVolumeScale(volum), Projectile.Center); //sound was 29. Item 29 or Zombie 29?
            }
            if (Projectile.timeLeft > 500 && Projectile.timeLeft < 625)
            {
                if (Strengt < 1)
                {
                    Strengt += 0.02f;
                }
                else
                {
                    Strengt = 1;
                }
                if (Range < 170)
                {
                    Range += 2f;
                }
                else
                {
                    Range = 170;
                }
            }
            if (Projectile.timeLeft < 60)
            {
                if (Strengt > 0)
                {
                    Strengt -= 0.02f;
                }
                else
                {
                    Strengt = 0;
                }
            }
            Vector2 vz = Vector2.Zero;
            double rot = 0;
            for (int i = 0; i < 256; i++)
            {
                vz = (player.Center - (Projectile.Center - new Vector2(0, 320)));

                rot = Math.Atan2(vz.Y, vz.X) / Math.PI * 3;
            }


            float Ct = (Projectile.ai[1] % 1) * 1000f;
            if (Ct < 1.0717)//紫
            {
                P = true;
            }
            if (Ct >= 1.0717 && Ct < 1.9061)//红
            {
                R = true;
            }
            if (Ct >= 1.9061 && Ct < 2.5879)//橙
            {
                O = true;
            }
            if (Ct >= 2.5879 && Ct < 3.9904)//黄
            {
                Y = true;
            }
            if (Ct >= 3.9904 && Ct < 5.3973)//绿
            {
                G = true;
            }
            if (Ct >= 5.3973 && Ct < 6.1569)//青
            {
                C = true;
            }
            if (Ct >= 6.1569)//蓝
            {
                B = true;
            }
            MythContentPlayer myplayer = player.GetModPlayer<MythContentPlayer>();
            if (vz.Length() <= 650 && rot > 1 && rot < 2 && Projectile.timeLeft > 10)
            {
                if (inAdd == 0)
                {
                    inAdd += 1;
                    if (R)
                    {
                        myplayer.RainCritAdd += Projectile.damage / 4;
                    }
                    if (O)
                    {
                        myplayer.RainDefenseAdd += Projectile.damage / 3;
                    }
                    if (Y)
                    {
                        myplayer.RainDamageAdd += Projectile.damage / 100f;
                    }
                    if (G)
                    {
                        myplayer.RainLifeAdd += Projectile.damage / 3;
                    }
                    if (C)
                    {
                        myplayer.RainMissAdd += Projectile.damage / 8f;
                    }
                    if (B)
                    {
                        myplayer.RainManaAdd += Projectile.damage / 10f;
                    }
                    if (P && !Padd)
                    {
                        myplayer.RainSpeedAdd += Projectile.damage / 10f;
                        myplayer.RainSpeedAddTime = Projectile.timeLeft;
                        Padd = true;
                    }
                }
            }
            else
            {
                if (inAdd == 1)
                {
                    inAdd = 0;
                    if (R)
                    {
                        myplayer.RainCritAdd -= Projectile.damage / 4;
                    }
                    if (O)
                    {
                        myplayer.RainDefenseAdd -= Projectile.damage / 3;
                    }
                    if (Y)
                    {
                        myplayer.RainDamageAdd -= Projectile.damage / 100f;
                    }
                    if (G)
                    {
                        myplayer.RainLifeAdd -= Projectile.damage / 3;
                    }
                    if (C)
                    {
                        myplayer.RainMissAdd -= Projectile.damage / 8f;
                    }
                    if (B)
                    {
                        myplayer.RainManaAdd -= Projectile.damage / 10f;
                    }
                    if (P)
                    {
                        myplayer.RainSpeedAdd -= Projectile.damage / 10f;
                    }
                }
            }
        }
        bool R = false;
        bool O = false;
        bool Y = false;
        bool G = false;
        bool C = false;
        bool B = false;
        bool P = false;
        bool Padd = false;
        int inAdd = 0;
        public override void Kill(int timeLeft)
        {
            /*myplayer.RainCritAdd = 0;
            myplayer.RainDamageAdd = 0;
            myplayer.RainMissAdd = 0;
            myplayer.RainManaAdd = 0;
            myplayer.RainLifeAdd = 0;
            myplayer.RainSpeedAdd = 0;
            myplayer.RainDefenseAdd = 0;*/
        }
        float Strengt = 0;
        float adding;
        float Range = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Niddle = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Light").Value;


            for (int i = 0; i < 8; i++)
            {
                Vector2 v0 = Projectile.Center - new Vector2(0, 330);
                Vector2 v1 = new Vector2(0, Range).RotatedBy(i / 4d * Math.PI + adding);
                Vector2 v2 = new Vector2(v1.X, v1.Y / 3f);
                Vector2 v3 = Projectile.Center + v2;
                Vector2 v4 = v3 - v0;
                int R = (int)(Projectile.ai[0] * Strengt / 3f);
                int G = (int)((Projectile.ai[0] % 1) * 1000f * Strengt / 3f);
                int B = (int)(Projectile.ai[1] * Strengt / 3f);
                Main.EntitySpriteDraw(Niddle, v3 - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(R, G, B, 0), (float)Math.Atan2(v4.Y, v4.X), new Vector2(320f, 48f), 1.04f + (float)(Math.Sin(adding * 1.4f + i + 2)) * 0.16f, SpriteEffects.FlipHorizontally, 0);
            }
            for (int i = 0; i < 8; i++)
            {
                Vector2 v0 = Projectile.Center - new Vector2(0, 330);
                Vector2 v1 = new Vector2(0, Range).RotatedBy(i / 4d * Math.PI - adding * 1.6);
                Vector2 v2 = new Vector2(v1.X, v1.Y / 3f);
                Vector2 v3 = Projectile.Center + v2;
                Vector2 v4 = v3 - v0;
                int R = (int)((Projectile.ai[0] + 10) * Strengt / 3f);
                int G = (int)(((Projectile.ai[0] % 1) * 1000f - 10) * Strengt / 3f);
                int B = (int)((Projectile.ai[1] - 7) * Strengt / 3f);
                Main.EntitySpriteDraw(Niddle, v3 - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(R, G, B, 0), (float)Math.Atan2(v4.Y, v4.X), new Vector2(320f, 48f), 0.94f + (float)(Math.Sin(adding * 1.1f + i * 0.3)) * 0.14f, SpriteEffects.None, 0);
            }
            for (int i = 0; i < 8; i++)
            {
                Vector2 v0 = Projectile.Center - new Vector2(0, 330);
                Vector2 v1 = new Vector2(0, Range).RotatedBy(i / 4d * Math.PI + adding * 0.3 * (i + 1));
                Vector2 v2 = new Vector2(v1.X, v1.Y / 3f);
                Vector2 v3 = Projectile.Center + v2;
                Vector2 v4 = v3 - v0;
                int R = (int)((Projectile.ai[0] - 10) * Strengt / 3f);
                int G = (int)(((Projectile.ai[0] % 1) * 1000f + 10) * Strengt / 3f);
                int B = (int)((Projectile.ai[1] + 7) * Strengt / 3f);
                Main.EntitySpriteDraw(Niddle, v3 - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(R, G, B, 0), (float)Math.Atan2(v4.Y, v4.X), new Vector2(320f, 48f), 1 + (float)(Math.Sin(adding * 1.8f + i * 0.9)) * 0.17f, SpriteEffects.None, 0);
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 v0 = Projectile.Center - new Vector2(0, 330);
                Vector2 v1 = new Vector2(0, Range).RotatedBy(i / 2d * Math.PI + Omega * 0.3 * (i - 1.5f) - adding);
                Vector2 v2 = new Vector2(v1.X, v1.Y / 3f);
                Vector2 v3 = Projectile.Center + v2;
                Vector2 v4 = v3 - v0;
                int R = (int)((Projectile.ai[0] - 5) * Strengt / 3f);
                int G = (int)(((Projectile.ai[0] % 1) * 1000f + 9) * Strengt / 3f);
                int B = (int)((Projectile.ai[1] - 2) * Strengt / 3f);
                Main.EntitySpriteDraw(Niddle, v3 - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(R, G, B, 0), (float)Math.Atan2(v4.Y, v4.X), new Vector2(320f, 48f), 1 + (float)(Math.Sin(adding * 0.7f + i * 0.9 + Omega * 0.2)) * 0.2f, SpriteEffects.None, 0);
            }
            if (!Main.gamePaused)
            {
                adding += 0.016f;
                Omega += Main.rand.NextFloat(-0.04f, 0.04f);
                if (Math.Abs(Omega) > 0.35f)
                {
                    Omega *= 0.99f;
                }
                for (int i = 0; i < 256; i++)
                {
                    if (inAdd == 1 && Main.rand.NextBool(3))
                    {
                        int R = (int)((Projectile.ai[0] - 5) * Strengt / 3f);
                        int G = (int)(((Projectile.ai[0] % 1) * 1000f + 9) * Strengt / 3f);
                        int B = (int)((Projectile.ai[1] - 2) * Strengt / 3f);
                        int num90 = Dust.NewDust(new Vector2(Main.player[i].Center.X, Main.player[i].Center.Y) - new Vector2(37.5f, 27.5f), 60, 80, ModContent.DustType<MiscItems.Dusts.Buff>(), 0f, 0f, 0, new Color(R, G, B, 0), Main.rand.NextFloat(0.8f, 1.2f));
                        Main.dust[num90].noGravity = true;
                        Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(-6f, -3f));
                    }
                }
            }
            return false;
        }
        float Omega = 0.25f;
    }
}
