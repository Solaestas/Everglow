using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class SeafoodDinnerBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SeafoodDinnerBuff");
			Description.SetDefault("够生猛！\n 增加12%暴击，伤害，攻速");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCritChance(DamageClass.Generic) += 12; // 加12%暴击
			player.GetDamage(DamageClass.Generic).Base += 0.12f; // 加12%伤害
			player.GetAttackSpeed(DamageClass.Generic) += 0.12f; // 加12%攻速
		}
	}
}

	