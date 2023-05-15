namespace Everglow.Myth.TheFirefly.Items;

public class LiveFluorescentWoodWall : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiveFluorescentWoodWall>());
		Item.width = 20;
		Item.height = 20;
	}
}