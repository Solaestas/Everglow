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
		Item.width = 50;
		Item.height = 25;
		Item.scale = 1.6f;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item108;
		Item.useTime = Item.useAnimation = 16;
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
		Item.shootSpeed = 16;
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
		Projectile.NewProjectile(source, position + 36f * velocity.NormalizeSafe(), velocity, ConsumedAmmoType, damage, knockback, player.whoAmI);
		return false;
	}

	public override void HoldItem(Player player)
	{
		if (player.controlUseItem)
		{
			player.SetArmToFitMousePosition(0.1f);
		}
	}

	public override Vector2? HoldoutOffset() => new Vector2(-16f, -7f);

	public override Vector2? HoldoutOrigin() => new Vector2(25, -25f);
}