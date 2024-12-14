using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class YggdrasilTownCentralSystem : ModSystem
{
	public static Rectangle TownArea;

	public override void OnWorldLoad()
	{
		if (SubworldSystem.Current is YggdrasilWorld)
		{
			TownArea = new Rectangle(430, Main.maxTilesY - 400, 501, 91);
		}
		base.OnWorldLoad();
	}

	public static bool InYggdrasilTown(Vector2 worldCoordiante)
	{
		return InYggdrasilTown(worldCoordiante.ToTileCoordinates());
	}

	public static bool InYggdrasilTown(Point tileCoordiante)
	{
		if (SubworldSystem.Current is YggdrasilWorld)
		{
			return tileCoordiante.X >= TownArea.X && tileCoordiante.X <= TownArea.X + TownArea.Width && tileCoordiante.Y >= TownArea.Y && tileCoordiante.Y <= TownArea.Y + TownArea.Height;
		}
		return false;
	}

	public static Vector2 GetTownCoord(Vector2 worldCoordiante)
	{
		return worldCoordiante - new Point(430, Main.maxTilesY - 400).ToWorldCoordinates();
	}
}