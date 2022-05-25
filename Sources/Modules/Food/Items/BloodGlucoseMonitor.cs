namespace Everglow.Sources.Modules.Food.Items
{
    public class BloodGlucoseMonitor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("血糖检测仪");
            Tooltip.SetDefault("显示当前饱食度");
        }

        public override void SetDefaults()
        {

            Item.value = Item.buyPrice(50000);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;

        }

        public override void UpdateInventory(Player player)
        {
            FoodSatietyInfoDisplayplayer SatietyInfo = player.GetModPlayer<FoodSatietyInfoDisplayplayer>();
            SatietyInfo.AccBloodGlucoseMonitor = true;
        }

        public override void AddRecipes()
        {

        }
    }
}
