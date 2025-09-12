using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CrimsonClub_smash : ClubProjSmash_Metal
{
	public override string Texture => "Everglow/" + ModAsset.CrimsonClub_Path;
	public override string TrailColorTex() => "Everglow/" + ModAsset.CrimsonClub_light_Path;
}