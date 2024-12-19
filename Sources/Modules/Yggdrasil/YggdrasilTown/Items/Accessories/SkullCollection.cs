namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class SkullCollection : ModItem
{
	public const int DetectRange = 96;
	public const int BuffDuration = 180;
	public const int BuffCooldownMax = 120;
	public const int FinalManaRegenBonus = 5;

	public int BuffCooldown { get; set; }

	public override void SetDefaults()
	{
		Item.accessory = true;
		Item.width = 28;
		Item.height = 20;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(platinum: 0, gold: 2);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{

		if (--BuffCooldown < 0)
		{
			BuffCooldown = 0;
		}

		foreach (var npc in Main.ActiveNPCs)
		{
			if (npc.friendly)
			{
				continue;
			}

			// If there's any enemy within 6 blocks / 96 pixels around the player
			var distance = Vector2.Distance(npc.Center, player.Center);
			if (distance <= DetectRange)
			{
				// 1. Add a 3s Mana Regeneration buff to the caster, then this effect goes to a 2s cooldown
				// ========================================================================================
				if (BuffCooldown <= 0)
				{
					player.AddBuff(BuffID.ManaRegeneration, BuffDuration);
					BuffCooldown = BuffCooldownMax;
				}

				// 2. +5 mana regeneration rate
				// ============================
				player.manaRegenBonus += 5;
				break;
			}
		}
	}
}