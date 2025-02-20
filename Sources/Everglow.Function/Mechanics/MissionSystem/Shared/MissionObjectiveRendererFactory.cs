using Everglow.Commons.Mechanics.MissionSystem.Abstracts;
using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Templates.Abstracts;
using Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.UIMissionObjectives;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared;

public static class MissionObjectiveRendererFactory
{
	private static readonly Dictionary<Type, IMissionObjectiveRenderer> renderers = [];

	static MissionObjectiveRendererFactory()
	{
		RegisterRenderer<ICollectItemMission>(new UICollectItemMissionObjectiveRenderer());
		RegisterRenderer<IKillNPCMission>(new UIKillNPCMissionObjectiveRenderer());
	}

	private static void RegisterRenderer<T>(IMissionObjectiveRenderer renderer)
		where T : IMissionObjective
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