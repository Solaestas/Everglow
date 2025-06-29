using Everglow.Yggdrasil.KelpCurtain.Buffs;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons.DevilHeart;

public class DevilHeartGun : ModItem
{
	public override void SetDefaults()
	{
		Item.height = 26;
		Item.width = 42;

		Item.damage = 13;
		Item.DamageType = DamageClass.Ranged;
		Item.crit = 4;
		Item.knockBack = 0f;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item11;
		Item.useTime = Item.useAnimation = 20;
		Item.noMelee = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(0, 0, 50, 0);

		Item.useAmmo = AmmoID.Bullet;
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