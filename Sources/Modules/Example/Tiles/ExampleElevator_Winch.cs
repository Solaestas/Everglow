using Everglow.Commons.CustomTiles;
using Everglow.Commons.Templates.Furniture.Elevator;
using Everglow.Example.Elevator;

namespace Everglow.Example.Tiles;

public class ExampleElevator_Winch : WinchTileBase<ExampleElevator>
{
	public override void EmitAuxiliaryStructure(ElevatorBase parentElevator)
	{
		var auxStructure = ColliderManager.Instance.Add<ExampleElevator_AuxiliaryStructure_Top>(parentElevator.Position);
		auxStructure.ParentElavator = parentElevator;
		auxStructure.RelativePosition = new Vector2(0, -86);
	}
}