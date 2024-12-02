namespace Everglow.Myth.TheFirefly.Items;

public class LiveFluorescentLeafWall : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiveFluorescentLeafWall>());
		Item.width = 24;
		Item.height = 24;
	}
}