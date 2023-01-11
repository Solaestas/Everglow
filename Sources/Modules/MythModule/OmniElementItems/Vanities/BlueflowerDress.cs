using Terraria.Localization;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.OmniElementItems.Vanities
{
    [AutoloadEquip(EquipType.Legs)]
    public class BlueflowerDress : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue flower Dress");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "黑蓝色花裙");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
        }
    }
}