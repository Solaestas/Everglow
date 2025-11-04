using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Example.Items;

public class ExampleClub : ClubItem
{
	public override string Texture => ModAsset.ExampleClub_Mod;

	public override void SetCustomDefaults()
	{
		Item.damage = 10;
		Item.knockBack = 1f;

		ProjType = ModContent.ProjectileType<Projectiles.ExampleClubProj>();
		ProjSmashType = ModContent.ProjectileType<Projectiles.ExampleClubSmash>();
	}
}