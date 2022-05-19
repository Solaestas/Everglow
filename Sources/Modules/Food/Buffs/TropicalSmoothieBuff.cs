using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class TropicalSmoothieBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TropicalSmoothieBuff");
			Description.SetDefault("热带风暴 \n 加5%暴击");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCritChance(DamageClass.Generic) += 5;//加5%暴击
		}
	}
}

	