using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class BloodGoldBlade : ModItem
    {
        public override void SetStaticDefaults()
        {// TODO: Localization needed
            /*DisplayName.SetDefault("Dull Gold Massacre");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "暗金屠杀");
            Tooltip.SetDefault("Mythical Weapon\nEmits ichor blades to surroundings\nA god-slaying blade, contaminated by blood of gods");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "神话武器\n向四周释放灵液刃\n弑神之刃,沾染了众神之血");*/
        }

        public override void SetDefaults()
        {
            Item.damage = 38;//伤害 原75→现42
            Item.DamageType = DamageClass.Melee; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 54;
            Item.height = 58;
            Item.useTime = 28;
            Item.useAnimation = 28;
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
            Item.crit = 4; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
        }
        private int l = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (l % 4 == 0)
            {
                type = ModContent.ProjectileType< MiscProjectiles.Weapon.Legendary.BloodGoldBlade>();
            }
            else if (l % 4 == 1)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.BloodGoldBlade2>();
            }
            else if (l % 4 == 2)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.BloodGoldBlade3>();
            }
            else
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.BloodGoldBlade1>();
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            l++;
            return false;
        }
    }
}
