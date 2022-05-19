using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class ShrimpPoBoyBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ShrimpPoBoyBuff");
			Description.SetDefault("抗氧化 \n 加6防御,25%挖矿速度.");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 6; // 加6防御
			player.pickSpeed -= 0.2f;// 加25%挖矿速度
		}
	}
}

	