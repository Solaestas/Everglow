namespace Everglow.Myth.TheFirefly.Items;

public class BlackStarShrub : ModItem
{
	//TODO:Translate:黑萤苣
	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 24;
		Item.maxStack = 999;
		Item.value = 100;
		Item.rare = ItemRarityID.White;
	}
}