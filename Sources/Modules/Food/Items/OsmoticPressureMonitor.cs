namespace Everglow.Sources.Modules.Food.Items
{
    public class OsmoticPressureMonitor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("it can display CurrentSatiety");

        }

        public override void SetDefaults()
        {

            Item.value = Item.buyPrice(50000);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;

        }

        public override void UpdateInventory(Player player)
        {
            ThirstystateInfoDisplayplayer ThirstystateInfo = player.GetModPlayer<ThirstystateInfoDisplayplayer>();
            ThirstystateInfo.AccOsmoticPressureMonitor = true;
        }

        public override void AddRecipes()
        {

        }
    }
}
