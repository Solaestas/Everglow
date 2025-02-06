using Everglow.Commons.ItemAbstracts;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Fishing;

public class GrassCarp : FishBase
{
	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.width = 52;
		Item.height = 44;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(silver: 40);
	}
}