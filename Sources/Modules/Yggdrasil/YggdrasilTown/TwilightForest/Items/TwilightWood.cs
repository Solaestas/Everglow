namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;

public class TwilightWood : ModItem
{
	//TODO:暮木
	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = Item.CommonMaxStack;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
	}
}