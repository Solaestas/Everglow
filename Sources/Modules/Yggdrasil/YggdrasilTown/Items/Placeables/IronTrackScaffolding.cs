namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class IronTrackScaffolding : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<YggdrasilTown.Walls.IronTrackScaffolding>());
		Item.width = 24;
		Item.height = 24;
	}
}