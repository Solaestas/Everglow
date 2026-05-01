using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class WorldExploreObjective : WorldObjectiveBase
{
	public WorldExploreObjective()
	{
	}

	public WorldExploreObjective(int distance, Func<Player, bool> condition)
	{
		Distance = distance;
		Condition = condition;
	}

	private float _localDistance;

	public int Distance { get; private set; }

	public Func<Player, bool> Condition { get; private set; }

	public float CurrentDistance { get; private set; }

	public override float Progress => Math.Clamp(CurrentDistance / Distance, 0, 1);

	public override bool NeedDeltaSync => _localDistance > 0;

	public override bool CheckCompletion() => CurrentDistance >= Distance;

	public override void Update()
	{
		var player = Main.LocalPlayer;
		if (Condition(player))
		{
			if (NetUtils.IsSingle)
			{
				CurrentDistance += player.velocity.Length();
			}
			else if (NetUtils.IsClient)
			{
				_localDistance += player.velocity.Length();
			}
		}
	}

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();

	public override void SendDelta(BinaryWriter bw)
	{
		bw.Write(_localDistance);
		_localDistance = 0;
	}

	public override void ReceiveDelta(BinaryReader br)
	{
		var distance = br.ReadSingle();
		CurrentDistance += distance;
	}

	public override void SendMain(BinaryWriter bw)
	{
		bw.Write(CurrentDistance);
	}

	public override void ReceiveMain(BinaryReader br)
	{
		CurrentDistance = br.ReadSingle();
	}
}