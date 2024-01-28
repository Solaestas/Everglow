namespace Everglow.Myth.TheFirefly.Items;

public class DarkCocoonWall : ModItem
{
	//TODO:Translate:暗茧墙壁
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.DarkCocoonWall>());
		Item.width = 24;
		Item.height = 24;
	}
}