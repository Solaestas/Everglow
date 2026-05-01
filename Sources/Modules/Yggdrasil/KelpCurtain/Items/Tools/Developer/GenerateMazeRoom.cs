using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Tools.Developer;

public class GenerateMazeRoom : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 30;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public Point OldMousePos = default;

	public override void HoldItem(Player player) => base.HoldItem(player);

	public override bool CanUseItem(Player player)
	{
		BuildMazeRoom(Main.MouseWorld);
		return false;
	}

	public void BuildMazeRoom(Vector2 worldPos)
	{
		Point tilePos = worldPos.ToTileCoordinates();
		List<Tile> tiles = YggdrasilWorldGeneration.BFSContinueTile(tilePos);
	}
}