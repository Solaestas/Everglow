using Everglow.Yggdrasil.KelpCurtain.Tiles;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Developer;

public class ResetKelpCurtain : ModItem
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

	public Point OldMousePos = default(Point);

	public override void HoldItem(Player player) => base.HoldItem(player);

	public override bool CanUseItem(Player player)
	{
		KelpCurtainBiome.StratumBoundCurve.Clear();
		YggdrasilWorldGeneration.ClearRectangleArea(20, (int)(Main.maxTilesY * 0.75f), Main.maxTilesX - 20, (int)(Main.maxTilesY * 0.9f));
		KelpCurtainGeneration.BuildKelpCurtain();

		// Point mouseTile = Main.MouseWorld.ToTileCoordinates();
		// Point mouseTile = Main.MouseWorld.ToTileCoordinates();
		// if (OldMousePos != default(Point))
		// {
		// KelpCurtainGeneration.ConnectMossyTunnel(OldMousePos, mouseTile, 8);
		// }
		// OldMousePos = mouseTile;
		// YggdrasilWorldGeneration.GenerateStalactite(Main.MouseWorld / 16f, 8, Main.rand.NextFloat(9, 42), ModContent.TileType<StoneScaleWood>());
		return false;
	}
	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}