namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class CobaltClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 41;
            Item.value = 2005;
            ProjType = ModContent.ProjectileType<Projectiles.CobaltClub>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
