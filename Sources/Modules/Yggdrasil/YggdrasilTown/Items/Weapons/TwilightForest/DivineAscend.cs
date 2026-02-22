using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.TwilightForest;

public class DivineAscend : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MeleeWeapons;

	public override void SetDefaults()
	{
		Item.width = 48;
		Item.height = 58;

		Item.DamageType = DamageClass.Melee;
		Item.damage = 27;
		Item.knockBack = 3;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(gold: 3);

		Item.shoot = ModContent.ProjectileType<DivineAscendProj>();
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