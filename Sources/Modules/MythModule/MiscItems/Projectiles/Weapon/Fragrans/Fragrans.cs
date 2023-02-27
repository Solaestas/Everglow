namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
    class Fragrans : ModProjectile
    {
		public static int FragransIndex = 1;
		public static int FragCritAdd = 0;
		public static float FragDamageAdd = 0;
		public static int IMMUNE = 0;
		public static int Dashcool = 0;
		public override void SetDefaults()
        {
            Projectile.width = 116;
            Projectile.height = 116;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 302;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((int)(100 * fade), (int)(100 * fade), (int)(100 * fade), 0));
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.position = player.Center - new Vector2(58);
            if (Reset > 0)
            {
                if (Projectile.timeLeft < Reset)
                {
                    Projectile.timeLeft = Reset;
                }
                if (power > 7500)
                {
                    player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransIII>(), Projectile.timeLeft);
                }
                else
                {
                    if (power > 2400)
                    {
                        player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransII>(), Projectile.timeLeft);
                    }
                    else
                    {
                        player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragrans>(), Projectile.timeLeft);
                    }
                }
                if (!Dummy)
                {
                    power += Reset * 2f;
                }
                else
                {
                    Dummy = false;
                }
                Reset = 0;
            }
            if (Projectile.timeLeft < 60)
            {
                fade = Projectile.timeLeft / 60f;
            }
            else
            {
                fade = 1;
            }
            if (Projectile.timeLeft == 299)
            {
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Weapon.Fragrans.Fragrans2>(), 0, 0, player.whoAmI, 0, 0);
            }
            if (Projectile.timeLeft == 301)
            {
                MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragDamageAdd += 0.25f;
                MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragCritAdd += 15;
            }
            if (power >= 4)
            {
                power -= 4;
            }
            else
            {
                power = 0;
            }
            if (power > 2400)
            {
                if (power > 7500)
                {
                    if (MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex < 3)
                    {
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex = 3;
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragDamageAdd += 0.25f;
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragCritAdd += 15;
                        player.ClearBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransII>());
                        player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransIII>(), Projectile.timeLeft);
                    }
                    MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex = 3;
                }
                else
                {
                    if (MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex < 2)
                    {
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex = 2;
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragDamageAdd += 0.25f;
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragCritAdd += 15;
                        player.ClearBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragrans>());
                        player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransII>(), Projectile.timeLeft);
                    }
                    if (MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex > 2)
                    {
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex = 2;
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragDamageAdd -= 0.25f;
                        MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragCritAdd -= 15;
                        player.ClearBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransIII>());
                        player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransII>(), Projectile.timeLeft);
                    }
                    MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex = 2;
                }
            }
            else
            {
                if (MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex == 2)
                {
                    MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex = 1;
                    MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragDamageAdd -= 0.25f;
                    MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragCritAdd -= 15;
                    player.ClearBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragransII>());
                    player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragrans>(), Projectile.timeLeft);
                }
                MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex = 1;
            }

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Fragrans>()] > 1)
            {
                Projectile.Kill();
            }
            timer++;
        }
        float fade = 1;
        int timer = 0;
        public static bool Dummy = false;
        public override void Kill(int timeLeft)
        {
            MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragDamageAdd = 0;
            MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragCritAdd = 0;
            power = 0;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Fragrans/Fragrans").Value;
            float T = Projectile.timeLeft / 24f;
            float fT = 10000 - Projectile.timeLeft / 4f;
            for (int i = 0; i < 7; i++)
            {
                float Cl = (float)(8 * Math.Sin(T + i));
                Vector2 v = new Vector2(0, Cl).RotatedBy(T * 2 + i);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + v, new Rectangle(0, 0, 116, 116), new Color((int)(10 * fade), (int)(10 * fade), (int)(10 * fade), 0), Projectile.rotation + (float)Main.time / 30f, new Vector2(58), 1f, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < 4; i++)
            {
                float Dou = ((fT + i * 15) % 60) / 30f + 0.55f;
                float shade = 1.85f - Dou;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 116, 116), new Color((int)(10 * fade * shade), (int)(10 * fade * shade), (int)(10 * fade * shade), 0), Projectile.rotation + (float)Main.time / 30f, new Vector2(58), Dou, SpriteEffects.None, 0f);
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Fragrans/Fragransflower").Value;
            for (int i = 0; i < 24; i++)
            {
                float Cl = (float)(8 * Math.Sin(timer / 30f + i));
                Vector2 v = new Vector2(0, Cl).RotatedBy(timer / 15f + i);
                Vector2 v1 = new Vector2(0, -75).RotatedBy(i / 12f * Math.PI);
                float Val = power - i * 100;
                if (Val > 100)
                {
                    Val = 1;
                }
                else
                {
                    if (Val >= 0)
                    {
                        Val *= 0.01f;
                    }
                    else
                    {
                        Val = 0;
                    }
                }
                Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition + v + v1, new Rectangle(0, 0, 24, 24), new Color((int)(Val * 200f * fade), (int)(Val * 200f * fade), (int)(Val * 200f * fade), 100), T + i, new Vector2(12), 0.25f + Val / 4f, SpriteEffects.None, 0f);
            }
        }
        public static int Reset = 0;
        public static float power = 0;
    }
}
