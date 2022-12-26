using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items.Weapons
{
    public class BloodyBoneYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Bone Yoyo");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血骨球");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Кровавые кости");
            Tooltip.SetDefault("Releases tusk nails on every 5 hits");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "每击中5次敌人会释放1次獠牙钉");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Выпускает гвозди из клыков при каждых 5 попаданиях");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 24;
            Item.height = 24;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.BloodyBoneYoyo>();
            Item.useAnimation = 5;
            Item.useTime = 14;
            Item.shootSpeed = 0f;
            Item.knockBack = 0.2f;
            Item.damage = 24;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.Green;
            ItemID.Sets.Yoyo[base.Item.type] = true;
        }
    }
}
