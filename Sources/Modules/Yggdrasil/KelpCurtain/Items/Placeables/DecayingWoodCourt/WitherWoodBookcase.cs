using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;
using Everglow.Commons.ItemAbstracts.Furniture;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables.DecayingWoodCourt;

public class WitherWoodBookcase : BookcaseItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.KelpCurtain.Tiles.DecayingWoodCourt.WitherWoodBookcase>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}