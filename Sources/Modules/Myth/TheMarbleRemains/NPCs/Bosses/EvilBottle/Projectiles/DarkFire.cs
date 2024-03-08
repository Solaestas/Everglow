using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
	public class DarkFire : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("紫冥鬼火");
            Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 20;
			base.Projectile.height = 30;
			base.Projectile.friendly = false;
            Projectile.hostile = true;
            base.Projectile.alpha = 255;
			base.Projectile.penetrate = 10;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 9000;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0));
        }
        public float num2 = 0;
        public override void AI()
        {
            if(Projectile.timeLeft == 8999)
            {
                Projectile.timeLeft = Main.rand.Next(180, 240);
            }
            if(num2 == 0)
            {
                num2 = Main.rand.Next(-100, 100) / 1000f;
            }
            base.Projectile.frameCounter++;
            if (base.Projectile.frameCounter > 6)
            {
                base.Projectile.frame++;
                base.Projectile.frameCounter = 0;
            }
            if (base.Projectile.frame > 3)
            {
                base.Projectile.frame = 0;
            }
            if (Projectile.timeLeft >= 60)
            {
                if (Projectile.alpha >= 5)
                {
                    Projectile.alpha -= 5;
                }
                Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.12f / 255f , (float)(255 - base.Projectile.alpha) * 0f / 255f, (float)(255 - base.Projectile.alpha) * 0.48f / 255f);
            }
            else
            {
                if (Projectile.alpha <= 250)
                {
                    Projectile.alpha += 5;
                }
                Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.12f / 255f * Projectile.timeLeft / 60f, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.timeLeft / 60f, (float)(255 - base.Projectile.alpha) * 0.48f / 255f * Projectile.timeLeft / 60f);
            }
            int pl = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
            if(Main.player[pl].velocity.Length() > 0)
            {
                Projectile.velocity += (Main.player[pl].Center - Projectile.Center) / (Main.player[pl].Center - Projectile.Center).Length() * Main.player[pl].velocity.Length() * 0.024f;
            }
            Projectile.velocity *= 0.96f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.timeLeft = 60;
            Projectile.velocity *= 0;
            return false;
        }
        /*public override Color? GetAlpha(Color lightColor)
        {
            if (projectile.timeLeft > 60)
            {
                return new Color?(new Color(1f,1f,1f,1f));
            }
            else
            {
                return new Color?(new Color(1f * projectile.timeLeft / 60f, 1f * projectile.timeLeft / 60f, 1f * projectile.timeLeft / 60f, 1f * projectile.timeLeft / 60f));
            }
        }*/
        /*public override void Kill(int timeLeft)
		{
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 37, 0.5f, 0f);
        }*/
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
            int num = TextureAssets.Projectile[base.Projectile.type].Value.Height / Main.projFrames[base.Projectile.type];
            int y = num * base.Projectile.frame;
            Main.spriteBatch.Draw(texture2D, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY + Projectile.height / 2f), new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)num / 2f), base.Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
