using Everglow.Commons.CustomTiles;
using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls.Elevator;

public class GiantPineTreeElevator_Winch : WinchTile<GiantPineTreeElevator>
{
	public override void EmitAuxiliaryStructure(CustomElevator parentElevator)
	{
		var auxStructure = ColliderManager.Instance.Add<GiantPineTreeElevator_AuxiliaryStructure_Top>(parentElevator.Position);
		auxStructure.ParentElavator = parentElevator;
		auxStructure.RelativePosition = new Vector2(0, -86);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		base.PostDraw(i, j, spriteBatch);
	}
}