using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;
using Everglow.Commons.Netcode.Packets;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class MissionBase_New : IMissionNetcode
{
	public virtual void NetSend(BinaryWriter writer)
	{
		writer.Write((int)State);
		writer.Write(Time);
		writer.Write(RewardClaimed);
		writer.Write(RewardClaimedPlayers.Count);
		foreach (var player in RewardClaimedPlayers)
		{
			writer.Write(player);
		}
	}

	public virtual void NetReceive(BinaryReader reader)
	{
		State = (WorldMissionState)reader.ReadInt32(); // TODO: What to do if the state is different?
		Time = reader.ReadInt32();
		RewardClaimed = reader.ReadBoolean();
		int rewardPlayerCount = reader.ReadInt32();
		for (int i = 0; i < rewardPlayerCount; i++)
		{
			RewardClaimedPlayers.Add(reader.ReadString());
		}
	}

	public void OnMPSync()
	{
		for (var task = Objectives.First; task != null; task = task.Next)
		{
			if (task is IDeltaSyncObjective deltaSync)
			{
				ModIns.PacketResolver.Send(new ObjectiveDeltaSyncPacket_SubProgress(WhoAmI, deltaSync));
			}
		}
	}
}