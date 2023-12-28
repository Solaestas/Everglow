using Everglow.Commons.Enums;

namespace Everglow.Commons.VFX.Scene;

public abstract class SceneTile : ModTile
{
	public virtual void AddScene(int i, int j)
	{
		foreach (var layer in Everglow.Commons.VFX.VFXManager.drawLayers)
		{
		}
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		AddScene(i, j);
		base.PlaceInWorld(i, j, item);
	}
	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		//AddScene(i, j);
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}
}