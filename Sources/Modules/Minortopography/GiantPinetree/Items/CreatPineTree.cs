using Everglow.Minortopography.GiantPinetree.TilesAndWalls;

namespace Everglow.Minortopography.GiantPinetree.Items;

public class CreatPineTree : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 10;
		Item.height = 10;
		Item.useTime = 7;
		Item.useAnimation = 7;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.createTile = ModContent.TileType<GiantPineCone_1>();
	}
	public override bool? UseItem(Player player)
	{
		GiantPinetree.BuildGiantPinetree();
		return true;
	}
}
