using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;
using Everglow.Commons.Netcode.Packets;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class MissionBase_New : IMissionNetcode
{
	public virtual void NetSend(BinaryWriter writer)
	{
		writer.Write((int)MissionState);
		writer.Write(Time);
		writer.Write(RewardClaimed);
	}

	public virtual void NetReceive(BinaryReader reader)
	{
		MissionState = (WorldMissionState)reader.ReadInt32();
		Time = reader.ReadInt32();
		RewardClaimed = reader.ReadBoolean();
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