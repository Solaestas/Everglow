using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class TalkNPCObjective : MissionObjectiveBase
{
	public TalkNPCObjective()
	{
	}

	public TalkNPCObjective(int type, string text)
	{
		NPCType = type > NPCID.None
			? type
			: throw new InvalidDataException($"NPC type should more than 1.");

		NPCText = !string.IsNullOrEmpty(text)
			? text
			: throw new ArgumentNullException("Argument 'text' should not be empty!");
	}

	public int NPCType { get; set; }

	public string NPCText { get; set; }

	public override void OnInitialize()
	{
		base.OnInitialize();
		AssetUtils.LoadVanillaNPCTextures([NPCType]);
	}

	public override float Progress => Main.LocalPlayer.talkNPC >= 0 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType ? 1f : 0f;

	public override bool CheckCompletion()
	{
		if (Main.LocalPlayer.talkNPC >= 0 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType)
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