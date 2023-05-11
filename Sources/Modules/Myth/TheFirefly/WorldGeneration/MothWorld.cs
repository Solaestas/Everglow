using Terraria.WorldBuilding;
using SubworldLibrary;
namespace Everglow.Myth.TheFirefly.WorldGeneration;

internal class MothWorld : Subworld
{
	public override bool ShouldSave => true;
	public override int Width => 800;
	public override int Height => 600;
	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new MothLand.MothLandGenPass()
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
