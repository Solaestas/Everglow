using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.MissionSystem.MissionAbstracts;

public interface IMissionObjectiveRenderer
{
	BaseElement Parse(MissionBase mission);
}
