using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.CagedDomain.Tiles.Elevators;

public class EbonwoodElevator : ElevatorBase
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		LightSourceColor = new Vector3(0.7f, 0.6f, 1f);
	}
}