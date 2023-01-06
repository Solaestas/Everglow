using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class ComingGhost : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Warped Blade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "诡弑");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Деформированный клинок");
            Tooltip.SetDefault("Randomly \"surprises\" a \"lucky\" enemy");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "随机抽取幸运怪物受到袭击");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Атакует случайного врага поблизости");*/
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        private int o = 0;

        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 57;//伤害 原75→现37
            Item.DamageType = DamageClass.Melee; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 54;
            Item.height = 58;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.noUseGraphic = true;
            Item.knockBack = 6;
            Item.value = 20000;
            Item.rare = 5;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.BloodGoldBlade>(); // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.shootSpeed = 8; // How fast the item shoots the projectile.
            Item.crit = 8; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
        }
        private int l = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (l % 4 == 0)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.ComingGhost>();
            }
            else if (l % 4 == 1)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.ComingGhost2>();
            }
            else if (l % 4 == 2)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.ComingGhost>();
            }
            else
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.ComingGhost2>();
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            l++;
            return false;
        }
    }
}
