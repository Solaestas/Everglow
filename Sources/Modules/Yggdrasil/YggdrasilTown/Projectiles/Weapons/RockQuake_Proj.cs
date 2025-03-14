
namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons;

public class RockQuake_Proj : ModProjectile, IWarpProjectile_warpStyle2
{
	public override string Texture => Commons.ModAsset.Empty_Mod;

	public override void SetDefaults()
	{
		Projectile.timeLeft = 550;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.penetrate = -1;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.width = 30;
		Projectile.height = 30;
	}

	public override void AI() => base.AI();

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => base.Colliding(projHitbox, targetHitbox);

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public void DrawWarp(VFXBatch spriteBatch) => throw new NotImplementedException();
}