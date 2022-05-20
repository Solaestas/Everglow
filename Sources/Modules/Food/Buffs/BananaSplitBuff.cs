using Terraria;
using Terraria.ModLoader;
using Everglow.Sources.Modules.Food;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class BananaSplitBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BananaSplitBuff");
			Description.SetDefault("低血压 \n 33%不消耗弹药，加8%暴击 ");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
			FoodModPlayer.BananaSplitBuff = true;
			player.GetCritChance(DamageClass.Melee) += 8; // 加8%暴击

		}
	}
}

	