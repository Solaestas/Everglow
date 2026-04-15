using Everglow.Commons.Templates.Furniture.Elevator;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles;

public class YggdrasilTownElevator_Indicator_Lamp : FloorIndicatorTileBase
{
	public override void SetCustomDefaults()
	{
		LightColor = new Vector3(1f, 0.8f, 0.3f);
	}
}