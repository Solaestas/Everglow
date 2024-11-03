using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class CyanVineRing : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 12;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Green2, 8800);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. 4% Crit chance.
		player.GetCritChance(DamageClass.Generic) += 4;
		player.GetModPlayer<CyanVineRingPlayer>().HasCyanVineRing = true;
	}
}

public class CyanVineRingPlayer : ModPlayer
{
	public bool HasCyanVineRing = false;
	public int HurtTimer = 0;

	public override void ResetEffects()
	{
		HasCyanVineRing = false;
		if (HurtTimer > 0)
		{
			HurtTimer--;
		}
		else
		{
			HurtTimer = 0;
		}
	}

	public override void PostHurt(Player.HurtInfo info)
	{
		if (HasCyanVineRing)
		{
			HurtTimer = 60;
		}
	}

	public override void OnHitAnything(float x, float y, Entity victim)
	{
		if(HurtTimer > 0)
		{
			Player.AddBuff(ModContent.BuffType<CyanVineRingCrit>(), 750);
		}
	}
}