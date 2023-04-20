namespace Everglow.Myth.TheFirefly.Items;

public class DarkCocoonWall : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;
		Item.maxStack = 999;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 7;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.createWall = ModContent.WallType<Walls.DarkCocoonWall>();
	}
}