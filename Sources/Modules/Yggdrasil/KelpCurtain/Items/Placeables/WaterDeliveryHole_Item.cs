using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.LightningMechanism;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class WaterDeliveryHole_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public int State = 0;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<WaterDeliveryHole>());
		Item.width = 38;
		Item.height = 14;
		Item.value = 5000;
	}

	public override void HoldItem(Player player)
	{
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			State++;
			if (State >= 2)
			{
				State = 0;
			}
			if (State == 1)
			{
				Item.DefaultToPlaceableTile(ModContent.TileType<WaterDeliveryHole>());
				Item.width = 18;
				Item.height = 36;
			}
			else
			{
				Item.DefaultToPlaceableTile(ModContent.TileType<WaterDeliveryHole_V>());
				Item.width = 18;
				Item.height = 36;
			}
		}
		base.HoldItem(player);
	}
}