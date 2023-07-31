using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles
{
    public class AzureOceanSpear2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("碧海长矛");
		}
        private bool initialization = true;
        private float X;
		public override void SetDefaults()
		{
            base.Projectile.width = 36;
            base.Projectile.height = 36;
            base.Projectile.aiStyle = 27;
            base.Projectile.friendly = true;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.ignoreWater = true;
            base.Projectile.penetrate = 5;
            base.Projectile.extraUpdates = 1;
            base.Projectile.timeLeft = 480;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 1;
		}
		public override void AI()
		{
            if (initialization)
            {
                X = Main.rand.Next(0,10);
				initialization = false;
            }
			Lighting.AddLight(base.Projectile.Center, 0.24f, 0f, 0.04f);
			if (base.Projectile.localAI[1] > 70f)
			{
				int num = Dust.NewDust(new Vector2((float)base.Projectile.position.X + 18f,(float)base.Projectile.position.Y + 15f), 0, 0, 277, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 150, Color.White, 1.2f);
				Main.dust[num].noGravity = true;
			}
            int num2 = Dust.NewDust(new Vector2((float)base.Projectile.position.X + 18f,(float)base.Projectile.position.Y + 15f), 0, 0, 56, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 150, Color.White, 1.2f);
            if (Projectile.timeLeft % 3 == 0)
            {
                int num1 = Dust.NewDust(new Vector2((float)base.Projectile.position.X + 18f, (float)base.Projectile.position.Y + 15f), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 150, Color.White, 1f);
                Main.dust[num1].noGravity = true;
            }
            Main.dust[num2].noGravity = true;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
            Main.EntitySpriteDraw(texture2D, base.Projectile.Center - Main.screenPosition, null, base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, Utils.Size(texture2D) / 2f, base.Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
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
            for (int j = 0; j < 90; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), 0f, 0f, 100, default(Color), 0.8f);
                Main.dust[num2].velocity.X = (float)(5f * Math.Sin(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(5f * Math.Cos(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 90; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave2>(), 0f, 0f, 100, default(Color), 2.4f);
                Main.dust[num2].velocity.X = (float)(5f * Math.Sin(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(5f * Math.Cos(Math.PI * (float)(j) / 45f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 60; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), 0f, 0f, 100, default(Color), 0.5f);
                Main.dust[num2].velocity.X = (float)(3f * Math.Sin(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(0.99f, 1.01f);
                Main.dust[num2].velocity.Y = (float)(3f * Math.Cos(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(0.99f, 1.01f);
            }
            for (int j = 0; j < 60; j++)
            {
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Wave2>(), 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num2].velocity.X = (float)(3f * Math.Sin(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(1f, 1.1f);
                Main.dust[num2].velocity.Y = (float)(3f * Math.Cos(Math.PI * (float)(j) / 30f)) * Main.rand.NextFloat(1f, 1.1f);
            }
            for (int j = 0; j < 200; j++)
            {
                if (!Main.npc[j].dontTakeDamage && (Main.npc[j].Center - Projectile.Center).Length() < 60f && !Main.npc[j].friendly)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 100 / (Main.npc[j].Center - Projectile.Center).Length(), (int)((Main.npc[j].Center.X - Projectile.Center.X) / Math.Abs(Main.npc[j].Center.X - Projectile.Center.X)));
                }
            }
        }
    }
}
