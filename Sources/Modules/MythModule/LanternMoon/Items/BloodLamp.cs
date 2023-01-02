using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Items
{
    public class BloodLamp : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Lamp");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血莲灯");
        }

        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.width = 38;//宽
            Item.height = 60;//高
            Item.rare = 2;//品质
            Item.scale = 1f;//大小
            Item.useStyle = 4;
            Item.useTurn = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 10000;
        }

        public override bool? UseItem(Player player)
        {

            if (!LanternMoon.OnLanternMoon && !Main.dayTime && !Main.snowMoon && !Main.pumpkinMoon)
            {
                LanternMoon.OnLanternMoon = true;
                LanternMoon.Point = 0;
                LanternMoon.WavePoint = 0;
                Color messageColor = new Color(175, 75, 255);
                Color messageColor1 = Color.PaleGreen;
                if (Language.ActiveCulture.Name == "zh-Hans") //TODO: Localization Needed
                {
                    Main.NewText(Language.GetTextValue("The Lantern Moon is rizing..."), messageColor1);
                    Main.NewText(Language.GetTextValue("Wave 1:"), messageColor);
                }
                else
                {
                    Main.NewText(Language.GetTextValue("灯笼月正在升起..."), messageColor1);
                    Main.NewText(Language.GetTextValue("波数: 1:"), messageColor);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void AddRecipes()
        {
            //CreateRecipe()
            //   .AddIngredient(344, 1)
            //   .AddIngredient(ItemID.Torch, 1)
            //   .AddIngredient(ModContent.ItemType<Items.Flowers.RedFlame>(), 8)
            //   .AddTile(26)
            //   .Register();
        }
    }
}
