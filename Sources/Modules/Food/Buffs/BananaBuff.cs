using Terraria;
using Terraria.ModLoader;
using Everglow.Sources.Modules.Food;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class BananaBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BananaBuff");
			Description.SetDefault("低血压 \n 20%不消耗弹药，加5%远程伤害");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
			FoodModPlayer.BananaBuff = true;
			player.GetDamage(DamageClass.Melee).Base += 0.05f ; // 加5%伤害

		}
	}
}

	