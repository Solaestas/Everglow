//using MythMod.Common.Players;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles.Weapon
{
    public class TuskPower : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk Staff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "獠牙法杖");
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
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
        float Addrot = 0;
        int PonitCount = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.itemTime = 5;
            player.itemAnimation = 5;
            Projectile.Center = player.Center;
            if (Main.mouseLeftRelease && neartokill == 0 && Projectile.timeLeft < 9990)
            {
                neartokill = 1;
            }
            if (neartokill == 0)
            {
                Addrot += 0.05f;
                PonitCount = 0;
                for (int f = 0; f < 12; f++)
                {
                    NewPos[f] = Vector2.Zero;
                    for (int st = 0; st < 70; st += 1)
                    {
                        Vector2 v0 = new Vector2(st, 0).RotatedBy(f / 6d * Math.PI + Addrot) + Main.MouseWorld;
                        if (Collision.SolidCollision(v0, 1, 1))
                        {
                            if (st > 5)
                            {
                                PonitCount++;
                                Dust.NewDust(v0 + new Vector2(4), 0, 0, DustID.Blood, 0, 0, 0, default, Stre);
                                NewPos[f] = v0;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                if (Stre < 1)
                {
                    Stre += 0.015f;
                }
                else
                {
                    Stre = 1;
                }
            }
            if (neartokill == 1)
            {
                for (int f = 0; f < 12; f++)
                {
                    if (NewPos[f] != Vector2.Zero)
                    {
                        Vector2 vx = NewPos[f] - Main.MouseWorld;
                        int h = Projectile.NewProjectile(Projectile.InheritSource(Projectile), NewPos[f], Vector2.Zero, ModContent.ProjectileType<Projectiles.TuskSpicePro>(), (int)(Stre * player.HeldItem.damage * 5), Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(22f, 27f) * Stre * 2.4f + 4, Stre);
                        float rot = (float)(Math.Atan2(vx.Y, vx.X) - Math.PI / 2d) + Main.rand.NextFloat(-0.6f, 0.6f);
                        Main.projectile[h].rotation = rot;
                        for (int z = 0; z < 10; z++)
                        {
                            int rss = Dust.NewDust(NewPos[f] + new Vector2(4), 0, 0, DustID.Blood, 0, 0, 0, default, Stre);
                            Main.dust[rss].velocity = new Vector2(Main.rand.NextFloat(0.5f, 7f), 0).RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f) + rot);
                        }
                    }
                }
                TuskModPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<TuskModPlayer>();
                mplayer.Shake = 2;
                mplayer.ShakeStrength = 1f;
                Projectile.Kill();
            }
        }
        public override void PostDraw(Color lightColor)
        {
        }
        private float Stre = 0;
        Vector2[] NewPos = new Vector2[12];
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 Vms = Main.MouseWorld;
            Vector2 Vdr = Vms - Projectile.Center;
            if (Main.mouseLeft)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(Vdr.Y, Vdr.X) - Math.PI / 2d));
            }
            player.heldProj = Projectile.whoAmI;
            Vdr = Vdr / Vdr.Length() * 7;
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Items/Weapons/ToothStaff").Value;
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
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition + Vdr * 4f, null, color, (float)(Math.Atan2(Vdr.Y, Vdr.X) + Math.PI / 4d), t.Size() / 2f, Projectile.scale, S, 0f);
            return true;
        }
    }
}
