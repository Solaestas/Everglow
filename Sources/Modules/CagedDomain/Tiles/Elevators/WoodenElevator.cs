using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.CagedDomain.Tiles.Elevators;

public class WoodenElevator : CustomElevator
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		LightSourceColor = new Vector3(1f, 0.8f, 0.7f);
	}
}