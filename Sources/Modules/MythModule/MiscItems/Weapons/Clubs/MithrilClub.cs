namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class MithrilClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 56;
            Item.value = 2682;
            ProjType = ModContent.ProjectileType<Projectiles.MithrilClub>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MythrilBar, 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
