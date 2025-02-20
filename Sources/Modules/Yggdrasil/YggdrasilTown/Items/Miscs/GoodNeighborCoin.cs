namespace Everglow.Yggdrasil.YggdrasilTown.Items.Miscs;

public class GoodNeighborCoin : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 28;

		Item.stack = Item.CommonMaxStack;

		Item.rare = ItemRarityID.Green;
		Item.value = 0;
	}

	public override bool CanStackInWorld(Item source) => true;
}