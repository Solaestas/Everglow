using Everglow.Yggdrasil.KelpCurtain.NPCs;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Critters;

public class RiverSlugItem : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 5;
	}

	public override void SetDefaults()
	{
		Item.width = 36;
		Item.height = 26;

		Item.maxStack = Item.CommonMaxStack;
		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(0, 0, 30);

		Item.consumable = true;
		Item.bait = 30;
		Item.makeNPC = ModContent.NPCType<RiverSlug>();

		Item.useStyle = ItemUseStyleID.Swing;
		Item.autoReuse = true;
		Item.useTurn = true;
		Item.useAnimation = 25;
		Item.useTime = 25;
		Item.noUseGraphic = true;
	}

	public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
	{
		itemGroup = ContentSamples.CreativeHelper.ItemGroup.Critters;
	}
}