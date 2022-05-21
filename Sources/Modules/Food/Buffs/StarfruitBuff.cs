using Terraria;
using Terraria.ModLoader;
using Everglow.Sources.Modules.Food;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class StarfruitBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("StarfruitBuff");
			Description.SetDefault("1437大帝的淫威 \n 使用远程武器时会生成向后的射弹");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
			FoodModPlayer.StarfruitBuff = true;
		}
	}
}

	