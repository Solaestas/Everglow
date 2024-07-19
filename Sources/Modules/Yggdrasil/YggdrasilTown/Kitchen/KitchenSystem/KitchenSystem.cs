using Everglow.SubSpace;
using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;
using SubworldLibrary;
using Terraria.UI;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.KitchenSystem;

public class KitchenSystem : ModSystem
{
	public KitchenSystemUI KitchenSystemUI;

	public int RestaurantBoardCount;

	public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
	{
		RestaurantBoardCount = tileCounts[ModContent.TileType<RestaurantLogoBoard>()];
	}

	public bool Active => SubworldSystem.IsActive<RoomWorld>() && IsInAKitchenWorld();

	public override void Load()
	{
		KitchenSystemUI = new KitchenSystemUI("KitchenSystemInterface", InterfaceScaleType.UI);
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		if (!Active)
		{
			return;
		}
		int mouseItemIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
		if (mouseItemIndex != -1)
		{
			layers.Insert(mouseItemIndex, KitchenSystemUI);
		}
	}

	public bool IsInAKitchenWorld()
	{
		return RestaurantBoardCount > 0;
	}
}