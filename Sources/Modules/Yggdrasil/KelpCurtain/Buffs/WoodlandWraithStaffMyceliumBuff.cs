using Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;
using Terraria;

namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class WoodlandWraithStaffMyceliumBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.lifeRegen += 10;
		if (ZoneSporeFade(player) > 0)
		{
			player.lifeRegen += 10;
		}
	}

	public float ZoneSporeFade(Player player)
	{
		float maxFade = 0;
		foreach (var proj in Main.projectile)
		{
			if (proj != null && proj.active && proj.type == ModContent.ProjectileType<WoodlandWraithStaff_SporeZone>() && proj.owner == player.whoAmI)
			{
				WoodlandWraithStaff_SporeZone wWSSZ = proj.ModProjectile as WoodlandWraithStaff_SporeZone;
				if (Vector2.Distance(proj.Center, player.Center) < wWSSZ.Range)
				{
					if (Math.Min(proj.timeLeft / 60f, 1f) > maxFade)
					{
						maxFade = Math.Min(proj.timeLeft / 60f, 1f);
					}
				}
			}
		}
		return maxFade;
	}
}