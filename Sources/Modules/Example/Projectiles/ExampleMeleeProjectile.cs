using Everglow.Commons.MEAC;
using Everglow.Commons.Templates.Weapons;
using Everglow.Commons.Utilities;
using Terraria.DataStructures;

namespace Everglow.Example.Projectiles;

public class ExampleMeleeProjectile : MeleeProj_3D
{
	public override void OnSpawn(IEntitySource source)
	{
		EnableSphereCoordDraw = false;
		SlashColor = new Color(0.5f, 0.4f, 0, 0);
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		Projectile.width = 82;
		Projectile.height = 82;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 5;
	}
}