using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles
{
	public class WaveSawPro : ModProjectile
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 22;
			base.Projectile.height = 22;
			base.Projectile.aiStyle = 20;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.hide = true;
			base.Projectile.ownerHitCheck = true;
			base.Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			int num = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 33, base.Projectile.velocity.X * 0.2f, base.Projectile.velocity.Y * 0.2f, 100, default(Color), 1.2f);
			Main.dust[num].noGravity = true;
		}
	}
}
