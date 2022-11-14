using Terraria.DataStructures;

namespace MythMod.Items.Weapons.SlingShots
{
    public class BlackWoodSlingshot : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ebonwood Slingshot");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "乌木弹弓");
            //Tooltip.SetDefault("Shoot little stones\nDon't need ammos\nFull power:120,pre power add 2.5% damage and shoot speed");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "射出石子\n免弹药远程武器");
        }
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.crit = 4;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 30;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.autoReuse = false;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = 1;
            //Item.UseSound = SoundID.Item5;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Slingshots.SlingshotAmmo>();
            Item.shootSpeed = 8f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Ranged.Slingshots.BlackWoodSlingshot>()] < 1)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Ranged.Slingshots.BlackWoodSlingshot>(), damage, knockback, player.whoAmI, Item.shootSpeed, Item.useAnimation);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cobweb, 14)
                .AddIngredient(ItemID.Ebonwood, 7)
                .AddTile(18)
                .Register();
        }
    }
}
