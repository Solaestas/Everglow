namespace Everglow.Commons.VFX.Scene;
public class SceneVFXSystem : ModSystem
{
	private int timeClock = 0;
	public override void OnWorldLoad()
	{
		timeClock = 0;
		base.OnWorldLoad();
	}
	public override void PostUpdateEverything()
	{
		if(timeClock < 10)
		{
			timeClock++;
		}
		base.PostUpdateEverything();
	}
	public override void PostUpdateWorld()
	{
		if (timeClock == 1)
		{
			for (int x = 20; x < Main.maxTilesX - 20; x++)
			{
				for (int y = 20; y < Main.maxTilesY - 20; y++)
				{
					Tile tile = Main.tile[x, y];
					if (tile != null)
					{
						if (TileLoader.GetTile(tile.TileType) is SceneTile)
						{
							SceneTile sceneTile = TileLoader.GetTile(tile.TileType) as SceneTile;
							sceneTile.AddScene(x, y);
						}
					}
				}
			}
		}
		base.PostUpdateWorld();
	}
}
