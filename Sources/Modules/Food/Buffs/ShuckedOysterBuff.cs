using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class ShuckedOysterBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ShuckedOysterBuff");
			Description.SetDefault("寄生虫！ \n");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 4; // 加4防御
		}
	}
}

	