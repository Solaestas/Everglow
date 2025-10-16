using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Example.Projectiles;

public class ExampleReflectiveClubSmash : ClubProjSmash_Reflective
{
	public override string Texture => ModAsset.ExampleClubProj_Mod;

	public override void SetDef()
	{
		ReflectionStrength = 5f;
	}
}