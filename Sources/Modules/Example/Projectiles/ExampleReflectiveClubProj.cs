using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Example.Projectiles;

public class ExampleReflectiveClubProj : ClubProj
{
	public override string Texture => ModAsset.ExampleClubProj_Mod;

	public override void SetCustomDefaults()
	{
		EnableReflection = true;
		ReflectionStrength = 5f;
	}
}