namespace Everglow.Yggdrasil.YggdrasilTown.Items.Miscs;

public class JellyBallFat : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 22;

		Item.stack = Item.CommonMaxStack;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(silver: 1);
	}
}