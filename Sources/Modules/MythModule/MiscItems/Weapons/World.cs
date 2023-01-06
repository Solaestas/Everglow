using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class World : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("The World");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "世界");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Мир");
            Tooltip.SetDefault("Summons \"The World\" at the curser, right click when it is summoned to switch between following mode and slashing mode\n'Power of the entire land is gathered here...'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "在鼠标处召唤\"世界\"，召唤时右键以在跟随与斩击模式间切换\n'整片大地的力量,归结于此...'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Призывает \"Мир\"\nна курсоре, щелкните правой кнопкой мыши, когда он вызывается, чтобы переключиться между режимом следования и режимом разреза\n 'Здесь собрана сила всей земли...'");*/
        }
        private int o = 0;
        public override void SetDefaults()
        {
            Item.damage = 572;//伤害 原75→现37
            Item.DamageType = DamageClass.Melee; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 118;
            Item.height = 146;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Swing; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.noUseGraphic = true;
            Item.knockBack = 6;
            Item.value = 20000;
            Item.rare = 10;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.World>(); // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.shootSpeed = 0; // How fast the item shoots the projectile.
            Item.crit = 8; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
        }
        private int l = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] == 0)
            {
                Projectile.NewProjectile(source, player.Center, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            l++;
            return false;
        }
    }
}
