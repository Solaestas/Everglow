namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.BookofSkulls;

internal class BookofSkullsBook : MagicBookProjectile
{
	public override void SetDef()
	{
		ProjType = ModContent.ProjectileType<SkullII>();
		DustType = DustID.Bone;
		ItemType = ItemID.BookofSkulls;
		MulStartPosByVelocity = 2f;
		UseGlow = false;
		effectColor = new Color(105, 75, 45, 100);
	}
}