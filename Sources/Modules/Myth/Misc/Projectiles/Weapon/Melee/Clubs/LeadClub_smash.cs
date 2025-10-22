namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class LeadClub_smash : ClubProjSmash
{
	public override string Texture => ModAsset.LeadClub_Mod;

	public override void SetDef()
	{
		EnableReflection = true;
	}
}