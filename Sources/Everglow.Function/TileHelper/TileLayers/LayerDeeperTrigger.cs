using Terraria.ObjectData;

namespace Everglow.Commons.TileHelper.TileLayers;

public abstract class LayerDeeperTriggerTile : ModTile
{
	public override void SetStaticDefaults()
	{
		SSD();
	}
	public virtual void SSD()
	{

	}
	public override bool RightClick(int i, int j)
	{
		Player player = Main.LocalPlayer;
		//if (TileLayerSystem.PlayerZoneLayer[player.whoAmI] >= 0)
		//{
		//	TileLayerSystem.LayerDeeper(Main.LocalPlayer, i, j);
		//}
		//else
		//{
		//	TileLayerSystem.LayerShallower(Main.LocalPlayer, i, j);
		//}
		TileLayerSystem.LayerDeeper(player, i, j);
		return base.RightClick(i, j);
	}
}