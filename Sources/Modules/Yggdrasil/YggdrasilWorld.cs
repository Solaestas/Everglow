using Terraria.WorldBuilding;
using SubworldLibrary;

namespace Everglow.Yggdrasil;

internal class YggdrasilWorld : Subworld
{
	public override bool ShouldSave => true;
	public override int Width => 1200;
	public override int Height => 12000;
	public override List<GenPass> Tasks => new List<GenPass>()
	{
        new WorldGeneration.YggdrasilWorldGeneration.YggdrasilWorldGenPass()
	};
	public override void OnEnter()
	{
		SubworldSystem.hideUnderworld = true;
	}
	public override void OnLoad()
	{
		Main.worldSurface = 20;
		Main.rockLayer = 150;
		GenVars.waterLine = Main.maxTilesY;
	}
}

