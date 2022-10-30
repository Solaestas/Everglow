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
            Item.knockBack = 4f;
            Item.damage = 13;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            Item.value = 3600;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Melee;

            Item.noMelee = true;
            Item.noUseGraphic = true;


            Item.shoot = ModContent.ProjectileType<Projectiles.CyanVineThrowingSpear>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.CyanVineBar>(), 12)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
