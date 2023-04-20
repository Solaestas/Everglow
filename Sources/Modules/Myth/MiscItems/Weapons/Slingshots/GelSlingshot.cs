namespace Everglow.Myth.MiscItems.Weapons.Slingshots;

public class GelSlingshot : SlingshotItem
{
	public override void SetDef()
	{
		Item.damage = 12;
		Item.crit = 4;
		ProjType = ModContent.ProjectileType<Projectiles.GelSlingshot>();

		Item.rare = ItemRarityID.Green;
		Item.value = Item.sellPrice(0, 1, 0, 0);
	}
}