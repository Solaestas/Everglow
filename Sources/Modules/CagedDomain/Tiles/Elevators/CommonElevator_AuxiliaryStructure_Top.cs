using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.CagedDomain.Tiles.Elevators;

public class CommonElevator_AuxiliaryStructure_Top : Elevator_AuxiliaryStructure_Top
{
	public override void SetDefaults()
	{
		Size = new Vector2(96, 16);
		RelativePosition = new Vector2(0, -86);
	}

	public override bool PreDraw()
	{
		return false;
	}
}