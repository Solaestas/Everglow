namespace Everglow.Commons.ItemAbstracts;

public abstract class FishBase : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 10;
		ItemID.Sets.CanBePlacedOnWeaponRacks[Item.type] = true;
	}

	public override void SetDefaults()
	{
		Item.maxStack = Item.CommonMaxStack;
	}

	public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
	{
		itemGroup = ContentSamples.CreativeHelper.ItemGroup.Fish;
	}
}