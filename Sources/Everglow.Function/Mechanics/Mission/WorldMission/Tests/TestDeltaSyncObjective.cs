using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Tests;

public class TestDeltaSyncObjective : WorldObjectiveBase, IDeltaSyncObjective
{
	public override bool CheckCompletion() => false;

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();

	public void ReceiveDelta(BinaryReader br)
	{
		var value = br.ReadInt32();
		//Console.WriteLine(value);
	}

	public void ReceiveMain(BinaryReader br)
	{
		var value = br.ReadInt32();
		//Console.WriteLine(value);
	}

	public void SendDelta(BinaryWriter bw)
	{
		bw.Write(123);
	}

	public void SendMain(BinaryWriter bw)
	{
		bw.Write(456);
	}
}

public class TestDeltaSyncMission : WorldMissionBase
{
	public override string Name => nameof(TestDeltaSyncMission);

	public override void Initialize()
	{
		Objectives.Add(new TestDeltaSyncObjective());
	}
}