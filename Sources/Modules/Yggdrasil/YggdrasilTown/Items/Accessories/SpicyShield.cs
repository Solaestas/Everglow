using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class SpicyShield : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 48;
		Item.height = 46;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Green2, 7500);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 3 Defense
		player.statDefense += 4;

		// 2. + 3% All damage
		player.allDamage += 0.04f;

		// 3.Add a 30s thorn buff after hurt.
		player.GetModPlayer<SpicyShieldPlayer>().HasSpicyShield = true;
	}
}

public class SpicyShieldPlayer : ModPlayer
{
	public bool HasSpicyShield = false;

	public override void ResetEffects()
	{
		HasSpicyShield = false;
	}

	public override void PostHurt(Player.HurtInfo info)
	{
		if (HasSpicyShield)
		{
			if (!Player.HasBuff(BuffID.Thorns))
			{
				Player.AddBuff(BuffID.Thorns, 1800);
			}
			if (Player.FindBuffIndex(BuffID.Thorns) >= 0)
			{
				if (Player.buffTime[Player.FindBuffIndex(BuffID.Thorns)] < 1800)
				{
					Player.AddBuff(BuffID.Thorns, 1800);
				}
			}
		}
		base.PostHurt(info);
	}
}