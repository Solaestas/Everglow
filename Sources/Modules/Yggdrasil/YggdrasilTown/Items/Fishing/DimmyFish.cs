using Everglow.Commons.Templates;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Fishing;

public class DimmyFish : FishBase
{
	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.width = 34;
		Item.height = 30;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(gold: 1);
	}
}