namespace Everglow.Minortopography.GiantPinetree.Items;

public class CreatPineTree : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 10;
		Item.height = 10;
		Item.useTime = 7;
		Item.useAnimation = 7;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.noUseGraphic = true;
	}
	public override bool? UseItem(Player player)
	{
		GiantPinetree.BuildGiantPinetree();
		Main.NewText(1);
		return true;
	}
}
