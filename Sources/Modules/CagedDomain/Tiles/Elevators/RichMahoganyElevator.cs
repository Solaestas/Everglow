using Everglow.Commons.Templates.Furniture.Elevator;

namespace Everglow.CagedDomain.Tiles.Elevators;

public class RichMahoganyElevator : CustomElevator
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		LightSourceColor = new Vector3(0.6f, 0.4f, 0.3f);
	}

	public override void Draw()
	{
		base.Draw();
	}
}