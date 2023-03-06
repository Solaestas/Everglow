namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class JungleVitamin : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jungle Vitamin");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "丛林维生素");
            //Tooltip.SetDefault("Increases max Hp by 50\n'It helps a lot in humid, dangerous jungle'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "生命上限增加50\n'在潮湿,危险的丛林中可以给予巨大的帮助'");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.value = 1528;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
        }
    }
}
