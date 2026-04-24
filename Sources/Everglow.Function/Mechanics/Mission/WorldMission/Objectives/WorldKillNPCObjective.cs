using Everglow.Commons.Mechanics.Mission.Hooks;
using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class WorldKillNPCObjective : WorldObjectiveBase
{
	public WorldKillNPCObjective()
	{
	}

	public WorldKillNPCObjective(int type, int count)
	{
		NPCType = type;
		NPCCount = count;
	}

	public int NPCType { get; private set; }

	public int NPCCount { get; private set; }

	public int KilledCount { get; private set; }

	public override float Progress => Math.Clamp(KilledCount / (float)NPCCount, 0, 1);

	public override bool CheckCompletion() => KilledCount >= NPCCount;

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();

	public override void Activate(WorldMissionBase sourceMission)
	{
		WorldMissionGlobalNPC.OnNPCKilled += WorldMissionGlobalNPC_OnNPCKilled;
	}

	public override void Deactivate()
	{
		WorldMissionGlobalNPC.OnNPCKilled -= WorldMissionGlobalNPC_OnNPCKilled;
	}

	private void WorldMissionGlobalNPC_OnNPCKilled(NPC npc)
	{
		if (npc.type == NPCType)
		{
			KilledCount++;
			if (KilledCount > NPCCount)
			{
				KilledCount = NPCCount;
			}
			Console.WriteLine($"{KilledCount}/{NPCCount}");
		}
	}

	public override void ResetProgress()
	{
		base.ResetProgress();
		KilledCount = 0;
	}

	public override void NetSend(BinaryWriter writer)
	{
		base.NetSend(writer);
		writer.Write(KilledCount);
	}

	public override void NetReceive(BinaryReader reader)
	{
		base.NetReceive(reader);
		KilledCount = reader.ReadInt32();
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		if (tag.TryGet<int>(nameof(KilledCount), out var cc))
		{
			KilledCount = cc;
		}
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		tag.Add(nameof(KilledCount), KilledCount);
	}
}