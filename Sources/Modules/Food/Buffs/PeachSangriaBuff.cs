using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class PeachSangriaBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PeachSangriaBuff");
			Description.SetDefault("我也是桃饱用户 \n 增加心的拾取范围，1生命回复，减4防御");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 4; // 减4防御
			player.lifeRegen += 1;//加1生命回复
			player.lifeMagnet = true;//增加心的拾取范围
		}
	}
}

	