namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class Elevator_AuxiliaryStructure_Top : Elevator_AuxiliaryStructure
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