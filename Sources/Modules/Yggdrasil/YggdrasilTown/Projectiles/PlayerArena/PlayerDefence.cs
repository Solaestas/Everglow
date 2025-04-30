using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.PlayerArena;

public class PlayerDefence : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 6;
		Projectile.timeLeft = 60;
	}

	public override void OnSpawn(IEntitySource source)
	{
		if (Projectile.owner < 0)
		{
			Projectile.active = false;
		}
		base.OnSpawn(source);
	}

	public override void AI()
	{
		Player owner = Main.player[Projectile.owner];
		Projectile.Center = owner.Center;
		base.AI();
	}

	public override bool PreDraw(ref Color lightColor) => base.PreDraw(ref lightColor);

	public override void OnKill(int timeLeft) => base.OnKill(timeLeft);

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);
	}
}