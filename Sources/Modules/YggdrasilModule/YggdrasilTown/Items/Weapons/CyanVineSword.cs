namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items.Weapons
{
    public class CyanVineSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 52;
            Item.height = 56;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.knockBack = 1f;
            Item.damage = 15;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;

            Item.value = 1100;
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
