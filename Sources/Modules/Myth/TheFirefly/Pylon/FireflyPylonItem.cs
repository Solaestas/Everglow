using Everglow.Commons.Templates.Pylon;
using Everglow.Myth.TheFirefly.Dusts;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Pylon;

public class FireflyPylonItem : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<FireflyPylon>());
	}
}