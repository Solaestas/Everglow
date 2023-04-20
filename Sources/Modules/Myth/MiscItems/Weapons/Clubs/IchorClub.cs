using Everglow.Myth;

namespace Everglow.Myth.MiscItems.Weapons.Clubs;

public class IchorClub : ClubItem
{
	public override void SetStaticDefaults()
	{
		
	}

	public override void SetDef()
	{
		
		Item.damage = 47;
		Item.value = 5000;
		ProjType = ModContent.ProjectileType<Projectiles.IchorClub>();
	}
}