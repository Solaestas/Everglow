using Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;
using Everglow.Yggdrasil.YggdrasilTown.NPCs.KingJellyBall;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class JellyBallDaifuku : ModItem
{
	public override void SetDefaults()
	{
		Item.consumable = true;
		Item.width = 42;
		Item.height = 24;
		Item.value = 7500;
		Item.maxStack = Item.CommonMaxStack;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public override bool? UseItem(Player player)
	{
		int type = ModContent.NPCType<KingJellyBall>();
		if (NPC.CountNPCS(type) <= 0)
		{
			NPC npc = NPC.NewNPCDirect(Item.GetSource_FromAI(), player.Center + new Vector2(player.direction * 1800, 0), type);
			npc.velocity.X = -player.direction * 12;
		}
		return base.UseItem(player);
	}

	public override bool CanUseItem(Player player)
	{
		int type = ModContent.NPCType<KingJellyBall>();
		if (NPC.CountNPCS(type) <= 0)
		{
			NPC npc = NPC.NewNPCDirect(Item.GetSource_FromAI(), player.Center + new Vector2(player.direction * 1800, 0), type);
			npc.velocity.X = -player.direction * 12;
		}
		else
		{
			return false;
		}
		return base.CanUseItem(player);
	}

	public override void AddRecipes()
	{
		CreateRecipe(1)
			.AddIngredient(ItemID.Gel, 60)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}