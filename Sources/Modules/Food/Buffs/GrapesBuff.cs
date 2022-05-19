using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class GrapesBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MilkCarton Buff");
			Description.SetDefault("多子多福 \n 加1召唤栏，减5防御");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxMinions += 1;// 加1召唤栏
			player.statDefense -= 8; // 减8防御
		}
	}
}

	