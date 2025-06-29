using Everglow.Yggdrasil.KelpCurtain.Buffs;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons.DevilHeart;

public class DevilHeartBow : ModItem
{
	public override void SetDefaults()
	{
		Item.height = 34;
		Item.width = 26;

		Item.damage = 13;
		Item.DamageType = DamageClass.Ranged;
		Item.crit = 4;
		Item.knockBack = 0f;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item5;
		Item.useTime = Item.useAnimation = 20;
		Item.noMelee = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(0, 0, 50, 0);

		Item.useAmmo = AmmoID.Arrow;
		Item.shoot = ProjectileID.Bullet;
		Item.shootSpeed = 12f;
	}

	public override void HoldItem(Player player)
	{
		if (player.ItemTimeIsZero)
		{
			player.AddBuff(ModContent.BuffType<DevilHeartWeaponBuff>(), 2);
		}
	}
}