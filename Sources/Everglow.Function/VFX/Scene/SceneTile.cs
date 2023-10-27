namespace Everglow.Commons.VFX.Scene;

public abstract class SceneTile : ModTile
{
	public virtual void AddScene(int i, int j)
	{

	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		AddScene(i, j);
		base.PlaceInWorld(i, j, item);
	}
}