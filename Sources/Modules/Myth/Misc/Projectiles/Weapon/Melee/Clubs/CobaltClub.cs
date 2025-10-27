using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CobaltClub : ClubProj_metal
{
	public override void SetDef()
	{
		Beta = 0.005f;
		MaxOmega = 0.4f;
		ReflectTexturePath = "Everglow/Myth/Misc/Projectiles/Weapon/Melee/Clubs/CobaltClub_light";
		ReflectStrength = 2f;
	}
}
