using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class MonsterLasagnaBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MonsterLasagnaBuff");
			Description.SetDefault("弄熟的恶魔还是恶魔，在千层饼中也不会改变 \n 加25%暴击率，每秒减4生命");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
			FoodModPlayer.MonsterLasagnaBuff = true;
			player.GetCritChance(DamageClass.Generic) += 25;
		}
	}
}

	