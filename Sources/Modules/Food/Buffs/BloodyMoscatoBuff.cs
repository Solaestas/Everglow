using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class BloodyMoscatoBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BloodyMoscatoBuff");
			Description.SetDefault("血色 \n 短时间内迅速回血");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 75; // 加75生命恢复
		}
	}
}

	