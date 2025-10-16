namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TitaniumClub : ClubProj_Reflective
{
	public override void SetCustomDefaults()
	{
		ReflectionStrength = 5f;
		ReflectionTexture = ModAsset.TitaniumClub_light_Mod;
		Beta = 0.0062f;
		MaxOmega = 0.468f;
	}
}