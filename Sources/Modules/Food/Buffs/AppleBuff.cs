using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class AppleBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AppleBuff");
			Description.SetDefault("一天一苹果，医生远离我 \n 加8%减伤");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.endurance *= 0.92f;// 加8%减伤
		}
	}
}

