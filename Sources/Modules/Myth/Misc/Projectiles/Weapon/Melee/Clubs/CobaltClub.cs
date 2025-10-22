namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CobaltClub : ClubProj
{
	public override void SetCustomDefaults()
	{
		EnableReflection = true;
		Beta = 0.005f;
		MaxOmega = 0.4f;
		ReflectionTexture = ModAsset.CobaltClub_light_Mod;
		ReflectionStrength = 2f;
	}
}