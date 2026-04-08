using Everglow.Commons.CustomTiles;
using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.CagedDomain.Tiles.Elevators;

public class EbonwoodElevator_Winch : WinchTileBase<EbonwoodElevator>
{
	public override string Texture => Commons.ModAsset.DefaultWinchTile_Mod;

	public override void EmitAuxiliaryStructure(ElevatorBase parentElevator)
	{
		var auxStructure = ColliderManager.Instance.Add<CommonElevator_AuxiliaryStructure_Top>(parentElevator.Position);
		auxStructure.ParentElavator = parentElevator;
		auxStructure.RelativePosition = new Vector2(0, -86);
	}
}