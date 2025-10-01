using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
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

	public override float Progress => Main.LocalPlayer.talkNPC >= NPCID.None && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType ? 1f : 0f;

	public override bool CheckCompletion() => Main.LocalPlayer.talkNPC >= NPCID.None && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType;

	public override void Complete()
	{
		base.Complete();

		Main.npcChatText = NPCText;
	}

	public override void GetObjectivesIcon(MissionIconGroup iconGroup)
	{
		var npc = new NPC();
		new NPC().SetDefaults(NPCType);
		iconGroup.Add(NPCMissionIcon.Create(NPCType, npc.TypeName));
	}

	public override void GetObjectivesText(List<string> lines)
	{
		var npc = new NPC();
		npc.SetDefaults(NPCType);

		lines.Add($"和{npc.TypeName}对话\n");
	}
}