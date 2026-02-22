using Everglow.Commons.Utilities;

namespace Everglow.Commons.MEAC;

public abstract class MeleeItem_3D : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MeleeWeapons;

	public override void SetDefaults()
	{
		// fixed defaults for all melee items using MeleeProj_3D
		Item.DamageType = DamageClass.Melee;
		Item.autoReuse = false;

		// These are just example values, you can change them in derived classes
		Item.width = 48;
		Item.height = 48;
		Item.damage = 24;
		Item.knockBack = 3;
		Item.useTime = Item.useAnimation = 24;
		Item.rare = ItemRarityID.White;
		Item.value = 15000;

		SetCustomDefaults();
	}

	public virtual void SetCustomDefaults()
	{
		// You can set custom defaults for each weapon in this method in derived classes, which will be called in SetDefaults.
	}

	public override void HoldItem(Player player)
	{
		if (player.ownedProjectileCounts[Item.shoot] <= 0)
		{
			Projectile proj = Projectile.NewProjectileDirect(player.GetSource_FromAI(), player.Center, Vector2.zeroVector, Item.shoot, 60, 6, player.whoAmI);
			MeleeProj_3D m3 = proj.ModProjectile as MeleeProj_3D;
			if (m3 != null)
			{
				m3.WeaponItemType = Type;
			}
		}
	}
}