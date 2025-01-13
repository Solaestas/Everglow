namespace Everglow.Commons.Utilities;

public class ItemUtils
{
	public static float GetPrefixDamage(int prefixType)
	{
		return prefixType switch
		{
			PrefixID.Dangerous => 1.05f,
			PrefixID.Savage => 1.1f,
			PrefixID.Sharp => 1.15f,
			PrefixID.Pointy => 1.1f,    // ？？？怎么这个也是1.1f
			PrefixID.Legendary => 1.15f,
			PrefixID.Terrible => 0.85f,
			PrefixID.Dull => 0.85f,
			PrefixID.Bulky => 1.05f,
			PrefixID.Shameful => 0.9f,
			PrefixID.Sighted => 1.1f,
			PrefixID.Deadly => 1.1f,
			PrefixID.Staunch => 1.1f,
			PrefixID.Unreal => 1.15f,
			PrefixID.Awful => 0.85f,
			PrefixID.Powerful => 1.15f,
			PrefixID.Frenzying => 0.85f,
			PrefixID.Mystic => 1.1f,
			PrefixID.Masterful => 1.15f,
			PrefixID.Mythical => 1.15f,
			PrefixID.Ignorant => 0.9f,
			PrefixID.Deranged => 0.9f,
			PrefixID.Intense => 1.1f,
			PrefixID.Celestial => 1.1f,
			PrefixID.Furious => 1.15f,
			PrefixID.Manic => 0.9f,
			PrefixID.Legendary2 => 1.17f,
			PrefixID.Superior => 1.1f,
			PrefixID.Hurtful => 1.1f,
			PrefixID.Unpleasant => 1.05f,
			PrefixID.Godly => 1.15f,
			PrefixID.Demonic => 1.15f,
			PrefixID.Broken => 0.7f,
			PrefixID.Damaged => 0.85f,
			PrefixID.Shoddy => 0.9f,
			PrefixID.Ruthless => 1.18f,
			PrefixID.Deadly2 => 1.1f,
			PrefixID.Murderous => 1.07f,
			PrefixID.Annoying => 0.8f,
			PrefixID.Nasty => 1.05f,
			_ => 1f,
		};
	}

	public static List<int> VanillaDuck => RecipeGroup.recipeGroups[RecipeGroupID.Ducks].ValidItems.ToList();

	public static List<int> VanillaButterfly => RecipeGroup.recipeGroups[RecipeGroupID.Butterflies].ValidItems.ToList();

	public static List<int> VanillaFruit => RecipeGroup.recipeGroups[RecipeGroupID.Fruit].ValidItems.Concat([ItemID.Grapes]).ToList();

	public static List<int> VanillaTurtle => RecipeGroup.recipeGroups[RecipeGroupID.Turtles].ValidItems.ToList();

	public static List<int> VanillaBug => RecipeGroup.recipeGroups[RecipeGroupID.Bugs].ValidItems.ToList();

	public static List<int> VanillaSquirrel => RecipeGroup.recipeGroups[RecipeGroupID.Squirrels].ValidItems.ToList();

	public static List<int> VanillaDragonfly => RecipeGroup.recipeGroups[RecipeGroupID.Dragonflies].ValidItems.ToList();

	public static List<int> VanillaSnail => RecipeGroup.recipeGroups[RecipeGroupID.Snails].ValidItems.ToList();

	public static List<int> VanillaFirefly => RecipeGroup.recipeGroups[RecipeGroupID.Fireflies].ValidItems.ToList();

	public static List<int> VanillaScorpion => RecipeGroup.recipeGroups[RecipeGroupID.Scorpions].ValidItems.ToList();

	public static List<int> VanillaParrot => RecipeGroup.recipeGroups[RecipeGroupID.Cockatiels].ValidItems.Concat(RecipeGroup.recipeGroups[RecipeGroupID.Macaws].ValidItems).ToList();

	public static List<int> VanillaBird => RecipeGroup.recipeGroups[RecipeGroupID.Birds].ValidItems.Concat(VanillaParrot).ToList();

	public static List<int> VanillaQuestFish => Main.anglerQuestItemNetIDs.ToList();

	public static List<int> VanillaSpecialFish => [
		ItemID.ArmoredCavefish,
		ItemID.ChaosFish,
		ItemID.CrimsonTigerfish,
		ItemID.Damselfish,
		ItemID.DoubleCod,
		ItemID.Ebonkoi,
		ItemID.FlarefinKoi,
		ItemID.FrostMinnow,
		ItemID.Hemopiranha,
		ItemID.Honeyfin,
		ItemID.NeonTetra,
		ItemID.Obsidifish,
		ItemID.PrincessFish,
		ItemID.Prismite,
		ItemID.SpecularFish,
		ItemID.Stinkfish,
		ItemID.VariegatedLardfish,
		];

	public static List<int> VanillaNormalFish => [
		ItemID.AtlanticCod,
		ItemID.Bass,
		ItemID.Flounder,
		ItemID.RedSnapper,
		ItemID.Salmon,
		ItemID.Trout,
		ItemID.Tuna,
		];

	public static List<int> VanillaFish => VanillaQuestFish.Concat(VanillaSpecialFish).Concat(VanillaNormalFish).ToList();
}