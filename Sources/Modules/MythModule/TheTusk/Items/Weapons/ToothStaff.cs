using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items.Weapons
{
    public class ToothStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk Staff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "獠牙法杖");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Посох-клык");
            Tooltip.SetDefault("Raises tusks from the ground");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "释放拔地而起的獠牙刺");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Поднимает клыки с земли");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Summon; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 6;
            Item.value = 2000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.TuskPower>();
            Item.shootSpeed = 1;
            Item.noUseGraphic = true;
            Item.crit = 8;
            Item.mana = 13;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -20);
        }
    }
}
