namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
	public class GreenGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Coagulated Green Gel");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "森绿凝晶");
			//Tooltip.SetDefault("Increases max Hp by 30 and Hp regen by 4\n'Studies have shown that green slimes generally have more vitality than blue slimes'");
			//Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "生命上限增加30,生命回复增加4\n'研究显示绿史莱姆通常比蓝史莱姆更具生命力'");
		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 26;
			Item.value = 1563;
			Item.accessory = true;
			Item.rare = 3;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statLifeMax2 += 30;
			player.lifeRegen += 4;
		}
	}
}
