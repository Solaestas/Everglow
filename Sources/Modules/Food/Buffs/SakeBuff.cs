using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class SakeBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SakeBuff");
			Description.SetDefault("纯度，太高了 \n 短时间内减18防御，加80%暴击，加80%伤害， 加80%攻速");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 18; // 减18防御
			player.GetCritChance(DamageClass.Melee) += 40; // 加40%暴击
			player.GetDamage(DamageClass.Melee).Base += 0.4f; // 加40%伤害
			player.GetAttackSpeed(DamageClass.Generic) += 0.4f; // 加40%攻速
		}
	}
}

	