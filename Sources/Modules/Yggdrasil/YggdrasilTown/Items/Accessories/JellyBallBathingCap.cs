namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class JellyBallBathingCap : ModItem
{
	public const float EffectTriggerChance = 0.05f;
	public const float DamageReduction = 0.5f;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(silver: 43, copper: 50);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 2 defense
		// ==============
		player.statDefense += 2;

		// 2. + 1 minion slot
		// ==================
		player.maxMinions += 1;

		// 3. Has chance to decrease projectile damage and reflect projectile
		// ==================================================================
		player.GetModPlayer<JellyBallBathingCapPlayer>().JellyBallBathingCapEnable = true;
	}
}

public class JellyBallBathingCapPlayer : ModPlayer
{
	public bool JellyBallBathingCapEnable { get; set; }

	public override void ResetEffects()
	{
		JellyBallBathingCapEnable = false;
	}

	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
	{
		if (JellyBallBathingCapEnable)
		{
			// 3. Has 5% chance to reduce 50% damage and reflect projectile
			if (Main.rand.NextFloat() < JellyBallBathingCap.EffectTriggerChance)
			{
				// 3.1 Reduce 50% final damage
				modifiers.FinalDamage *= 1.0f - JellyBallBathingCap.DamageReduction;

				// 3.2 Reflect projectile
				proj.hostile = false;
				proj.friendly = true;
				proj.velocity *= -1f;
				proj.extraUpdates += 1;
				proj.penetrate = 1;
			}
		}
	}
}