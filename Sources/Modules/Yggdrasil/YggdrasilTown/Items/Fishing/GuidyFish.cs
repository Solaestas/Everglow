using Everglow.Commons.ItemAbstracts;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Fishing;

public class GuidyFish : FishBase
{
	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.width = 29;
		Item.height = 26;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(gold: 1);
	}
}