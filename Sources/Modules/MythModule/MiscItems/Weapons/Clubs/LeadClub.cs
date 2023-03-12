namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class LeadClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 7;
            Item.value = 88;
            ProjType = ModContent.ProjectileType<Projectiles.LeadClub>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 18)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
