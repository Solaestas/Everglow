using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class CreamSodaBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CreamSodaBuff");
			Description.SetDefault("清凉一下 \n 加8%远程伤害");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Ranged ).Base += 0.08f; // 加8%伤害
		}
	}
}

	