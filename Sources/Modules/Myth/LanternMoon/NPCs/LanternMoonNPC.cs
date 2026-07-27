using Everglow.Myth.LanternMoon.LanternCommon;
using Terraria.GameContent.ItemDropRules;

namespace Everglow.Myth.LanternMoon.NPCs;

public abstract class LanternMoonNPC : ModNPC
{
	public LanternMoonInvasionEvent LanternMoon = ModContent.GetInstance<LanternMoonInvasionEvent>();

	public float LanternMoonScore = 1f;

	public float SpawnFrequencyInLanternMoon = 1f;

	public override bool SpecialOnKill()
	{
		LanternMoon.AddPoint(LanternMoonScore);
		return base.SpecialOnKill();
	}

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		LeadingConditionRule LanternMoonHeartRule = new LeadingConditionRule(new LanternMoonNPCDropHeartCondition());
		int itemType = ItemID.Heart;
		var parameters = new DropOneByOne.Parameters()
		{
			ChanceNumerator = 1,
			ChanceDenominator = 1,
			MinimumStackPerChunkBase = 1,
			MaximumStackPerChunkBase = 1,
			MinimumItemDropsCount = 12,
			MaximumItemDropsCount = 15,
		};

		LanternMoonHeartRule.OnSuccess(new DropOneByOne(itemType, parameters));
		npcLoot.Add(LanternMoonHeartRule);
	}
}