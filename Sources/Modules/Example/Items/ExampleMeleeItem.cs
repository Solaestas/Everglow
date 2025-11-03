using Everglow.Commons.MEAC;
using Everglow.Commons.Templates.Weapons.Clubs;
using Everglow.Example.Projectiles;

namespace Everglow.Example.Items;

public class ExampleMeleeItem : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 82;
		Item.shoot = ModContent.ProjectileType<ExampleMeleeProjectile>();
	}

	public override void HoldItem(Player player)
	{
		if (player.ownedProjectileCounts[Item.shoot] <= 0)
		{
			Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromAI(),player.Center,Vector2.zeroVector, Item.shoot, 60,4,player.whoAmI);
			MeleeProj_3D mProj3 = proj.ModProjectile as MeleeProj_3D;
			if (mProj3 != null)
			{
				mProj3.WeaponItemType = Type;
			}
		}
	}
}