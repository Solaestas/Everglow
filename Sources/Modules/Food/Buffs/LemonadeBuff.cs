using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class LemonadeBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("LemonadeBuff");
			Description.SetDefault("消炎美容 \n 加4%远程暴击,仇恨值减200  ");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCritChance(DamageClass.Ranged ) += 4; // 加4%暴击
			player.aggro -= 200;//仇恨值减200
		}
	}
}

	