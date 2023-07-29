using System;
using Everglow.Myth.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Weapons.Other
{
	public class StarMarkYoyo : ModProjectile
	{
		// Are these needed?
		private bool M = false;
        private float X = 0;
		public override void SetDefaults()
		{
			base.Projectile.CloneDefaults(547);
            base.Projectile.width = 16;
			base.Projectile.height = 16;
			base.Projectile.scale = 1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;
        }
		public override void AI()
		{
            ProjectileExtras.YoyoAI(base.Projectile.whoAmI, 60f, 300f, 14f);
            if(Main.rand.Next(15) == 1)
            {
                int num = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, ModContent.DustType<Dusts.Star>(), 0f, 0f, 100, default(Color), 0.8f);
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale = 0.5f;
                    Main.dust[num].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(153, 300);
        }
        public override void PostDraw(Color lightColor)
        {
            Main.EntitySpriteDraw((Texture2D)ModContent.Request<Texture2D>("Ocean/Projectiles/Weapons/Other/StarMarkYoyoGlow"), base.Projectile.Center - Main.screenPosition, null, Color.White * 0.7f, base.Projectile.rotation, new Vector2(8f, 8f), 1f, SpriteEffects.None, 0f);
        }
    }
}
