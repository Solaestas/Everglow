namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class LichenInfected : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}
	public override void Update(NPC npc, ref int buffIndex)
	{
		int buffDamage = (int)(5 + npc.velocity.Length() * 8);

		npc.lifeRegen = -buffDamage;
		npc.lifeRegenExpectedLossPerSecond = (int)(1 + npc.velocity.Length() * 2.4f);

		base.Update(npc, ref buffIndex);
	}
}
public class LichenInfectedNPC : GlobalNPC
{
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
	{
		if(npc.HasBuff(ModContent.BuffType<LichenInfected>()))
		{

		}
		base.OnHitPlayer(npc, target, hurtInfo);
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
	{
		if (npc.HasBuff(ModContent.BuffType<LichenInfected>()))
		{
			Projectile p0 = Projectile.NewProjectileDirect(player.GetSource_FromAI(), npc.Center, Vector2.zeroVector, ModContent.ProjectileType<LichensPycnidium>(), 150, 0, player.whoAmI);
			LichensPycnidium lp = p0.ModProjectile as LichensPycnidium;
			if(lp != null)
			{
				lp.AttachTarget = npc;
			}
		}
		base.OnHitByItem(npc, player, item, hit, damageDone);
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
	{
		if (npc.HasBuff(ModContent.BuffType<LichenInfected>()))
		{
			Projectile p0 = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), npc.Center, Vector2.zeroVector, ModContent.ProjectileType<LichensPycnidium>(), 150, 0, projectile.owner);
			LichensPycnidium lp = p0.ModProjectile as LichensPycnidium;
			if (lp != null)
			{
				lp.AttachTarget = npc;
			}
		}
		base.OnHitByProjectile(npc, projectile, hit, damageDone);
	}
}
