using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class ChristmasPuddingBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ChristmasPuddingBuff");
			Description.SetDefault("美容养颜  \n 仇恨值减400");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.aggro -= 20;//仇恨值减400
		}
	}
}

	