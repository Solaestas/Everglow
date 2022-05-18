using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class ElderberryBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ElderberryBuff");
			Description.SetDefault("抗氧化 \n 你可以短距离冲刺");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 4; // 加4防御
		}
	}
}

	