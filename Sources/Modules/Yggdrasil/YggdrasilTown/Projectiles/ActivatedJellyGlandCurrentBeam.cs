using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ActivatedJellyGlandCurrentBeam : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.magic = true;
		Projectile.extraUpdates = 100;
		Projectile.timeLeft = 200;
		Projectile.penetrate = 1;
	}

	public override void OnSpawn(IEntitySource source)
	{
		// TODO: Generate electric waves
	}

	public override void AI()
	{
		// TODO: Update dust arguments
		var smog = new JellyBallCurrentBeamDust()
		{
			Active = true,
			Visible = true,
			position = Projectile.Center,
			rotation = Projectile.velocity.ToRotation(),
		};
		Ins.VFXManager.Add(smog);
	}

	public override bool PreDraw(ref Color lightColor) => false;
}