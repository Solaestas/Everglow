using Everglow.Plant.Common;
using Terraria.Localization;

namespace Everglow.Plant.Buffs
{
	public class CactusBallBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Struck");
			DisplayName.AddTranslation(PlantUtils.LocaizationChinese, "重击");
			Description.SetDefault("Your armour was crushed\nContiniously losing life and defense reduced by 12");
			Description.AddTranslation(PlantUtils.LocaizationChinese, "你的护甲已被击毁\n持续流失生命，防御减少12");
			Main.debuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense = npc.defDefense - 12;
			if (npc.lifeRegen > 0)
				npc.lifeRegen = 0;
			npc.lifeRegen -= 5;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 12;
			if (player.lifeRegen > 0)
				player.lifeRegen = 0;
			player.lifeRegen -= 5;
		}
	}
}