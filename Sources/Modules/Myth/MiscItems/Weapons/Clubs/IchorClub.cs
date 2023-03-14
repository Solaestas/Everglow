namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
	public class IchorClub : ClubItem
	{
		public override void SetStaticDefaults()
		{
			ItemGlowManager.AutoLoadItemGlow(this);
		}
		public static short GetGlowMask = 0;
		public override void SetDef()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.damage = 47;
			Item.value = 5000;
			ProjType = ModContent.ProjectileType<Projectiles.IchorClub>();
		}
	}
}