using Everglow.Commons.Weapons.StabbingSwords;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class EternalNight_shadow : ModProjectile
    {
        public override void SetDefaults()
        {
			Projectile.friendly= true;
			Projectile.hostile = false;
			Projectile.timeLeft = 240;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
		}
		private int timeToKill = 0;
		public int targetNPC = -1;
		public int stickNPC = -1;
		public override void OnSpawn(IEntitySource source)
		{
			targetNPC = (int)Projectile.ai[0];
			base.OnSpawn(source);
		}
		public void UpdateTarget()
		{
			if(targetNPC >= 0 && targetNPC < 200)
			{
				if (Main.npc[targetNPC].active && !Main.npc[targetNPC].dontTakeDamage)
				{
					return;
				}
			}
			float minDis = 2000;
			int whoAmI = 0;
			foreach(NPC npc in Main.npc)
			{
				if (Main.npc[targetNPC].active && !Main.npc[targetNPC].dontTakeDamage)
				{
					if((npc.Center - Projectile.Center).Length() < minDis)
					{
						minDis = npc.Center.Length();
						whoAmI = npc.whoAmI;
					}
				}
			}
			if(minDis != 2000)
			{
				targetNPC = whoAmI;
			}
			else
			{
				targetNPC = -1;
			}
		}
		public void AdjustRotation()
		{
			Vector2 toTarget = new Vector2(0, -10);
			if (targetNPC != -1)
			{
				NPC target = Main.npc[targetNPC];
				toTarget = target.Center - Projectile.Center;
			}
			float aimRot = MathF.Atan2(toTarget.Y, toTarget.X);
			Projectile.rotation = Projectile.rotation * 0.95f + aimRot * 0.05f;
		}
		public override void AI()
		{
			timeToKill--;
			if(timeToKill < 0)
			{
				UpdateTarget();
				if(Projectile.timeLeft > 180)
				{
					AdjustRotation();
				}
				if(Projectile.timeLeft == 180)
				{
					Vector2 toTarget = new Vector2(0, -10);
					if (targetNPC != -1)
					{
						NPC target = Main.npc[targetNPC];
						toTarget = target.Center - Projectile.Center;
					}
					while(!Collide(Projectile.Center))
					{
						Projectile.Center += Utils.SafeNormalize(toTarget, new Vector2(0, 1));
						if (Projectile.Center.X <= 320 || Projectile.Center.X >= Main.maxTilesX - 320 || Projectile.Center.Y <= 320 || Projectile.Center.Y >= Main.maxTilesY - 320)
						{
							break;
						}
					}
				}
			}
		}
		public bool Collide(Vector2 positon)
		{
			foreach(NPC npc in Main.npc)
			{
				if (Main.npc[targetNPC].active && !Main.npc[targetNPC].dontTakeDamage)
				{
					if ((new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1)).Intersects(npc.Hitbox))
					{
						stickNPC = npc.whoAmI;
						return true;
					}
				}
			}
			return Collision.SolidCollision(positon, 0, 0);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			timeToKill = 90;
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
		}
	}
}