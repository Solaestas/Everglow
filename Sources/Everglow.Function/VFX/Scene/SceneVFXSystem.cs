namespace Everglow.Commons.VFX.Scene;

public class SceneVFXSystem : ModSystem
{
	public static Dictionary<(int, int), bool> TilePointHasScene = new Dictionary<(int, int), bool>();
	internal Vector2 LastCheckScreenPosition = default(Vector2);
	internal float MaxUpdateDistance = 500;

	public override void PostUpdateWorld()
	{
		Vector2 deltaScreenPos = LastCheckScreenPosition - Main.screenPosition;
		if (deltaScreenPos.Length() > MaxUpdateDistance - 150)
		{
			LastCheckScreenPosition = Main.screenPosition;
			int startX = (int)((Main.screenPosition.X - MaxUpdateDistance) / 16f);
			startX = Math.Max(startX, 20);
			int endX = (int)((Main.screenPosition.X + Main.screenWidth + MaxUpdateDistance) / 16f);
			endX = Math.Min(endX, Main.maxTilesX - 20);
			int startY = (int)((Main.screenPosition.Y - MaxUpdateDistance) / 16f);
			startY = Math.Max(startY, 20);
			int endY = (int)((Main.screenPosition.Y + Main.screenHeight + MaxUpdateDistance) / 16f);
			endY = Math.Min(endY, Main.maxTilesY - 20);
			for (int x = startX; x < endX; x++)
			{
				for (int y = startY; y < endY; y++)
				{
					Tile tile = Main.tile[x, y];
					if (tile != null && tile.HasTile)
					{
						if (TileLoader.GetTile(tile.TileType) is ISceneTile)
						{
							if (!TilePointHasScene.ContainsKey((x, y)) || !TilePointHasScene[(x, y)])
							{
								ISceneTile sceneTile = TileLoader.GetTile(tile.TileType) as ISceneTile;
								if (sceneTile != null)
								{
									sceneTile.AddScene(x, y);
									TilePointHasScene[(x, y)] = true;
								}
							}
						}
					}
				}
			}
		}
		base.PostUpdateWorld();
	}

	public override void OnWorldLoad()
	{
		TilePointHasScene = new Dictionary<(int, int), bool>();
		base.OnWorldLoad();
	}
}

public class SceneTileBehavior : GlobalTile
{
	public override void PlaceInWorld(int i, int j, int type, Item item)
	{
		Tile tile = Main.tile[i, j];
		ISceneTile sceneTile = TileLoader.GetTile(tile.TileType) as ISceneTile;
		if (sceneTile != null)
		{
			sceneTile.AddScene(i, j);
			SceneVFXSystem.TilePointHasScene[(i, j)] = true;
		}

		base.PlaceInWorld(i, j, type, item);
	}
}