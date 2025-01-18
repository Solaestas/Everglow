using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Abstracts.Missions;
using Everglow.Commons.MissionSystem.UI.UIElements.UIMissionObjectives;

namespace Everglow.Commons.MissionSystem.Shared;

public static class MissionObjectiveRendererFactory
{
	private static readonly Dictionary<Type, IMissionObjectiveRenderer> renderers = [];

	static MissionObjectiveRendererFactory()
	{
		RegisterRenderer<IGainItemMission>(new UIGainItemMissionObjectiveRenderer());
		RegisterRenderer<IKillNPCMission>(new UIKillNPCMissionObjectiveRenderer());
	}

	private static void RegisterRenderer<T>(IMissionObjectiveRenderer renderer)
		where T : IMissionObjectiveAbstract
	{
		renderers[typeof(T)] = renderer;
	}

	public static IEnumerable<IMissionObjectiveRenderer> GetRenderer(MissionBase mission)
	{
		var interfaces = mission.GetType().GetInterfaces();
		foreach (var interfaceType in interfaces)
		{
			if (renderers.TryGetValue(interfaceType, out IMissionObjectiveRenderer value))
			{
				yield return value;
			}
		}
	}
}