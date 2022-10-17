namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items.Weapons
{
    public class CyanVinePickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 48;
            Item.height = 52;
            Item.useAnimation = 14;
            Item.useTime = 14;
            Item.knockBack = 0.8f;
            Item.damage = 11;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;

            Item.value = 1100;

            Item.pick = 65;
        }
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ModContent.ItemType<Items.CyanVineBar>(), 16)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
