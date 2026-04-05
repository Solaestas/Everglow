using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls.Elevator;

public class GiantPineTreeElevator : CustomElevator
{
	public override Color MapColor => new Color(122, 91, 79);

	public override void SetDefaults()
	{
		base.SetDefaults();
		LightSourceRelativePos = new Vector2(1, -55);
		LightSourceColor = new Vector3(0.4f, 0.6f, 0.8f) * 0.6f;
	}

	public override int ElevatorCableJointOffset => 125;

	public override void AddLight()
	{
		if (LightSourceOn)
		{
			Lighting.AddLight(Box.Center + LightSourceRelativePos, LightSourceColor);
			Lighting.AddLight(Box.Center + new Vector2(-20, -59), LightSourceColor);
			Lighting.AddLight(Box.Center + new Vector2(-16, -57), LightSourceColor);
		}
	}
}