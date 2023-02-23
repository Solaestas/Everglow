using Everglow.Common.Enums;
using Everglow.Common.Interfaces;
using Terraria.Graphics.Light;

namespace Everglow.Common.VFX;

public class VisualQualityController : IVisualQualityController
{
	public VisualQuality Quality => Lighting.Mode switch
	{
		LightMode.Color or LightMode.White => VisualQuality.High,
		_ => VisualQuality.Low
	};
}