using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Weapons.Other
{
    public class 橄榄石长枪2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("橄榄石长枪");
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
			Lighting.AddLight(base.Projectile.Center, 0.25f, 0.65f, 0f);
			if (base.Projectile.localAI[1] > 70f)
			{
				int num = Dust.NewDust(new Vector2((float)base.Projectile.position.X + 18f,(float)base.Projectile.position.Y + 15f), (int)(Projectile.velocity.X * 0.4f), (int)(Projectile.velocity.Y * 0.4f), Mod.Find<ModDust>("Olivine").Type, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 150, Color.White, 1.2f);
				Main.dust[num].noGravity = true;
			}
            int num2 = Dust.NewDust(new Vector2((float)base.Projectile.position.X + 18f,(float)base.Projectile.position.Y + 15f), (int)(Projectile.velocity.X * 0.3f), (int)(Projectile.velocity.Y * 0.3f), Mod.Find<ModDust>("Olivine").Type, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 150, Color.White, 1.2f);
            int num1 = Dust.NewDust(new Vector2((float)base.Projectile.position.X + 18f,(float)base.Projectile.position.Y + 15f), (int)(Projectile.velocity.X * 0.5f), (int)(Projectile.velocity.Y * 0.5f), Mod.Find<ModDust>("Olivine").Type, base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 150, Color.White, 1f);
            Main.dust[num1].noGravity = true;
            Main.dust[num2].noGravity = true;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
            Main.spriteBatch.Draw(texture2D, base.Projectile.Center - Main.screenPosition, null, base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, Utils.Size(texture2D) / 2f, base.Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft > 60)
            {
                return new Color?(new Color(255, 255, 255, 0));
            }
            else
            {
                return new Color?(new Color(1 * Projectile.timeLeft / 60f, 1 * Projectile.timeLeft / 60f, 1 * Projectile.timeLeft / 60f, 0));
            }
        }
    }
}
