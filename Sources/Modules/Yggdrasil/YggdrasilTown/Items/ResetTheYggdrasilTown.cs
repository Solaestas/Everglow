using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class ResetTheYggdrasilTown : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 30;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public override bool CanUseItem(Player player)
	{
		for (int i = 50; i < Main.maxTilesX - 50; i++)
		{
			for (int j = 10000; j < Main.maxTilesY - 50; j++)
			{
				Main.tile[i, j].ClearEverything();
			}
		}
		YggdrasilTownGeneration.BuildMidnightBayou();
		return false;
	}

	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}