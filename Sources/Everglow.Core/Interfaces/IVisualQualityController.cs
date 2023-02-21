using Everglow.Common.Enums;

namespace Everglow.Common.Interfaces;

public interface IVisualQualityController
{
	public VisualQuality Quality { get; }
	public bool High => Quality == VisualQuality.High;
	public bool Low => Quality == VisualQuality.Low;
}
