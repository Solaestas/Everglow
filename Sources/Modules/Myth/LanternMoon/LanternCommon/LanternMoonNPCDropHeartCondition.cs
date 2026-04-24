using Everglow.Myth.LanternMoon.NPCs;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonNPCDropHeartCondition : IItemDropRuleCondition
{
	private static LocalizedText description;

	public LanternMoonNPCDropHeartCondition()
	{
		description ??= Language.GetOrRegister("Mods.Everglow.DropConditions.Example");
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		bool b0 = info.npc.ModNPC is LanternMoonNPC;
		if (!b0)
		{
			return false;
		}

		LanternMoonNPC lanternNPC = info.npc.ModNPC as LanternMoonNPC;
		bool b1 = lanternNPC.LanternMoonScore >= 10f;
		return b0 && b1;
	}

	public bool CanShowItemDropInUI()
	{
		return false;
	}

	public string GetConditionDescription()
	{
		return description.Value;
	}
}