using Everglow.Commons.CustomTiles;
using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.CagedDomain.Tiles.Elevators;

public class NebulaElevator_Winch : WinchTile<NebulaElevator>
{
	public override string Texture => Commons.ModAsset.DefaultWinchTile_Mod;

	public override void EmitAuxiliaryStructure(CustomElevator parentElevator)
	{
		var auxStructure = ColliderManager.Instance.Add<CommonElevator_AuxiliaryStructure_Top>(parentElevator.Position);
		auxStructure.ParentElavator = parentElevator;
		auxStructure.RelativePosition = new Vector2(0, -86);
	}
}