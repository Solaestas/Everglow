using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class CookedShrimpBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CookedShrimpBuff");
			Description.SetDefault("补钙 \n 加8防御");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 10 ; // 加10防御
		}
	}
}

	