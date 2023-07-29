using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Ocean.Projectiles
{
    public class WaveBallMini : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("º£ÀËÇò");
            Main.projFrames[Projectile.type] = 4; 

        }
        public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, base.Projectile.alpha));
		}
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 75;
            Projectile.scale = 0.5f;
        }
        float timer = 0;
        static float j = 0;
        static float m = 0;
        static float n = 0;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            base.Projectile.frameCounter++;
            if (base.Projectile.frameCounter > 4 && Projectile.timeLeft % 3 == 0)
			{
				base.Projectile.frame++;
				base.Projectile.frameCounter = 0;
			}
			if (base.Projectile.frame > 3)
			{
				base.Projectile.frame = 0;
			}
            #region
            if (Projectile.timeLeft == 710) { Projectile.tileCollide = true; }//µ±ÄãµÄÌØÐ§´æÔÚ710ìõÊ±£¬²»ÄÜ´©Ç½
            Projectile.rotation = (float)System.Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;//ÈÃÄãµÄÌØÐ§Õý³£»¯
            Projectile.light = 0.1f;//·¢¹â//0Îª²»·¢¹â
            Vector2 pc = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2;//ÈÃÄãµÄÌØÐ§Õý³£»¯
            Projectile.light = 0.1f;//·¢¹â
            #endregion
            if (Projectile.timeLeft % 5 == 0 && Projectile.timeLeft < 895)
            {
                int dustID = Dust.NewDust(Projectile.position, (int)(Projectile.width / 2f), (int)(Projectile.height / 2f), ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f ,201, Color.White , 1.5f);/*·Û³¾Ð§¹û²»ÓÃ¹Ü*/
                int dustID2 = Dust.NewDust(Projectile.position, (int)(Projectile.width / 2f), (int)(Projectile.height / 2f), 56, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f ,201, Color.White , 1f);/*·Û³¾Ð§¹û²»ÓÃ¹Ü*/
                int dustID3 = Dust.NewDust(Projectile.position, (int)(Projectile.width / 2f), (int)(Projectile.height / 2f), ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f ,201, Color.White , 1f);/*·Û³¾Ð§¹û²»ÓÃ¹Ü*/
                Main.dust[dustID].noGravity = true;
            }
            Projectile.velocity *= 0.97f;
        }
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
			int num = TextureAssets.Projectile[base.Projectile.type].Value.Height / Main.projFrames[base.Projectile.type];
			int y = num * base.Projectile.frame;
			Main.spriteBatch.Draw(texture2D, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)num / 2f), base.Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14.WithVolumeScale(0.36f), new Vector2(base.Projectile.position.X, base.Projectile.position.Y));
            base.Projectile.position.X = base.Projectile.position.X + (float)(base.Projectile.width / 2);
            base.Projectile.position.Y = base.Projectile.position.Y + (float)(base.Projectile.height / 2);
            base.Projectile.width = 40;
            base.Projectile.height = 40;
            base.Projectile.position.X = base.Projectile.position.X - (float)(base.Projectile.width / 2);
            base.Projectile.position.Y = base.Projectile.position.Y - (float)(base.Projectile.height / 2);
            for (int j = 0; j < 30; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num2].velocity.X = (float)(1.5f * Math.Sin(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(1.5f * Math.Cos(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 30; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave2>(), 0f, 0f, 100, default(Color), 4.5f);
                Main.dust[num2].velocity.X = (float)(1.5f * Math.Sin(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(1.5f * Math.Cos(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 20; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), 0f, 0f, 100, default(Color), 0.7f);
                Main.dust[num2].velocity.X = (float)(1f * Math.Sin(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(1f * Math.Cos(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 20; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave2>(), 0f, 0f, 100, default(Color), 2.1f);
                Main.dust[num2].velocity.X = (float)(1f * Math.Sin(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(1f * Math.Cos(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 10; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), 0f, 0f, 100, default(Color), 0.4f);
                Main.dust[num2].velocity.X = (float)(0.5f * Math.Sin(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(0.5f * Math.Cos(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 10; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave2>(), 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[num2].velocity.X = (float)(0.5f * Math.Sin(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(0.5f * Math.Cos(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 200; j++)
            {
                if(!Main.npc[j].dontTakeDamage && (Main.npc[j].Center - Projectile.Center).Length() < 40f && !Main.npc[j].friendly)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f,1.15f)),100 / (Main.npc[j].Center - Projectile.Center).Length(),(int)((Main.npc[j].Center.X - Projectile.Center.X) / Math.Abs(Main.npc[j].Center.X - Projectile.Center.X)));
                }
            }
        }
    }
}
