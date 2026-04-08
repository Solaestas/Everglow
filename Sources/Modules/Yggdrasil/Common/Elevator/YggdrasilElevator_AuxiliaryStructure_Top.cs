using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.Yggdrasil.Common.Elevator;

public class YggdrasilElevator_AuxiliaryStructure_Top : Elevator_AuxiliaryStructure_Top
{
	public override bool PreDraw()
	{
		return false;
	}

	public override void PostDraw() => base.PostDraw();
}