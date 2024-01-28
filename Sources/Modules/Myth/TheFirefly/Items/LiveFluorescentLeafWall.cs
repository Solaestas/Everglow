namespace Everglow.Myth.TheFirefly.Items;

public class LiveFluorescentLeafWall : ModItem
{
	//TODO:Translate:流萤荧光素树叶墙
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiveFluorescentLeafWall>());
		Item.width = 24;
		Item.height = 24;
	}
}