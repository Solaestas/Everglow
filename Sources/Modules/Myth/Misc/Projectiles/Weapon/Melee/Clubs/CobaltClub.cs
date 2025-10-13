namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CobaltClub : ClubProj_Reflect
{
	public override void SetCustomDefaults()
	{
		Beta = 0.005f;
		MaxOmega = 0.4f;
		ReflectTexture = ModAsset.CobaltClub_light_Mod;
		ReflectStrength = 2f;
	}
}