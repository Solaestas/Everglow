using MythMod.EternalResolveMod.Common;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
    public class PlatinumStabbingSword : StabbingSwordItem
	{

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 75);
            //Item.shoot = ModContent.ProjectileType<PlatinumStabbingSword_Pro>();
            base.SetDefaults();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PlatinumBar, 24).
                AddTile(TileID.Anvils).
                Register();
            base.AddRecipes();
        }
    }
}