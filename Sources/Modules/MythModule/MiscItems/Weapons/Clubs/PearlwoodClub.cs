using Terraria.DataStructures;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class PearlwoodClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 9;
            Item.value = 111;
            ProjType = ModContent.ProjectileType<Projectiles.PearlwoodClub>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Pearlwood, 18)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
