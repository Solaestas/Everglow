using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class PinaColadaBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("PinaColadaBuff");
			Description.SetDefault("从不添加香精当生榨 \n 加4防御，33%反伤");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 4; // 加4防御
			player.thorns += 0.33f ;//33%反伤
		}
	}
}

	