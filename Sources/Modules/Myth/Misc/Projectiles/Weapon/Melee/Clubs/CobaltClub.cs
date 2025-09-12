namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CobaltClub : ClubProj_Metal
{
	public override void SetCustomDefaults()
	{
		Beta = 0.005f;
		MaxOmega = 0.4f;
		ReflectTexturePath = ModAsset.CobaltClub_light_Mod;
		ReflectStrength = 2f;
	}
}