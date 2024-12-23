using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class ThermostatRod : ModItem
{
	public override void SetStaticDefaults()
	{
		ItemID.Sets.CanFishInLava[Item.type] = true;
	}

	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.WoodFishingPole);

		Item.value = Item.buyPrice(silver: 10);
		Item.rare = ItemRarityID.White;

		Item.fishingPole = 10;
		Item.shootSpeed = 12f;
		Item.shoot = ModContent.ProjectileType<ThermostatBobber>();
	}

	public override void HoldItem(Player player)
	{
		player.accFishingLine = true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Projectile.NewProjectile(source, position, velocity, type, 0, 0f, player.whoAmI);
		return false;
	}

	public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
	{
		lineOriginOffset = new Vector2(36, -20);
		lineColor = ThermostatBobber.FishingLineColor;
	}
}