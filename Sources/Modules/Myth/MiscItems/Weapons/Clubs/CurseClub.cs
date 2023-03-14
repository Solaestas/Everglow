using Everglow.Myth;

namespace Everglow.Myth.MiscItems.Weapons.Clubs
{
	public class CurseClub : ClubItem
	{
		public override void SetStaticDefaults()
		{
			ItemGlowManager.AutoLoadItemGlow(this);
		}
		public static short GetGlowMask = 0;
		public override void SetDef()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.damage = 57;
			Item.value = 5000;
			ProjType = ModContent.ProjectileType<Projectiles.CurseClub>();
		}
	}
}
