namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.Furnace;

[AutoloadEquip(EquipType.Balloon)]
public class HotAirBalloon : ModItem
{
	public const int JumpSpeedBoost = 2;

	public override void SetStaticDefaults()
	{
		Item.SetNameOverride("Hot Air-balloon");
	}

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.hasVanityEffects = true;
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(gold: 1, silver: 50);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. Enable Jump Boost
		// ====================
		player.jumpBoost = true;

		// 2. Increase Jump Speed
		// ====================================
		player.jumpSpeedBoost += JumpSpeedBoost;

		// 3. Enable Fire Dust VFX
		// =======================
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

	// 3. Enable Fire Dust VFX
	// =======================
	public override void PostUpdateMiscEffects()
	{
		if (HotAirBalloonEnable)
		{
			// If the player is moving
			if (Player.velocity.Length() <= 1E-05f)
			{
				return;
			}

			// Draw dust
			if(Main.rand.NextBool(1))
			{
				Dust dust = Dust.NewDustDirect(Player.position - new Vector2(2), Player.width + 4, Player.height + 4, DustID.Torch, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default(Color), 1.6f);
				dust.noGravity = true;
				dust.velocity *= 1.8f;
				dust.velocity.Y -= 0.5f;
			}
		}
	}
}