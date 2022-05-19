using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class LemonBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("LemonBuff");
			Description.SetDefault("消炎美容 \n 加5%远程暴击,仇恨值减300 ");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCritChance(DamageClass.Ranged) += 5; // 加5%暴击
			player.aggro -= 300;//仇恨值减300
		}
	}
}

	