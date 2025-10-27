using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TitaniumClub : ClubProj_metal
{
	public override void SetDef()
	{
		ReflectStrength = 5f;
		ReflectTexturePath = "Everglow/Myth/Misc/Projectiles/Weapon/Melee/Clubs/TitaniumClub_light";
		Beta = 0.0062f;
		MaxOmega = 0.468f;
	}
}
