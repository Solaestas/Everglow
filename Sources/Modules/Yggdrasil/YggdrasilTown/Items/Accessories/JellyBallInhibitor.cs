using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Everglow.Yggdrasil.YggdrasilTown.NPCs.KingJellyBall;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class JellyBallInhibitor : ModItem
{
	public const float JellyBallDamageReduction = 0.25f;
	public const float JellyBallDamageBonus = 0.6f;

	public override void SetDefaults()
	{
		Item.accessory = true;
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(silver: 66, copper: 26);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.GetModPlayer<JellyBallInhibitorPlayer>().JellyBallInhibitorEnable = true;
	}
}

public class JellyBallInhibitorPlayer : ModPlayer
{
	public bool JellyBallInhibitorEnable { get; set; }

	public override void ResetEffects()
	{
		JellyBallInhibitorEnable = false;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		if (JellyBallInhibitorEnable)
		{
			if (target.IsJellyBall())
			{
				modifiers.FinalDamage *= 1 + JellyBallInhibitor.JellyBallDamageBonus;
			}
		}
	}

	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
	{
		if (JellyBallInhibitorEnable)
		{
			if (npc.IsJellyBall())
			{
				modifiers.FinalDamage *= 1 - JellyBallInhibitor.JellyBallDamageReduction;
			}
		}
	}
}

public static class JellyBallInhibitorExtensions
{
	public static bool IsJellyBall(this NPC npc)
	{
		return npc.ModNPC is JellyBall or GiantJellyBall or KingJellyBall;
	}
}