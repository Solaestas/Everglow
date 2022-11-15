namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class WoodSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<Projectiles.WoodSlingshot>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cobweb, 14)
                .AddIngredient(ItemID.Wood, 7)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
