using Everglow.Commons.Weapons;
using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee;

public class Glow_Fall : TrailingProjectile
{
	public override void OnSpawn(IEntitySource source)
	{
		Projectile.hide = true;
	}
	public override void SetDef()
	{
		TrailColor = new Color(0, 0.7f, 1f, 0);
		SelfLuminous = true;
		TrailWidth = 20f;
		Projectile.extraUpdates = 2;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 200;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		
	}
	public override void AI()
	{
		base.AI();
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver4;
		if(TimeTokill < 0)
		{
			for (int t = 0; t < 4; t++)
			{
				Dust d0 = Dust.NewDustDirect(Projectile.Center - Projectile.velocity * Main.rand.NextFloat(1f), 8, 8, DustID.MushroomSpray);
				d0.scale *= Main.rand.NextFloat(0.4f, 1f);
				d0.velocity = Projectile.velocity * Main.rand.NextFloat(0.01f, 0.04f);
			}
		}
	}
	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
		base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
	}
	public override void DrawSelf()
	{
		base.DrawSelf();
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}
	public override void DrawTrail()
	{
		base.DrawTrail();
	}
	public override void KillMainStructure()
	{
		TimeTokill = ProjectileID.Sets.TrailCacheLength[Projectile.type];
		Projectile.velocity = Projectile.oldVelocity;
	}
}
