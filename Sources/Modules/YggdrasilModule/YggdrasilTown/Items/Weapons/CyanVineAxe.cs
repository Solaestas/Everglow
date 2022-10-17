namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items.Weapons
{
    public class CyanVineAxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 46;
            Item.height = 50;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.knockBack = 2.5f;
            Item.damage = 10;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;

            Item.value = 3600;

            Item.axe = 12;
        }
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ModContent.ItemType<Items.CyanVineBar>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
