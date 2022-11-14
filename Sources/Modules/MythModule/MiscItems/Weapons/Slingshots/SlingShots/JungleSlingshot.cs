using Terraria.DataStructures;

namespace MythMod.Items.Weapons.SlingShots
{
    public class JungleSlingshot : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
            //DisplayName.SetDefault("JungleSlingshot");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "丛林弹弓");
            //Tooltip.SetDefault("Shoot little stones\nDon't need ammos");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "射出荧光蜂毒弹,接近荧光孢子粉的怪物会被染上毒素\n免弹药远程武器");
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 22;
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
            Item.value = Item.sellPrice(0, 0, 10, 80);
            Item.rare = 2;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.BlueGemBead>();
            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Ranged.Slingshots.JungleSlingshot>()] < 1)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Ranged.Slingshots.JungleSlingshot>(), damage, knockback, player.whoAmI, Item.shootSpeed, Item.useAnimation);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(210, 8)
                .AddIngredient(209, 6)
                .AddIngredient(331, 8)
                .AddTile(16)
                .Register();
        }
    }
}
