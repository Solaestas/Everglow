namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class PalmWoodSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<Projectiles.PalmWoodSlingshot>();
            Item.damage = 7;
            Item.useTime = 24;
            Item.useAnimation = 24;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cobweb, 14)
                .AddIngredient(ItemID.PalmWood, 7)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
