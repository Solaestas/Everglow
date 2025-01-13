namespace Everglow.Food.FoodRecipes;

public abstract class FoodRecipes : ModSystem
{

	public List<CookingUnitWithOrder> CookingUnitWithOrderMenu;
	public List<CookingUnit> CookingUnitMenu;

	public override void Unload()
	{
		CookingUnitWithOrderMenu.Clear();
		CookingUnitMenu.Clear();
	}

	public struct CookingUnitWithOrder
	{
		public int[][] Ingredients;
		public int Type;
		public int Num;

		public CookingUnitWithOrder(int type, int num , params int[][] itemgroup)
		{
			Ingredients = itemgroup;

			Type = type;

			Num = num;
		}
	}

	public struct CookingUnit
	{
		public List<int[]> Ingredients;
		public int Type;
		public int Num;

		public CookingUnit(int type, int num, params int[][] itemgroup)
		{

			Ingredients = itemgroup.ToList();

			Type = type;

			Num = num;
		}
	}
	


	public static int[] AnyDuck = RecipeGroup.recipeGroups[RecipeGroupID.Ducks].ValidItems.ToArray();

	public static int[] AnyButterfly = RecipeGroup.recipeGroups[RecipeGroupID.Butterflies].ValidItems.ToArray();

	public static int[] AnyFruit = RecipeGroup.recipeGroups[RecipeGroupID.Fruit].ValidItems.Concat([ItemID.Grapes]).ToArray();

	public static int[] AnyTurtle = RecipeGroup.recipeGroups[RecipeGroupID.Turtles].ValidItems.ToArray();

	public static int[] AnyBug = RecipeGroup.recipeGroups[RecipeGroupID.Bugs].ValidItems.ToArray();

	public static int[] AnySquirrel = RecipeGroup.recipeGroups[RecipeGroupID.Squirrels].ValidItems.ToArray();

	public static int[] AnyDragonfly = RecipeGroup.recipeGroups[RecipeGroupID.Dragonflies].ValidItems.ToArray();

	public static int[] AnySnail = RecipeGroup.recipeGroups[RecipeGroupID.Snails].ValidItems.ToArray();

	public static int[] AnyFirefly = RecipeGroup.recipeGroups[RecipeGroupID.Fireflies].ValidItems.ToArray();

	public static int[] AnyScorpion = RecipeGroup.recipeGroups[RecipeGroupID.Scorpions].ValidItems.ToArray();

	public static int[] AnyParrot = RecipeGroup.recipeGroups[RecipeGroupID.Cockatiels].ValidItems.Concat(RecipeGroup.recipeGroups[RecipeGroupID.Macaws].ValidItems).ToArray();

	public static int[] AnyBird = RecipeGroup.recipeGroups[RecipeGroupID.Birds].ValidItems.Concat(AnyParrot).ToArray();

	public static int[] AnyQuestFish = Main.anglerQuestItemNetIDs;

	public static int[] AnySpecialFish = [
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

	public static int[] AnyNormalFish = [
		ItemID.AtlanticCod,
		ItemID.Bass,
		ItemID.Flounder,
		ItemID.RedSnapper,
		ItemID.Salmon,
		ItemID.Trout,
		ItemID.Tuna,
	    ];

	public static int[] AnyFish = AnyQuestFish.Concat(AnySpecialFish).Concat(AnyNormalFish).ToArray();

}
