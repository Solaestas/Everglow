using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.Yggdrasil.Common.Elevator;

public class YggdrasilElevator : CustomElevator
{
	public override Color MapColor => new Color(122, 91, 79);

	public override void SetDefaults()
	{
		base.SetDefaults();
		LightSourceRelativePos = new Vector2(10, -57);
	}

	public override int ElevatorCableJointOffset => 125;

	public override void DrawAuxiliaryStructure(Color lightColor)
	{
		base.DrawAuxiliaryStructure(lightColor);
	}

	public override bool PreDrawElevatorCable(Color lightColor)
	{
		return true;
	}
}