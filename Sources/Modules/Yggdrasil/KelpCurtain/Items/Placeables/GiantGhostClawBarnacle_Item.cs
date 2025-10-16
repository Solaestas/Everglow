using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;
using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class GiantGhostClawBarnacle_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<GiantGhostClawBarnacleCollideTile>());
		Item.width = 16;
		Item.height = 16;
	}

	public override void HoldItem(Player player)
	{
		if(Main.mouseRight && Main.mouseRightRelease)
		{
			YggdrasilWorld.CanEnterTheGiantGhoseClawBarnacle = !YggdrasilWorld.CanEnterTheGiantGhoseClawBarnacle;
		}
		Main.placementPreview = true;
	}

	public override bool CanUseItem(Player player)
	{
		var gGCBCT = TileLoader.GetTile(ModContent.TileType<GiantGhostClawBarnacleCollideTile>()) as GiantGhostClawBarnacleCollideTile;
		if (gGCBCT != null)
		{
			int x = (int)(Main.MouseWorld.X / 16 - 0);
			int y = (int)(Main.MouseWorld.Y / 16 - 12);
			gGCBCT.PlaceOriginAtTopLeft(x, y);
			Item.stack--;
			return false;
		}
		return false;
	}

	public override bool? UseItem(Player player)
	{
		return false;
	}
}