using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class GrapefruitBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GrapefruitBuff");
			Description.SetDefault("好肾脏 \n 加10%召唤物击退");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetKnockback(DamageClass.Summon) += 0.1f;//加10%召唤物击退
		}
	}
}

	