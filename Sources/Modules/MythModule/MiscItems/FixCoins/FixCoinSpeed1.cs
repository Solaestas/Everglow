using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.FixCoins
{
    public class FixCoinSpeed1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Enchanted Coin Speed I");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "一阶移速附魔币");
            //Tooltip.SetDefault("Gives a random accessory in inventory a prefix which increases movement speed by 1%");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "随机地给你背包中的一个饰品施加1%移速的前缀");
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.value = 2000;
            Item.rare = ItemRarityID.White;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Typeless.FixCoins.FixCoinSpeed1>();
            Item.shootSpeed = 16;
            Item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            for (int x = 0; x < 58; x++)
            {
                if (player.inventory[x].accessory)
                {
                    return true;
                }
            }
            string tex3 = "There's no accessory in your inventory";
            if (Language.ActiveCulture.Name == "zh-Hans")
            {
                tex3 = "你的背包中没有饰品";
            }
            CombatText.NewText(new Rectangle((int)player.Center.X - 10, (int)player.Center.Y - 10, 20, 20), Color.White, tex3);
            return false;
        }
    }
}
