using System;
using Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
	public class EvilSlingshotAmmo : SlingshotAmmo
	{
		public override void SetDef()
		{
		}
		public override void AI()
		{
			int r = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) + base.Projectile.velocity * 1.5f + new Vector2(-9, -4), 0, 0, 27, 0, 0, 0, default(Color), 1f);
			Main.dust[r].velocity.X = 0;
			Main.dust[r].velocity.Y = 0;
			Main.dust[r].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			base.Kill(timeLeft);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(153, 900);
		}
	}
}
