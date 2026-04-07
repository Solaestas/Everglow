using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.CagedDomain.Tiles.Elevators;

public class PearlwoodElevator : CustomElevator
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		LightSourceColor = new Vector3(0.5f, 0.7f, 1f);
	}
}