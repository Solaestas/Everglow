namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public interface IDeltaSyncObjective
{
	void SendDelta(BinaryWriter bw);

	void ReceiveDelta(BinaryReader br);

	void SendMain(BinaryWriter bw);

	void ReceiveMain(BinaryReader br);
}