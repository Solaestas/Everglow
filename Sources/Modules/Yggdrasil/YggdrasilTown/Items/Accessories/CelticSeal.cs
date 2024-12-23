namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

/// <summary>
/// While player taked damage over 6 for 3 times, heal by 15.
/// </summary>
public class CelticSeal : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 30;
		Item.accessory = true;
		Item.rare = ItemRarityID.Blue;
		Item.value = 5055;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.GetModPlayer<CelticSealPlayer>().CelticSealEnable = true;
	}
}

public class CelticSealPlayer : ModPlayer
{
	public bool CelticSealEnable = false;
	public int HitCount;

	public override void ResetEffects()
	{
		CelticSealEnable = false;
	}

	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
	{
		if (CelticSealEnable)
		{
			if (hurtInfo.Damage >= 6)
			{
				HitCount++;
				if (HitCount >= 2)
				{
					Player.Heal(15);
					HitCount = 0;
				}
			}
		}
		else
		{
			HitCount = 0;
		}
	}

	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
	{
		if (CelticSealEnable)
		{
			if (hurtInfo.Damage >= 6)
			{
				HitCount++;
				if (HitCount >= 2)
				{
					Player.Heal(15);
					HitCount = 0;
				}
			}
		}
		else
		{
			HitCount = 0;
		}
	}
}