using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items
{
    public class StoneTusk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossilized Tusk");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "石化牙");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
        }
    }
}
