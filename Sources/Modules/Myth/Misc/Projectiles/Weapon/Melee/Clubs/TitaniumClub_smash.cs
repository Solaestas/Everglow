namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TitaniumClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_TitaniumClubPath;
	public override string TrailColorTex() => "Everglow/" + ModAsset.TitaniumClub_lightPath;
}
