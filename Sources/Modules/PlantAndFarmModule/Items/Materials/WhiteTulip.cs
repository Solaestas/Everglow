namespace Everglow.Sources.Modules.PlantAndFarmModule.Items.Materials
{
    public class WhiteTulip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("White Tulip");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "白酒杯花");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = 0;
            Item.rare = ItemRarityID.White;
            Item.material = true;
        }
    }
}
