namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items.Weapons
{
    public class CyanVineHammer : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 38;
            Item.height = 42;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.knockBack = 3.5f;
            Item.damage = 9;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;

            Item.value = 1100;

            Item.hammer = 35;
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
