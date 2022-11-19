namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    public class Odd8Ring : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Strange Ring of 8");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "诡8指环");
            //Tooltip.SetDefault("Increases damage dealt by flat 8\n'Mystrious relic, mechanism unknown, seems to be related to certain Mathematical cult'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "造成的伤害增加8点\n'神秘的遗物,机理不明,似乎与某种数学崇拜有关'");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = 1375;
            Item.accessory = true;
            Item.rare = 2;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
        }
    }
}
