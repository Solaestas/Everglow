using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Everglow.Myth.Common;
using Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Dusts;
using Everglow.Myth.MiscItems.Projectiles.Typeless;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
    public class SuperDarkFlameball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("超级恶魔火球");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3600;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            this.CooldownSlot = 1;
        }
        float r = 10;
        private Vector2 v0;
        private int Fra = 0;
        private int FraX = 0;
        private int FraY = 0;
        public override void AI()
        {
            base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) - (float)Math.PI * 0.5f;
            if (Projectile.timeLeft < 3595)
            {
                Vector2 vector = base.Projectile.Center;
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, ModContent.DustType<DarkF>(), 50f, 50f, 0, default(Color), (float)Projectile.scale * 2.4f);
                Main.dust[num].velocity *= 0.0f;
                Main.dust[num].noGravity = true;
                Main.dust[num].alpha = 150;
            }
            if(Projectile.velocity.Y < 15)
            {
                Projectile.velocity.Y += 0.2f;
            }
            if (Projectile.timeLeft == 3600)
            {
                v0 = Projectile.Center;
            }
            if (Projectile.timeLeft > 3540)
            {
                r += 0.2f;
            }
            if (Projectile.timeLeft <= 3540 && Projectile.timeLeft >= 60)
            {
                r = 24 + (float)(0.4f * Math.Sin((Projectile.timeLeft - 60) / 60d * Math.PI));
            }
            if (Projectile.timeLeft < 60 && r > 0.5f)
            {
                r -= 1f;
            }
            int Dx = (int)(r * 1.5f);
            int Dy = (int)(r * 1.5f);
            Fra = ((3600 - Projectile.timeLeft) / (int)3) % 30;
            FraX = (Fra % 6) * 270;
            FraY = (Fra / (int)6) * 290;
            /*if (v0 != Vector2.Zero)
            {
                projectile.position = v0 - new Vector2(Dx, Dy) / 2f;
            }*/
            if(Projectile.timeLeft < 3480)
            {
                Projectile.tileCollide = true;
            }
            Projectile.width = Dx;
            Projectile.height = Dy;
        }
        public override void Kill(int timeLeft)
        {
			SoundStyle 烟花爆炸 = new SoundStyle("Everglow/Myth/Sounds/烟花爆炸");
			烟花爆炸.MaxInstances = 10;

			int pl = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
            SoundEngine.PlaySound(烟花爆炸, (int)Projectile.Center.X, (int)Projectile.Center.Y);
            for (int i = 0; i < 160; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(1.9f, (float)(7 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, ModContent.DustType<DarkF2>(), 0f, 0f, 100, Color.White, (float)(10f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v;
            }
            float D = (Main.player[pl].Center - Projectile.Center).Length();
            if (D < 150)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.player[pl].Center.X, Main.player[pl].Center.Y, 0, 0, ModContent.ProjectileType<Hit>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
            }
            r = 200;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 150));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(r < 100)
            {
                Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Projectiles/DarkLazerBall"), base.Projectile.Center - Main.screenPosition, new Rectangle(FraX, 10 + FraY, 270, 270), new Color(1f, 1f, 1f, 0), base.Projectile.rotation, new Vector2(135f, 135f), r / 60f, SpriteEffects.None, 0f);
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}