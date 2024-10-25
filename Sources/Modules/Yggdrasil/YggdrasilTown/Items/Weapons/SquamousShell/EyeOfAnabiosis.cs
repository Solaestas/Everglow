using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;

public class EyeOfAnabiosis : ModItem
{
	public const int UseTime = 22;

	private int WeaponProjectileIndex { get; set; } = 0;

	private Projectile WeaponProjectile => Main.projectile[WeaponProjectileIndex];

	public override void SetDefaults()
	{
		Item.width = 72;
		Item.height = 66;

		Item.DamageType = DamageClass.Magic;
		Item.damage = 27;
		Item.knockBack = 3f;
		Item.crit = 14;
		Item.mana = 8;

		Item.useTime = Item.useAnimation = UseTime;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item117;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		Item.value = Item.sellPrice(gold: 1, silver: 3);
		Item.rare = ItemRarityID.Green;

		Item.shoot = ModContent.ProjectileType<EyeOfAnabiosis_Weapon>();
		Item.shootSpeed = 7;
	}

	public override void HoldItem(Player player)
	{
		bool hasGeneratedWeaponProj = WeaponProjectileIndex > -1 && WeaponProjectile is not null && WeaponProjectile.type == ModContent.ProjectileType<EyeOfAnabiosis_Weapon>() && WeaponProjectile.owner == player.whoAmI && WeaponProjectile.timeLeft > 0;

		Vector2 position = player.Center;
		if (player.HeldItem == Item && hasGeneratedWeaponProj is false)
		{
			WeaponProjectileIndex = Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, Vector2.Zero, ModContent.ProjectileType<EyeOfAnabiosis_Weapon>(), 0, 0, player.whoAmI);
		}
	}

	public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;
}