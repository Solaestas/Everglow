using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class RambutanBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RambutanBuff");
			Description.SetDefault("提高免疫 \n 免疫中毒和毒液");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffImmune[20] = true;
			player.buffImmune[70] = true;
		}
	}
}

	