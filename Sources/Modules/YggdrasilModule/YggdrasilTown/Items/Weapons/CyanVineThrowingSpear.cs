namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items.Weapons
{
    public class CyanVineThrowingSpear : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 52;
            Item.height = 60;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.knockBack = 1f;
            Item.damage = 15;
            Item.rare = ItemRarityID.Orange;
            Item.value = 1300;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Melee;

            Item.noMelee = true;
            Item.noUseGraphic = true;


            Item.shoot = ModContent.ProjectileType<Projectiles.CyanVineThrowingSpear>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ModContent.ItemType<Items.CyanVineBar>(), 12)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
