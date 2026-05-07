using Everglow.Commons.Mechanics.Mission.WorldMission.Base;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class WorldGiveObjective : WorldObjectiveBase, IDeltaSyncObjective
{
	public WorldGiveObjective()
	{
	}

	public WorldGiveObjective(int npcType, int itemType, int itemCount)
	{
		NpcType = npcType;
		ItemType = itemType;
		ItemCount = itemCount;
	}

	public int NpcType { get; private set; }

	public int ItemType { get; private set; }

	public int ItemCount { get; private set; }

	private bool progress;

	public override float Progress => progress ? 1f : 0f;

	public override bool NeedDeltaSync => progress;

	public override bool CheckCompletion() => progress;

	public override void Update()
	{
		foreach (var player in Main.ActivePlayers)
		{
			if (player.talkNPC == NpcType)
			{
				if (player.CountItem(ItemType, ItemCount) >= ItemCount)
				{
					// Remove item from player inventory.
					for (int i = 0; i < ItemCount; i++)
					{
						player.ConsumeItem(ItemType);
					}
					progress = true;
					return;
				}
			}
		}
	}

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();

	public override void SendDelta(BinaryWriter bw)
	{
		bw.Write(progress);
	}

	public override	void ReceiveDelta(BinaryReader br)
	{
		progress = br.ReadBoolean();
	}

	public override void SendMain(BinaryWriter bw)
	{
		bw.Write(progress);
	}

	public override void ReceiveMain(BinaryReader br)
	{
		progress = br.ReadBoolean();
	}
}