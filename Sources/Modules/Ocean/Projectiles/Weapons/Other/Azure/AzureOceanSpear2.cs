using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles
{
    public class AzureOceanSpear2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("碧海长矛");
		}
        private bool initialization = true;
        private float X;
		public override void SetDefaults()
		{
            base.projectile.width = 36;
            base.projectile.height = 36;
            base.projectile.aiStyle = 27;
            base.projectile.friendly = true;
            base.projectile.melee = true;
            base.projectile.ignoreWater = true;
            base.projectile.penetrate = 5;
            base.projectile.extraUpdates = 1;
            base.projectile.timeLeft = 480;
            base.projectile.usesLocalNPCImmunity = true;
            base.projectile.localNPCHitCooldown = 1;
		}
		public override void AI()
		{
            if (initialization)
            {
                X = Main.rand.Next(0,10);
				initialization = false;
            }
			Lighting.AddLight(base.projectile.Center, 0.24f, 0f, 0.04f);
			if (base.projectile.localAI[1] > 70f)
			{
				int num = Dust.NewDust(new Vector2((float)base.projectile.position.X + 18f,(float)base.projectile.position.Y + 15f), 0, 0, 277, base.projectile.velocity.X * 0.5f, base.projectile.velocity.Y * 0.5f, 150, Color.White, 1.2f);
				Main.dust[num].noGravity = true;
			}
            int num2 = Dust.NewDust(new Vector2((float)base.projectile.position.X + 18f,(float)base.projectile.position.Y + 15f), 0, 0, 56, base.projectile.velocity.X * 0.5f, base.projectile.velocity.Y * 0.5f, 150, Color.White, 1.2f);
            if (projectile.timeLeft % 3 == 0)
            {
                int num1 = Dust.NewDust(new Vector2((float)base.projectile.position.X + 18f, (float)base.projectile.position.Y + 15f), 0, 0, mod.DustType("Wave"), base.projectile.velocity.X * 0.5f, base.projectile.velocity.Y * 0.5f, 150, Color.White, 1f);
                Main.dust[num1].noGravity = true;
            }
            Main.dust[num2].noGravity = true;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D = Main.projectileTexture[base.projectile.type];
            spriteBatch.Draw(texture2D, base.projectile.Center - Main.screenPosition, null, base.projectile.GetAlpha(lightColor), base.projectile.rotation, Utils.Size(texture2D) / 2f, base.projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
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
            for (int j = 0; j < 90; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave"), 0f, 0f, 100, default(Color), 0.8f);
                Main.dust[num2].velocity.X = (float)(5f * Math.Sin(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(5f * Math.Cos(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 90; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave2"), 0f, 0f, 100, default(Color), 2.4f);
                Main.dust[num2].velocity.X = (float)(5f * Math.Sin(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(5f * Math.Cos(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 60; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave"), 0f, 0f, 100, default(Color), 0.5f);
                Main.dust[num2].velocity.X = (float)(3f * Math.Sin(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(3f * Math.Cos(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 60; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 0, 0, mod.DustType("Wave2"), 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num2].velocity.X = (float)(3f * Math.Sin(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(3f * Math.Cos(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 200; j++)
            {
                if (!Main.npc[j].dontTakeDamage && (Main.npc[j].Center - projectile.Center).Length() < 60f && !Main.npc[j].friendly)
                {
                    Main.npc[j].StrikeNPC((int)(projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 100 / (Main.npc[j].Center - projectile.Center).Length(), (int)((Main.npc[j].Center.X - projectile.Center.X) / Math.Abs(Main.npc[j].Center.X - projectile.Center.X)));
                }
            }
        }
    }
}
