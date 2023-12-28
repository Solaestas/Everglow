namespace Everglow.Myth.TheTusk.NPCs;

public class Tester : ModItem
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Tester");
	}
	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 24;
		Item.maxStack = 999;
		Item.value = 100;
		Item.rare = ItemRarityID.Blue;
		Item.useStyle = 1;
		Item.useTime = Item.useAnimation = 30;
	}
	public override bool CanUseItem(Player player)
	{
		return base.CanUseItem(player);
	}
}
