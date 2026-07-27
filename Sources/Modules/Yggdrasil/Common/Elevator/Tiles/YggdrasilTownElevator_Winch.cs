using Everglow.Commons.CustomTiles;
using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles;

public class YggdrasilTownElevator_Winch : WinchTileBase<YggdrasilElevator>
{
	public override void EmitAuxiliaryStructure(ElevatorBase parentElevator)
	{
		var auxStructure = ColliderManager.Instance.Add<YggdrasilElevator_AuxiliaryStructure_Top>(parentElevator.Position);
		auxStructure.ParentElavator = parentElevator;
		auxStructure.RelativePosition = new Vector2(0, -86);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		base.PostDraw(i, j, spriteBatch);
	}
}