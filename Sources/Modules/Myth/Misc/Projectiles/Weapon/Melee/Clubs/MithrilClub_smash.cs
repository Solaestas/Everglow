namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MithrilClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_MithrilClubPath;
	public override string TrailColorTex() => "Everglow/" + ModAsset.MithrilClub_lightPath;
}
