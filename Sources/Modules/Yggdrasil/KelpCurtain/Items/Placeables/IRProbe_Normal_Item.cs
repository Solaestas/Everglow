using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.IRProbe;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class IRProbe_Normal_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public int PlaceType = 0;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<IRProbe_Normal>());
		Item.width = 20;
		Item.height = 24;
		Item.value = 400;
	}

	public override void HoldItem(Player player)
	{
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			PlaceType++;
			if (PlaceType >= 2)
			{
				PlaceType = 0;
			}
			switch (PlaceType)
			{
				case 0:
					Item.DefaultToPlaceableTile(ModContent.TileType<IRProbe_Normal>());
					break;
				case 1:
					Item.DefaultToPlaceableTile(ModContent.TileType<IRProbe_90_Degree_Scan>());
					break;
			}
		}
		base.HoldItem(player);
	}
}