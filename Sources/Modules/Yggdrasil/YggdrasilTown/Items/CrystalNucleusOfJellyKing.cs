namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class CrystalNucleusOfJellyKing : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 46;
		Item.height = 52;
		Item.rare = ItemRarityID.White;
		Item.value = 5000;
		Item.maxStack = Item.CommonMaxStack;
	}
}