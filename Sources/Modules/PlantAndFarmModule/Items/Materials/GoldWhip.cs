namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class GoldWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Banana of the Valley");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "金鞭兰");
        }
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 46;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
