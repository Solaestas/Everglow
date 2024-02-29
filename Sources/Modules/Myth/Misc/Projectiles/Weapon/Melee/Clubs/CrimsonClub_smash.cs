namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CrimsonClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_CrimsonClubPath;
	public override string TrailColorTex() => "Everglow/" + ModAsset.CrimsonClub_lightPath;
}
