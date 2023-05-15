namespace Everglow.Myth.TheFirefly.Items;

public class DarkCocoonWall : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.DarkCocoonWall>());
		Item.width = 20;
		Item.height = 20;
	}
}