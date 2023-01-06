namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class GoldRoundYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Golden Round");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "金边球");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Золотой тур");
            Tooltip.SetDefault("Leaves a track of homing golden rings");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "留下跟踪的金圈轨迹");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Оставляет след из самонаводящихся золотых колец");*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        private int o = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.useStyle = 5;
            Item.width = 24;
            Item.height = 24;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.GoldRoundYoyo>();
            Item.useAnimation = 5;
            Item.useTime = 14;
            Item.shootSpeed = 0f;
            Item.knockBack = 0.2f;
            Item.damage = 136;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = 8;
            ItemID.Sets.Yoyo[base.Item.type] = true;
        }
    }
}
