using Terraria.DataStructures;

namespace MythMod.Items.Weapons.SlingShots
{
    public class PurpleGemSlingshot : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Purple Gem Slingshot");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "紫晶弹弓");
            //Tooltip.SetDefault("Shoot little stones\nDon't need ammos");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "射出富含能量的宝石\n免弹药远程武器");
        }
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.crit = 4;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 36;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.autoReuse = false;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = 3;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.PurpleGemBead>();
            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Ranged.Slingshots.PurpleGemSlingshot>()] < 1)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Ranged.Slingshots.PurpleGemSlingshot>(), damage, knockback, player.whoAmI, Item.shootSpeed, Item.useAnimation);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(181, 8)
                .AddIngredient(20, 6)
                .AddTile(16)
                .Register();
        }
    }
}
