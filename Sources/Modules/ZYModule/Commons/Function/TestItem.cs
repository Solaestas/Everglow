namespace Everglow.ZYModule.Commons.Function;

internal class TestItem : ModItem
{
	public override string Texture => "Terraria/Images/UI/Wires_0";
	public override void SetDefaults()
	{
		Item.useAnimation = 10;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.autoReuse = false;
	}
	public override bool CanUseItem(Player player)
	{
		return false;
	}
	public override void AddRecipes()
	{
		CreateRecipe().Register();
	}
}