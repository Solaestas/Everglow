using SubworldLibrary;
namespace Everglow.Yggdrasil;

public class YggdrasilImpression : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 50;
		Item.height = 50;
		Item.useTurn = true;
		Item.useAnimation = 15;
		Item.useTime = 7;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = false;
	}
	public override bool? UseItem(Player player)
	{
		// TODO world
		if (player.itemAnimation == player.itemAnimationMax)
		{
			if (SubworldSystem.IsActive<YggdrasilWorld>())
				SubworldSystem.Exit();
			else
			{
				if (!SubworldSystem.Enter<YggdrasilWorld>())
					Main.NewText("Fail!");
			}
		}
		return base.UseItem(player);
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.Register();
	}
}
