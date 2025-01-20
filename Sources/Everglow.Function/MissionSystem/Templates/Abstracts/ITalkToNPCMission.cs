using Everglow.Commons.MissionSystem.Abstracts;

namespace Everglow.Commons.MissionSystem.Templates.Abstracts;

public interface ITalkToNPCMission : IMissionObjective
{
	public abstract int NPCType { get; }

	public abstract string NPCText { get; }

	public float CalculateProgress()
	{
		if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType)
		{
			UpdateNPCText(NPCText);
			return 1f;
		}

		return 0f;
	}

	public void UpdateNPCText(string text)
	{
		Main.npcChatText = text;
	}
}