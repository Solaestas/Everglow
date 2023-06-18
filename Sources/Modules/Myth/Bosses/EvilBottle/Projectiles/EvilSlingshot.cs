using System;
using Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class EvilSlingshot : SlingshotProjectile
	{
		//public override void SetStaticDefaults()
		//{
  //          // base.DisplayName.SetDefault("妖火弹球");
		//}
		//public override void SetDefaults()
		//{
		//	base.Projectile.width = 20;
		//	base.Projectile.height = 20;
		//	base.Projectile.friendly = true;
		//	base.Projectile.DamageType = DamageClass.Melee;
		//	base.Projectile.penetrate = 15;
		//	base.Projectile.aiStyle = 1;
		//	base.Projectile.timeLeft = 1200;
  //          base.Projectile.hostile = false;
  //          Projectile.penetrate = 1;

  //      }

		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<EvilSlingshotAmmo>();
			SlingshotLength = 8;
			SplitBranchDis = 8;
		}
		//public override void AI()
		//{
  //          int r = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) + base.Projectile.velocity * 1.5f + new Vector2(-9, -4), 0, 0, 27, 0, 0, 0, default(Color), 1f);
  //          Main.dust[r].velocity.X = 0;
  //          Main.dust[r].velocity.Y = 0;
  //          Main.dust[r].noGravity = true;
  //      }
  //      public override void Kill(int timeLeft)
  //      {
  //      }
  //      public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
  //      {
  //          target.AddBuff(153, 900);
  //      }
  //      public int projTime = 15;
	}
}
