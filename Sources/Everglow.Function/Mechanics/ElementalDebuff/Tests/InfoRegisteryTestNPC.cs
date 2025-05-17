namespace Everglow.Commons.Mechanics.ElementalDebuff.Tests;

public class InfoRegisteryTestNPC : ModNPC
{
	public override string Texture => ModAsset.Point_Mod;

	public override void SetStaticDefaults()
	{
		ElementalDebuffInfoRegistry.Register(NPCID.Zombie, ElementalDebuffType.Burn, new ElementalDebuffInfo(ElementalResistance: 0.5f));
	}
}