using Everglow.Yggdrasil.YggdrasilTown.Walls;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class IronTrackScaffolding : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.IronTrackScaffolding>());
		Item.width = 24;
		Item.height = 24;
	}
}
