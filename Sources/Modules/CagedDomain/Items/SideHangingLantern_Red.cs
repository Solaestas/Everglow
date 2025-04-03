namespace Everglow.CagedDomain.Items;

public class SideHangingLantern_Red : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 100;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SideHangingLantern_Red>());
		Item.width = 22;
		Item.height = 20;
		Item.value = 1000;
	}
}