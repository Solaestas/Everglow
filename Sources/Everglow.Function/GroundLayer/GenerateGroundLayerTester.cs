using Everglow.Commons.GroundLayer.LayerSupport;
using SteelSeries.GameSense;

namespace Everglow.Commons.GroundLayer;
class GenerateGroundLayerTester : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 20;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}
	public override bool CanUseItem(Player player)
	{
		GroundLayerManager.Instance.AddLayer("Test",
				"Everglow/" + ModAsset.Noise_cellPath,
				new Vector3(player.Center, Main.rand.NextFloat(1)),
				new Point(Main.rand.Next(128, 256), Main.rand.Next(128, 256)),
				new Point(256, 256),
				true,
				1,
				int.MaxValue);
		return true;
	}
	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}
