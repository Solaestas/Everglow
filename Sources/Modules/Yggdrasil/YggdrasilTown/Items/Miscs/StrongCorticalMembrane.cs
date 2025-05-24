namespace Everglow.Yggdrasil.YggdrasilTown.Items.Miscs;

public class StrongCorticalMembrane : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 24;

		Item.stack = Item.CommonMaxStack;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(silver: 10);
	}
}