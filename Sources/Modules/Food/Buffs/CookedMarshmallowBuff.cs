using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class CookedMarshmallowBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CookedMarshmallowBuff");
			Description.SetDefault("熟的轻飘飘 \n 减50%最大掉落速度，增加额外摔伤距离");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxFallSpeed *= 0.5f;
			player.extraFall += 30;
		}
	}
}

	