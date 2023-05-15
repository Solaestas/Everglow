namespace Everglow.Myth.TheFirefly.Items;

public class LiveFluorescentLeafWall : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiveFluorescentLeafWall>());
		Item.width = 20;
		Item.height = 20;
	}
}