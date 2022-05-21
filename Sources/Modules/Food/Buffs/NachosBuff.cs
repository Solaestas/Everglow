using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class NachosBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("NachosBuff");
			Description.SetDefault("Grants +4 defense.");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
			FoodModPlayer.BananaBuff = true;
		}
	}
}

	