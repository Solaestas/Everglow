namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class Elevator_AuxiliaryStructure_Side : Elevator_AuxiliaryStructure
{
	public override void SetDefaults()
	{
		Size = new Vector2(16, 68);
	}

	public override void AI()
	{
		base.AI();
		if (ParentElavator.MoveState == CustomElevator.State.Stop)
		{
			Size = new Vector2(16, 16);
			RelativePosition.Y = -84;
		}
		else
		{
			Size = new Vector2(16, 68);
			RelativePosition.Y = -42;
		}
	}
}