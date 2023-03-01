namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class PearlwoodSlingshot : SlingshotItem
    {
        public override void SetDef()
        {
            ProjType = ModContent.ProjectileType<Projectiles.PearlwoodSlingshot>();
            Item.damage = 18;
            Item.useTime = 21;
            Item.useAnimation = 21;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cobweb, 14)
                .AddIngredient(ItemID.Pearlwood, 7)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}