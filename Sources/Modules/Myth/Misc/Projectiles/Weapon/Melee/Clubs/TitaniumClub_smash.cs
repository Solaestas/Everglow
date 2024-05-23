namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TitaniumClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_TitaniumClub_Path;
	public override string TrailColorTex() => "Everglow/" + ModAsset.TitaniumClub_light_Path;
	public override void SetDef()
	{
		ReflectStrength = 5f;
		base.SetDef();
	}
}
