using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Pets;

namespace Everglow.Yggdrasil.YggdrasilTown.Buffs.Pets;

public class LampWoodSeedBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
		Main.lightPet[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.buffTime[buffIndex] = 2;

		bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<LampWoodSeedProj>()] <= 0;
		if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
		{
			Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, -Vector2.UnitY * 10f, ModContent.ProjectileType<LampWoodSeedProj>(), 0, 0f, player.whoAmI, 0f, 0f);
		}
	}
}