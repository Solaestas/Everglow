using Everglow.Commons.Mechanics.Quest.WorldSide.Abstractions;
using Everglow.Commons.Netcode;
using Everglow.Commons.Netcode.Abstracts;

namespace Everglow.Commons.Mechanics.Quest.WorldSide.Packets;

public class ObjectiveDeltaSyncPacket_SubProgress : IPacket
{
	private string _questName;

	private IDeltaSyncObjective syncObjective;

	public ObjectiveDeltaSyncPacket_SubProgress()
	{
	}

	public ObjectiveDeltaSyncPacket_SubProgress(string questName, IDeltaSyncObjective objective)
	{
		_questName = questName;
		syncObjective = objective;
	}

	public void Receive(BinaryReader reader, int whoAmI)
	{
		var questName = reader.ReadString();
		var objectiveId = reader.ReadInt32();
		var quest = WorldQuestManager.Instance.GetQuest(questName);
		var objective = quest.Objectives.AllObjectives[objectiveId];
		if (objective is IDeltaSyncObjective deltaSyncObjective)
		{
			deltaSyncObjective.ReceiveDelta(reader);
			ModIns.PacketResolver.Route(new ObjectiveDeltaSyncPacket_MainProgress(questName, deltaSyncObjective), RouteDestination.AllDownstream);
		}
		else
		{
			Ins.Logger.Error($"{questName} {objectiveId} {objective.GetType().Name} is not {nameof(IDeltaSyncObjective)}.");
		}
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(_questName); // Quest id
		writer.Write((syncObjective as WorldObjectiveBase).ObjectiveID); // Objective id
		syncObjective.SendDelta(writer);
	}

	[HandlePacket(typeof(ObjectiveDeltaSyncPacket_SubProgress))]
	public class QuestDeltaSyncPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
		}
	}
}
