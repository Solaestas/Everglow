namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    public class BlueGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coagulated Blue Gel");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "湛蓝凝晶");
            //Tooltip.SetDefault("Increases max mana by 30 and mana regen by 4");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "增加30魔力上限和4魔力回复\n史莱姆的尸体通常会化作一滩水,但有的凝结了起来");
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.value = 1342;
            Item.accessory = true;
            Item.rare = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 30;
            player.manaRegen += 4;
        }
    }
}
