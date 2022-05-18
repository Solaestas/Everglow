using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class GrapeJuiceBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GrapeJuiceBuff");
			Description.SetDefault("多子多福 \n ");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxMinions += 2;
			player.statDefense -= 15; // 减15防御
		}
	}
}

	