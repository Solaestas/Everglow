using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.FevensAttack;

public class Fevens_DefenseEffect : ModProjectile
{
	public override string Texture => ModAsset.Fevens_Wing_Mod;

	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 60;
		Projectile.timeLeft = 120;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
	}

	public override void AI()
	{
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => base.Colliding(projHitbox, targetHitbox);

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		base.OnHitPlayer(target, info);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
}