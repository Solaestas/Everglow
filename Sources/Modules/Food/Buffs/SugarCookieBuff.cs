using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class SugarCookieBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SugarCookieBuff");
			Description.SetDefault("养胃滋润 \n 加10%远程伤害");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Ranged).Base += 0.10f; // 加10%伤害
		}
	}
}

	