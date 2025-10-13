namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TitaniumClub : ClubProj_Reflect
{
	public override void SetCustomDefaults()
	{
		ReflectStrength = 5f;
		ReflectTexture = ModAsset.TitaniumClub_light_Mod;
		Beta = 0.0062f;
		MaxOmega = 0.468f;
	}
}