namespace Everglow.Myth.TheTusk.Items.Materials;

public class StoneTusk : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 24;
		Item.maxStack = Item.CommonMaxStack;
		Item.value = 100;
		Item.rare = ItemRarityID.Blue;
	}
}