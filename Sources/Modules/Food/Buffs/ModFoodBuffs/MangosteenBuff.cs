namespace Everglow.Food.Buffs.ModFoodBuffs;

public class MangosteenBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		//DisplayName.SetDefault("AleBuff");
		//Description.SetDefault("提升回旋镖，悠悠球、链枷类武器距离\n“补养之效”");
		Main.buffNoTimeDisplay[Type] = false;
		Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
	}

	public override void Update(Player player, ref int buffIndex)
	{
		if (Boomerangs.vanillaBoomerangs.Contains(player.HeldItem.type)
			|| Flails.vanillaFlails.Contains(player.HeldItem.type)
			|| Yoyos.vanillaYoyos.Contains(player.HeldItem.type))
		{
			player.GetAttackSpeed(DamageClass.Ranged) *= 1.5f;

		}
	}
}

