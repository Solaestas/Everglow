namespace Everglow.Commons.CustomTiles.Tiles;

public interface IGrabbable
{
	public bool CanGrab(Player player);

	public void EndGrab(Player player);

	public void OnGrab(Player player);
}
