using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class GunOfAvarice : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 62;
		Item.height = 32;
		Item.scale = 0.75f;

		Item.DamageType = DamageClass.Ranged;
		Item.damage = 16;
		Item.knockBack = 1.5f;
		Item.crit = 2;
		Item.noMelee = true;

		Item.useTime = 27;
		Item.useAnimation = 27;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item41;
		Item.autoReuse = false;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.sellPrice(gold: 10);

		Item.shoot = ProjectileID.Bullet;
		Item.shootSpeed = 12;
		Item.useAmmo = AmmoID.Bullet;
	}

	private const int AmmoCapacity = 14;

	private const int AutoReloadDuration = 45;
	private const float AutoReloadSuccess = 0.8f;
	private const float AutoReloadFailureDamageRatio = 2.8f;

	private const int ManualReloadDuration = 15;

	private const float DamageBonusPerLevel = 0.25f;
	private const int MaxLevel = 10;

	private const int TargetValueBonusInCopper = 75;

	private int AmmoAmount { get; set; } = AmmoCapacity;

	public override bool CanUseItem(Player player)
	{
		return AmmoAmount > 0;
	}

	public override bool CanShoot(Player player)
	{
		return AmmoAmount > 0;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		return true;
	}

	public override bool AltFunctionUse(Player player)
	{
		return true;
	}
}