using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Pets;

namespace Everglow.Yggdrasil.YggdrasilTown.Buffs.Pets;

public class BadrBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
		Main.lightPet[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		// + 3% pick speed
		player.pickSpeed += 0.03f;

		player.buffTime[buffIndex] = 2;

		bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<BadrProj>()] <= 0;
		if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
		{
			Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, -Vector2.UnitY * 10f, ModContent.ProjectileType<BadrProj>(), 0, 0f, player.whoAmI, 0f, 0f);
		}
	}
}