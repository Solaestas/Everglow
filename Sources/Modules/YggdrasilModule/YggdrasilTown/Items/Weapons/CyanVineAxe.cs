namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items.Weapons
{
    public class CyanVineAxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 1;
            Item.height = 1;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.knockBack = 2.5f;
            Item.damage = 10;
            Item.rare = ItemRarityID.Orange;

            Item.DamageType = DamageClass.Melee;

            Item.value = 6000;

            Item.axe = 5;
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
