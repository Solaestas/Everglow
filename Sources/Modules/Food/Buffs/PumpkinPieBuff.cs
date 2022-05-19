using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class PumpkinPieBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PumpkinPieBuff");
			Description.SetDefault("丰收的喜悦 \n 最大生命值加50");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statLifeMax2 += 50 ;
			
		}
	}
}

	