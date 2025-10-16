namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MythrilClub : ClubProj_Reflective
{
	public override void SetCustomDefaults()
	{
		Beta = 0.005f;
		MaxOmega = 0.43f;
		ReflectionTexture = ModAsset.MythrilClub_light_Mod;
		ReflectionStrength = 2f;
	}
}