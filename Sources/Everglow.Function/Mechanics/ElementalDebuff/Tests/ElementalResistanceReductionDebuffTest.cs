using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Tests;

public class ElementalResistanceReductionDebuffTest : ModBuff
{
	public override string Texture => ModAsset.Point_Mod;

	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		npc.GetElementalResistance(ElementalDebuffType.Burn).Base += 0.1f;
		npc.GetElementalResistance(ElementalDebuffType.Burn) += 0.05f;
	}
}