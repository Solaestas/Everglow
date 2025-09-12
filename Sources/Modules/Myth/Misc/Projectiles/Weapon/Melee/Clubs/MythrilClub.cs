namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MythrilClub : ClubProj_Metal
{
	public override void SetCustomDefaults()
	{
		Beta = 0.005f;
		MaxOmega = 0.43f;
		ReflectTexturePath = ModAsset.MythrilClub_light_Mod;
		ReflectStrength = 2f;
	}
}