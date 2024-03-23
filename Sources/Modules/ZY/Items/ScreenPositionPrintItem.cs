using Everglow.Commons;
using Everglow.Commons.FeatureFlags;

namespace Everglow.ZY.Items;

internal class ScreenPositionPrintItem : ModItem
{
	// public override bool CloneNewInstances => true;
	public override string Texture => "Terraria/Images/UI/Wires_0";

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.useTime = 40;
		Item.useAnimation = 40;
	}

	public override bool AltFunctionUse(Player player)
	{
		return true;
	}

	public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2)
        {
            Main.NewText("screenPosition X:" + Main.screenPosition.X);
        }
		else
		{
			Main.NewText("screenPosition Y:" + Main.screenPosition.Y);
		}
		return true;
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		tooltips.Add(new TooltipLine(ModIns.Mod, "ScreenPos", string.Empty + Main.screenPosition));
	}
}