using Everglow.Commons.Mechanics.MissionSystem.Core;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class TalkNPCObjective : MissionObjectiveBase
{
	public int NPCType { get; }

	public string NPCText { get; }

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaNPCTextures([NPCType]);
	}

	public override float Progress => Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType ? 1f : 0f;

	public override bool CheckCompletion()
	{
		if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType)
		{
			UpdateNPCText(NPCText);
			return true;
		}

		return false;
	}

	private static void UpdateNPCText(string text)
	{
		Main.npcChatText = text;
	}

	public override void GetObjectivesText(List<string> lines)
	{
		var npc = new NPC();
		npc.SetDefaults(NPCType);

		lines.Add($"和{npc.TypeName}对话\n");
	}
}