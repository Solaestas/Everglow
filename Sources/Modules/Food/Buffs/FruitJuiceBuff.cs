using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class FruitJuiceBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MilkCarton Buff");
			Description.SetDefault("Grants +4 defense.");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 4; // 加4防御
		}
	}
}

	