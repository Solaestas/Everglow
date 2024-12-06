using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ActivatedJellyGlandBeam : ModProjectile
{
	public override string Texture => ModAsset.ActivatedJellyGlandExplosion_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.magic = true;
		Projectile.extraUpdates = 200;
		Projectile.timeLeft = 200;
		Projectile.penetrate = 3;
	}

	public override void OnSpawn(IEntitySource source)
	{
		var lightning = new BranchedLightning(50f, 3f, Projectile.Center, Projectile.velocity.ToRotation(), 30f, (float)(Math.PI / 50));
		Ins.VFXManager.Add(lightning);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return true;
	}
}