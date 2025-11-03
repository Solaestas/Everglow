using Everglow.Commons.MEAC;
using Everglow.Commons.Templates.Weapons;
using Everglow.Commons.Utilities;
using Terraria.DataStructures;

namespace Everglow.Example.Projectiles;

public class ExampleMeleeProjectile : MeleeProj_3D
{
	public override void OnSpawn(IEntitySource source)
	{
		EnableSphereCoordDraw = true;
	}

	public override void SetDefaults()
	{
		Projectile.width = 82;
		Projectile.height = 82;
		Projectile.tileCollide = false;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 5;
	}

	public override void AI()
	{
		base.AI();
	}
}