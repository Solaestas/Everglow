using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class GrapesBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GrapesBuff");
			Description.SetDefault("多子多福 \n 加1召唤栏，幸运值加10%，减8防御");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxMinions += 1;// 加1召唤栏
			player.luck *= 1.1f ; 
			player.statDefense -= 8; // 减8防御
		}
	}
}

	