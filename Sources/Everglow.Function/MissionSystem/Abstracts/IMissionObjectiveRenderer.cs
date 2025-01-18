using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.MissionSystem.Abstracts;

public interface IMissionObjectiveRenderer
{
	BaseElement Parse(MissionBase mission);
}
