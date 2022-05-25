namespace Everglow.Sources.Modules.Food.Items
{
    public class OsmoticPressureMonitor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("渗透压检测仪");
            Tooltip.SetDefault("显示口渴状态");
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
