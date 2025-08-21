using Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Tests;

public class OverrideRegistryTestNPC : ModNPC
{
	public override string Texture => ModAsset.Point_Mod;

	public override void SetStaticDefaults()
	{
		ElementalDebuffOverrideRegistry.Register(NPCID.Zombie, BurnDebuff.ID, new ElementalDebuffOverride(ElementalResistance: 0.5f));
	}
}