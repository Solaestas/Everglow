using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class GlowstickLauncher : ModItem
{
	private static List<int> ammoTypes =
	[
		ItemID.Glowstick,
		ItemID.StickyGlowstick,
		ItemID.BouncyGlowstick,
		ItemID.FairyGlowstick,
		ItemID.SpelunkerGlowstick,
	];

	public int ConsumedAmmoType { get; set; }

	public override void SetDefaults()
	{
		Item.width = 22;
		Item.height = 22;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 20;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.channel = true;

		Item.DamageType = DamageClass.Default;
		Item.damage = 0;
		Item.knockBack = 0;
		Item.crit = 0;

		Item.value = Item.buyPrice(gold: 1);
		Item.rare = ItemRarityID.Blue;

		Item.shoot = ProjectileID.StickyGlowstick;
		Item.shootSpeed = 12;
	}

	public override bool CanUseItem(Player player) => ammoTypes.Select(player.HasItem).Any(has => has);

	public override bool? UseItem(Player player)
	{
		foreach (var type in ammoTypes)
		{
			if (player.HasItem(type))
			{
				player.ConsumeItem(type);
				switch (type)
				{
					case ItemID.Glowstick:
						ConsumedAmmoType = ProjectileID.StickyGlowstick;
						break;
					case ItemID.StickyGlowstick:
						ConsumedAmmoType = ProjectileID.StickyGlowstick;
						break;
					case ItemID.BouncyGlowstick:
						ConsumedAmmoType = ProjectileID.BouncyGlowstick;
						break;
					case ItemID.FairyGlowstick:
						ConsumedAmmoType = ProjectileID.FairyGlowstick;
						break;
					case ItemID.SpelunkerGlowstick:
						ConsumedAmmoType = ProjectileID.SpelunkerGlowstick;
						break;
				}

				return true;
			}
		}

		return false;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Projectile.NewProjectile(source, position, velocity, ConsumedAmmoType, damage, knockback, player.whoAmI);
		return false;
	}
}