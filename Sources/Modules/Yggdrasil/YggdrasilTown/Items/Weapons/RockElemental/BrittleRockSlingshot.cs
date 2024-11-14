using Everglow.Commons.Weapons.Slingshots;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.RockElemental;

public class BrittleRockSlingshot : SlingshotItem
{
	public override void SetDef()
	{
		Item.damage = 24;
		Item.knockBack = 10;
		ProjType = ModContent.ProjectileType<Projectiles.BrittleRockSlingshotProj>();
		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(0, 0, 55, 0);
	}
}