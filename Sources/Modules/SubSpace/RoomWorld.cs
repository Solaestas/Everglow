using SubworldLibrary;
using Terraria.WorldBuilding;

namespace Everglow.SubSpace;

public class RoomWorld : Subworld
{
	public Subworld OriginalWorld;
	public Point AnchorWorldCoordinate;
	public override int Width => 200;
	public override int Height => 200;
	public override void OnLoad()
	{
		Main.worldSurface = 190;
		Main.rockLayer = 192;
		GenVars.waterLine = 194;
	}
	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new WoodenBoxRoomGenPass()
	};
	public override bool ShouldSave => true;
	public override void OnEnter()
	{
		base.OnEnter();
	}
	public override void OnExit()
	{
		base.OnExit();
	}
}

