using Terraria.ObjectData;

namespace Everglow.Example.TileLayers;

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
		TileLayerSystem.LayerDeeper(Main.LocalPlayer, i, j);
		return base.RightClick(i, j);
	}
}