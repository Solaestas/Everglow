using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.Abstracts;

public interface IMissionObjectiveRenderer
{
	BaseElement Parse(MissionBase mission);
}