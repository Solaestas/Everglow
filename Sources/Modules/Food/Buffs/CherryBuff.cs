using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class CherryBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CherryBuff");
			Description.SetDefault("cr,nmsl \n 增加移速与跳跃高度");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed += 0.2f;
			player.jumpSpeedBoost += 1;
		}
	}
}

	