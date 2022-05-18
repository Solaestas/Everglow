using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class BowlofSoupBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BowlofSoupBuff");
			Description.SetDefault("补脑 \n 加20魔力上限,5%魔法伤害");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Magic).Base += 0.05f;//加5%魔法伤害
			player.statManaMax2 += 20;//加20魔力上限
		}
	}
}

	