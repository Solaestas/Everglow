namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class DukeTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tusks of the Duke");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "公爵之牙");
            //Tooltip.SetDefault("Increases armor penetration by 12\n'Fortunately, Duke Fishron is not an endangered species...as long as there're truffle worms'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "增加12盔甲穿透力\n'还好猪鲨不是濒危物种...只要有松露虫'");
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 48;
            Item.value = 26090;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 12;
        }
    }
}
