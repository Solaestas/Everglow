using Everglow.Commons.CustomTiles;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// Visulalize the data of mouse-covered-tile.
/// </summary>
public class NormalCableCar_item : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 34;
		Item.useTurn = true;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.value = 0;
		Item.rare = ItemRarityID.White;
	}

	public override void HoldItem(Player player)
	{
	}

	public override bool? UseItem(Player player)
	{
		Point mouseWorldTile = Main.MouseWorld.ToTileCoordinates();
		for (int i = -5; i < 6; i++)
		{
			for (int j = -5; j < 6; j++)
			{
				var checkPoint = mouseWorldTile + new Point(i, j);
				Tile tile = Main.tile[checkPoint];
				if(tile.TileType == ModContent.TileType<CableCarJoint>())
				{
					TileSystem.Instance.AddTile(new NormalCableCar() { Position = checkPoint.ToWorldCoordinates(), size = new Vector2(80, 80), Direction = -1, Active = true });
				}
			}
		}
		return true;
	}
}