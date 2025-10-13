namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MythrilClub : ClubProj_Reflect
{
	public override void SetCustomDefaults()
	{
		Beta = 0.005f;
		MaxOmega = 0.43f;
		ReflectTexture = ModAsset.MythrilClub_light_Mod;
		ReflectStrength = 2f;
	}
}