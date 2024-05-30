using Terraria.Enums;

namespace Everglow.Yggdrasil.Furnace.Items.Accessories;

[AutoloadEquip(EquipType.Balloon)]
public class HotAirBalloon : ModItem
{
	public const int JumpSpeedBoost = 2;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.hasVanityEffects = true;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 1, silver: 50));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. Enable Jump Boost
		player.jumpBoost = true;

		// 2. Increase Jump Speed (pixels/tick)
		player.jumpSpeedBoost += JumpSpeedBoost;

		// 3. Enable Fire Dust
		player.GetModPlayer<HotAirBalloonPlayer>().HotAirBalloonEnable = true;
	}
}

internal class HotAirBalloonPlayer : ModPlayer
{
	public bool HotAirBalloonEnable = false;

	public override void ResetEffects()
	{
		HotAirBalloonEnable = false;
	}

	// 3. Enable Fire Dust
	public override void PreUpdate()
	{
		if (HotAirBalloonEnable)
		{
			if (!HotAirBalloonEnable)
			{
				return;
			}

			// Check if the player is moving
			if (Player.velocity == Vector2.Zero)
			{
				return;
			}

			// Draw dust
			if (Main.rand.NextBool(1))
			{
				Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch);
			}
		}
	}
}