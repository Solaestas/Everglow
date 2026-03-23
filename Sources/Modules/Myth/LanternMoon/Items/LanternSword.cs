using Everglow.Myth.LanternMoon.Projectiles.Weapons;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Items;

/// <summary>
/// Mark target with a lantern label.
/// Do at least 500 damage to a labeled target will remove the label and trigger an explosion.
/// </summary>
public class LanternSword : MeleeItem_3D
{
	public override void SetCustomDefaults()
	{
		Item.damage = 140;
		Item.knockBack = 3;
		Item.width = 50;
		Item.height = 50;
		Item.rare = ItemRarityID.Lime;
		Item.value = 15000;

		Item.shoot = ModContent.ProjectileType<LanternSword_Proj>();
	}

	public override void HoldItem(Player player)
	{
		if (player.ownedProjectileCounts[Item.shoot] <= 0)
		{
			Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromAI(), player.Center, Vector2.zeroVector, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
			MeleeProj_3D m3 = proj.ModProjectile as MeleeProj_3D;
			if (m3 != null)
			{
				m3.WeaponItemType = Type;
			}
		}
	}
}