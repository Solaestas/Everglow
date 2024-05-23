namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MythrilClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_MythrilClub_Path;
	public override string TrailColorTex() => "Everglow/" + ModAsset.MythrilClub_light_Path;
}
