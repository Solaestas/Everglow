using System.Security.Cryptography.X509Certificates;
using Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

namespace Everglow.Myth.Misc.Buffs;

public class SilveralSpeed : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.maxRunSpeed += 0.05f;
		player.maxFallSpeed += 1.5f;
		player.wingAccRunSpeed += 0.05f;
		player.GetDamage(DamageClass.Generic) *= 1.05f;
	}
}
public class SilveralGlobalNPC : GlobalNPC {
	Player player = Main.LocalPlayer;
	public override bool InstancePerEntity => true;
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
	{
		if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.SilveralGun>() || player.HeldItem.type == ModContent.ItemType<Items.Weapons.SilveralRifle>())
			player.AddBuff(ModContent.BuffType<SilveralSpeed>(), 300);
		base.OnHitByProjectile(npc, projectile, hit, damageDone);
	}
}