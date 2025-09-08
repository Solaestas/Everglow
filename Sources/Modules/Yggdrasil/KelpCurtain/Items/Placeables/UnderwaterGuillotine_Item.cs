using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.UnderwaterGuillotine;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class UnderwaterGuillotine_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<UnderwaterGuillotine>());
		Item.width = 40;
		Item.height = 48;
		Item.value = 32000;
	}

	public override void HoldItem(Player player)
	{
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			YggdrasilWorld.CanEnterTheGiantGhoseClawBarnacle = !YggdrasilWorld.CanEnterTheGiantGhoseClawBarnacle;
		}
		Main.placementPreview = true;
	}

	public override bool CanUseItem(Player player)
	{
		var uG = TileLoader.GetTile(ModContent.TileType<UnderwaterGuillotine>()) as UnderwaterGuillotine;
		if (uG != null)
		{
			int x = (int)(Main.MouseWorld.X / 16 - 4);
			int y = (int)(Main.MouseWorld.Y / 16 - 0);
			uG.PlaceOriginAtTopLeft(x, y);
			Item.stack--;
			return false;
		}
		return false;
	}

	public override bool? UseItem(Player player)
	{
		return false;
	}
}