using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Example.Projectiles;

public class ExampleReflectiveClubProj : ClubProj_Reflective
{
	public override string Texture => ModAsset.ExampleClubProj_Mod;

	public override void SetCustomDefaults()
	{
		ReflectionStrength = 5f;
	}
}