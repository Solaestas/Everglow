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
		npc.GetElementalDebuff(ElementalDebuffType.Burn).GetElementalResistance().Base += 0.1f;
		npc.GetElementalDebuff(ElementalDebuffType.Burn).GetElementalResistance() += 0.05f;
	}
}