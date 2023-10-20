using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;
namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class StoneBridgeCreate : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 20;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}
	public override bool CanUseItem(Player player)
	{
		int x0 = (int)(Main.MouseWorld.X / 16);
		int y0 = (int)(Main.MouseWorld.Y / 16);
		PlaceFrameImportantTiles(x0, y0, 5, 3, ModContent.TileType<LampWood_Stone_5x4>(), 90 * Main.rand.Next(1), 0);
		return false;
	}
	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}
