namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class IstafelsSunfireGrasp : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.SetNameOverride("Istafel's Sunfire Grasp");
	}

	public override void SetDefaults()
	{
		Item.width = 34;
		Item.height = 38;

		Item.accessory = true;

		Item.rare = ItemRarityID.Expert;
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		tooltips.Add(new TooltipLine(Mod, "Expert", $"Legendary"));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
	}
}