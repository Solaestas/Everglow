using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class GelPowerGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("GelPowerGun");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "啫喱充能器");
            Tooltip.SetDefault("Legendary Weapon\nShoot powerful gels(Ammo: Gel)");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n射出高能凝胶流(使用凝胶作为子弹)");*/
        }

        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.DamageType = DamageClass.Ranged; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 118;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.knockBack = 0;
            Item.value = 2000;
            Item.rare = 7;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.JellyPower>();
            Item.shootSpeed = 5f; // The speed of the projectile (measured in pixels per frame.)
            Item.useAmmo = 23;
            Item.crit = 4; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.Next(100) > 50)
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.JellyPower2>();
            }
            int h = Projectile.NewProjectile(source, position + velocity * 8 + new Vector2(0, -8), velocity, type, damage, knockback, player.whoAmI, 0);
            Main.projectile[h].ai[0] = 0;
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20f, 0);
        }
    }
}
