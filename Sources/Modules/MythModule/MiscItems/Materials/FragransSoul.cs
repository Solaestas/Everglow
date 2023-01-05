using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Materials
{
    public class FragransSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Soul");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "金桂之魂");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 99999;
            Item.value = 0;
            Item.rare = 0;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Wheat.ToVector3() * 0.55f * Main.essScale);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0);
        }
    }
}
