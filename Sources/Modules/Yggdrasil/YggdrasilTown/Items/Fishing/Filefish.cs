using Everglow.Commons.Templates;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Fishing;

public class Filefish : FishBase
{
	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.width = 40;
		Item.height = 40;

		Item.rare = ItemRarityID.White;
		Item.value = Item.buyPrice(silver: 40);
	}
}