using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
    public class ShadewoodClub : ClubItem
    {
        public override void SetDef()
        {
            Item.damage = 7;
            Item.value = 80;
            ProjType = ModContent.ProjectileType<Projectiles.ShadewoodClub>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Shadewood, 18)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
