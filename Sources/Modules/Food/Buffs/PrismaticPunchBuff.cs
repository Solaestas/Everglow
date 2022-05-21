using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class PrismaticPunchBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PrismaticPunchBuff");
			Description.SetDefault("高雅兴致 \n 加400仇恨值,1召唤栏,减8防御 ");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxMinions += 1; //加1召唤栏
			player.aggro += 400; //加400仇恨值
			player.statDefense -= 8 ; // 减8防御
		}
	}
}

	