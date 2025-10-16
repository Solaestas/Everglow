using Everglow.Commons.Templates.Weapons.Clubs;
using Everglow.Example.Projectiles;

namespace Everglow.Example.Items;

public class ExampleReflectiveClub : ClubItem
{
	public override string Texture => ModAsset.ExampleClub_Mod;

	public override void SetCustomDefaults()
	{
		ProjType = ModContent.ProjectileType<ExampleReflectiveClubProj>();
		ProjSmashType = ModContent.ProjectileType<ExampleReflectiveClubSmash>();
	}
}