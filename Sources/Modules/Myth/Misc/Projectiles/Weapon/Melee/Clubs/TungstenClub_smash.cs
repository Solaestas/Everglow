namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TungstenClub_smash : ClubProjSmash
{
	public override string Texture => ModAsset.TungstenClub_Mod;

	public override void SetDef()
	{
		EnableReflection = true;
	}
}