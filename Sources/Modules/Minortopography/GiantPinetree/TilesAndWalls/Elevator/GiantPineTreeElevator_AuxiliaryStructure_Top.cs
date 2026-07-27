using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls.Elevator;

public class GiantPineTreeElevator_AuxiliaryStructure_Top : Elevator_AuxiliaryStructure_Top
{
	public override void SetDefaults()
	{
		Size = new Vector2(64, 16);
		RelativePosition = new Vector2(0, -86);
	}

	public override bool PreDraw()
	{
		return false;
	}

	public override void PostDraw() => base.PostDraw();
}