using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace MythMod.Projectiles
{
    public class WaveBallMini : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("海浪球");
            Main.projFrames[projectile.type] = 4; 

        }
        public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, base.projectile.alpha));
		}
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.aiStyle = -1;
            projectile.timeLeft = 75;
            projectile.scale = 0.5f;
        }
        float timer = 0;
        static float j = 0;
        static float m = 0;
        static float n = 0;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            base.projectile.frameCounter++;
            if (base.projectile.frameCounter > 4 && projectile.timeLeft % 3 == 0)
			{
				base.projectile.frame++;
				base.projectile.frameCounter = 0;
			}
			if (base.projectile.frame > 3)
			{
				base.projectile.frame = 0;
			}
            #region
            if (projectile.timeLeft == 710) { projectile.tileCollide = true; }//当你的特效存在710祯时，不能穿墙
            projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;//让你的特效正常化
            projectile.light = 0.1f;//发光//0为不发光
            Vector2 pc = projectile.position + new Vector2(projectile.width, projectile.height) / 2;//让你的特效正常化
            projectile.light = 0.1f;//发光
            #endregion
            if (projectile.timeLeft % 5 == 0 && projectile.timeLeft < 895)
            {
                int dustID = Dust.NewDust(projectile.position, (int)(projectile.width / 2f), (int)(projectile.height / 2f), mod.DustType("Wave"), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f ,201, Color.White , 1.5f);/*粉尘效果不用管*/
                int dustID2 = Dust.NewDust(projectile.position, (int)(projectile.width / 2f), (int)(projectile.height / 2f), 56, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f ,201, Color.White , 1f);/*粉尘效果不用管*/
                int dustID3 = Dust.NewDust(projectile.position, (int)(projectile.width / 2f), (int)(projectile.height / 2f), mod.DustType("Wave"), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f ,201, Color.White , 1f);/*粉尘效果不用管*/
                Main.dust[dustID].noGravity = true;
            }
            projectile.velocity *= 0.97f;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[base.projectile.type];
			int num = Main.projectileTexture[base.projectile.type].Height / Main.projFrames[base.projectile.type];
			int y = num * base.projectile.frame;
			Main.spriteBatch.Draw(texture2D, base.projectile.Center - Main.screenPosition + new Vector2(0f, base.projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), base.projectile.GetAlpha(lightColor), base.projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)num / 2f), base.projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 14, 0.36f, 0f);
            base.projectile.position.X = base.projectile.position.X + (float)(base.projectile.width / 2);
            base.projectile.position.Y = base.projectile.position.Y + (float)(base.projectile.height / 2);
            base.projectile.width = 40;
            base.projectile.height = 40;
            base.projectile.position.X = base.projectile.position.X - (float)(base.projectile.width / 2);
            base.projectile.position.Y = base.projectile.position.Y - (float)(base.projectile.height / 2);
            for (int j = 0; j < 30; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave"), 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num2].velocity.X = (float)(1.5f * Math.Sin(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(1.5f * Math.Cos(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 30; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave2"), 0f, 0f, 100, default(Color), 4.5f);
                Main.dust[num2].velocity.X = (float)(1.5f * Math.Sin(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(1.5f * Math.Cos(Math.PI * (float)(j) / 15f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 20; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave"), 0f, 0f, 100, default(Color), 0.7f);
                Main.dust[num2].velocity.X = (float)(1f * Math.Sin(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(1f * Math.Cos(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 20; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave2"), 0f, 0f, 100, default(Color), 2.1f);
                Main.dust[num2].velocity.X = (float)(1f * Math.Sin(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(1f * Math.Cos(Math.PI * (float)(j) / 10f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 10; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave"), 0f, 0f, 100, default(Color), 0.4f);
                Main.dust[num2].velocity.X = (float)(0.5f * Math.Sin(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(0.5f * Math.Cos(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 10; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave2"), 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[num2].velocity.X = (float)(0.5f * Math.Sin(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(0.5f * Math.Cos(Math.PI * (float)(j) / 5f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 200; j++)
            {
                if(!Main.npc[j].dontTakeDamage && (Main.npc[j].Center - projectile.Center).Length() < 40f && !Main.npc[j].friendly)
                {
                    Main.npc[j].StrikeNPC((int)(projectile.damage * Main.rand.NextFloat(0.85f,1.15f)),100 / (Main.npc[j].Center - projectile.Center).Length(),(int)((Main.npc[j].Center.X - projectile.Center.X) / Math.Abs(Main.npc[j].Center.X - projectile.Center.X)));
                }
            }
        }
    }
}
