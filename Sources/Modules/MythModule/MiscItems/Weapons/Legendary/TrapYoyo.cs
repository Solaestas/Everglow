namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class TrapYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("TrapYoyo");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "机关球");
            Tooltip.SetDefault("Legendary Weapon\nSummon a light yoyo every 1.5s, until it has 5, try to rightclick.\nA crystallize masterpiece of ancient lizards");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n每隔一定时间产生一个虚幻的光球,凑够5个光球按下右键可以释放太阳粒子流\n结构精巧,是太阳崇拜者万年来的智慧结晶");*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.useStyle = 5;
            Item.width = 30;
            Item.height = 26;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.TrapYoyo>();
            Item.useAnimation = 5;
            Item.useTime = 14;
            Item.shootSpeed = 0f;
            Item.knockBack = 0.2f;
            Item.damage = 86;//140→114
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.crit = 5;
            Item.rare = 8;
        }
    }
}
