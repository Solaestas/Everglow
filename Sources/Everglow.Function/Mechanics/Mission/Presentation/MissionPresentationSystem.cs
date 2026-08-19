using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.UI;
using Everglow.Commons.Mechanics.Mission.WorldSide;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public sealed class MissionPresentationSystem : ModSystem
{
	private enum MissionEventType
	{
		Added,
		Removed,
		StatusUpdated,
		ObjectiveUpdated,
	}

	private readonly List<(MissionEventType Type, MissionIdentity Identity)> _pendingEvents = [];

	public event Action<MissionIdentity> MissionAdded;
	public event Action<MissionIdentity> MissionRemoved;
	public event Action<MissionIdentity> MissionStatusUpdated;
	public event Action<MissionIdentity> MissionObjectiveUpdated;

	public MissionPresentationService Service { get; private set; }

	public override void PostSetupContent()
	{
		PlayerMissionSystem playerSystem = ModContent.GetInstance<PlayerMissionSystem>();
		WorldMissionSystem worldSystem = ModContent.GetInstance<WorldMissionSystem>();
		Service = new MissionPresentationService(
			playerSystem.Manager,
			playerSystem.Actions,
			worldSystem.Manager,
			worldSystem.Actions);

		if (!Main.dedServ)
		{
			playerSystem.Manager.MissionAdded += identity => QueueEvent(MissionEventType.Added, identity);
			playerSystem.Manager.MissionRemoved += identity => QueueEvent(MissionEventType.Removed, identity);
			playerSystem.Manager.MissionStatusUpdated += identity => QueueEvent(MissionEventType.StatusUpdated, identity);
			playerSystem.Manager.MissionObjectiveUpdated += identity => QueueEvent(MissionEventType.ObjectiveUpdated, identity);
			worldSystem.Manager.MissionStatusUpdated += identity => QueueEvent(MissionEventType.StatusUpdated, identity);
			worldSystem.Manager.MissionObjectiveUpdated += identity => QueueEvent(MissionEventType.ObjectiveUpdated, identity);
			MissionContainer.Instance.SubscribePresentationEvents(this);
		}
	}

	public override void PostUpdateEverything()
	{
		(MissionEventType Type, MissionIdentity Identity)[] pendingEvents = _pendingEvents.Distinct().ToArray();
		_pendingEvents.Clear();

		foreach ((MissionEventType type, MissionIdentity identity) in pendingEvents)
		{
			switch (type)
			{
				case MissionEventType.Added:
					MissionAdded?.Invoke(identity);
					break;
				case MissionEventType.Removed:
					MissionRemoved?.Invoke(identity);
					break;
				case MissionEventType.StatusUpdated:
					MissionStatusUpdated?.Invoke(identity);
					break;
				case MissionEventType.ObjectiveUpdated:
					MissionObjectiveUpdated?.Invoke(identity);
					break;
			}
		}
	}

	public override void Unload()
	{
		if (!Main.dedServ)
		{
			MissionContainer.Instance.Unload();
		}

		_pendingEvents.Clear();
		MissionAdded = null;
		MissionRemoved = null;
		MissionStatusUpdated = null;
		MissionObjectiveUpdated = null;
		Service = null;
	}

	private void QueueEvent(MissionEventType type, MissionIdentity identity) => _pendingEvents.Add((type, identity));
}
