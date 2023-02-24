namespace Everglow.ZYModule.Items;

internal class ShieldPlayer : ModPlayer
{
	public WoodShieldProj shield;
	public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
	{
		if (shield is null)
			return;
		if (!shield.Projectile.active)
		{
			shield = null;
			return;
		}
		if (shield.IsDefending && npc.Hitbox.Intersects(shield.Projectile.Hitbox))
		{
			crit = false;
			shield.DefendDamage(npc, ref damage);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
	{
		if (shield is null)
			return;
		if (!shield.Projectile.active)
		{
			shield = null;
			return;
		}
		if (shield.IsDefending && proj.Colliding(proj.Hitbox, shield.Projectile.Hitbox))
		{
			crit = false;
			shield.DefendDamage(proj, ref damage);
		}
	}
}
