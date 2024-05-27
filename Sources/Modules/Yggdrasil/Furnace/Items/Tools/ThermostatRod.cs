using Everglow.Yggdrasil.Furnace.Projectiles;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Everglow.Yggdrasil.Furnace.Items.Tools;

public class ThermostatRod : ModItem
{
	public override void SetStaticDefaults()
	{
		ItemID.Sets.CanFishInLava[Item.type] = true;
	}

	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.WoodFishingPole);
		Item.SetShopValues(ItemRarityColor.White0, Item.buyPrice(silver: 10));

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
		int bobberAmount = Main.rand.Next(3, 6); // 3 to 5 bobbers
		float spreadAmount = 75f; // how much the different bobbers are spread out.

		for (int index = 0; index < bobberAmount; ++index)
		{
			Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f, Main.rand.NextFloat(-spreadAmount, spreadAmount) * 0.05f);

			// Generate new bobbers
			Projectile.NewProjectile(source, position, bobberSpeed, type, 0, 0f, player.whoAmI);
		}
		return false;
	}

	public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor)
	{
		// Change these two values in order to change the origin of where the line is being drawn.
		// This will make it draw 43 pixels right and 30 pixels up from the player's center, while they are looking right and in normal gravity.
		lineOriginOffset = new Vector2(43, -30);

		// Sets the fishing line's color. Note that this will be overridden by the colored string accessories.
		if (bobber.ModProjectile is ThermostatBobber exampleBobber)
		{
			// ExampleBobber has custom code to decide on a line color.
			lineColor = exampleBobber.FishingLineColor;
		}
		else
		{
			// If the bobber isn't ExampleBobber, a Fishing Bobber accessory is in effect and we use DiscoColor instead.
			lineColor = Main.DiscoColor;
		}
	}
}