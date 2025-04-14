using Everglow.Commons.Mechanics.MissionSystem.Objectives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Terraria;
using Terraria.ID;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class CollectItemObjectiveTest
{
	public TestContext TestContext { get; set; } = default!;

	[TestMethod]
	public void Progress_Should_CalculateCorrectly_When_TypeIsSingle()
	{
		var random = new Random();
		for (int i = 0; i < 100; i++)
		{
			int testStack = random.Next(30, 120);
			var objective = new CollectItemObjective(CountItemRequirement.Create([ItemID.DirtBlock], testStack, false));

			var player = new Player();
			player.inventory = [];

			Assert.IsTrue(objective.DemandCollectItem.Progress(player) == 0f);

			for (int stack = 0; stack <= testStack; stack++)
			{
				player.inventory =
				[
					new Item()
					{
						type = ItemID.DirtBlock,
						stack = stack,
					}
				];

				var targetProgress = stack / (float)testStack;
				var actualProgress = objective.DemandCollectItem.Progress(player);
				Assert.IsTrue(actualProgress == targetProgress);
			}
		}
	}

	[TestMethod]
	public void Progress_Should_CalculateCorrectly_When_TypeIsMultiple()
	{
		for (int i = 0; i < 30; i++)
		{
			var random = new Random();

			int type1 = ItemID.DirtBlock;
			int testStack1 = random.Next(23, 48);

			int type2 = ItemID.Wood;

			int type3 = ItemID.IronOre;

			var mission = new CollectItemObjective(CountItemRequirement.Create([type1, type2, type3], testStack1, false));

			var player = new Player();
			player.inventory = [];

			Assert.IsTrue(mission.DemandCollectItem.Progress(player) == 0f);

			for (int stack1 = 0; stack1 <= testStack1; stack1++)
			{
				for (int stack2 = 0; stack2 <= testStack1; stack2++)
				{
					for (int stack3 = 0; stack3 <= testStack1; stack3++)
					{
						player.inventory =
						[
							new Item()
							{
								type = type1,
								stack = stack1,
							},
							new Item()
							{
								type = type2,
								stack = stack2,
							},
							new Item()
							{
								type = type3,
								stack = stack3,
							},
						];
						var targetProgress = Math.Clamp((stack1 + stack2 + stack3) / (float)testStack1, 0f, 1f);
						var acturalProgress = mission.DemandCollectItem.Progress(player);
						Assert.IsTrue(acturalProgress == targetProgress);
					}
				}
			}
		}
	}

	[TestMethod]
	public void Progress_Should_CappedAtOne()
	{
		for (int i = 0; i < 100; i++)
		{
			int testStack = (int)new Random().NextInt64(50, 200);

			var objective = new CollectItemObjective(
				CountItemRequirement.Create([ItemID.DirtBlock], testStack, false));

			var player = new Player();
			player.inventory = [];

			Assert.IsTrue(objective.DemandCollectItem.Progress(player) == 0f);

			for (int stack = testStack; stack <= 2 * testStack; stack++)
			{
				player.inventory =
				[
					new Item()
					{
						type = ItemID.DirtBlock,
						stack = stack,
					},
				];

				Assert.IsTrue(objective.DemandCollectItem.Progress(player) == 1f);
			}
		}
	}

	[TestMethod]
	public void CreateRequirement_Should_ThrowInvalidDataException_When_ParamIsEmpty()
	{
		Assert.ThrowsException<InvalidDataException>((Action)(() =>
		{
			CountItemRequirement.Create([], 1);
		}));

		Assert.ThrowsException<InvalidDataException>((Action)(() =>
		{
			CountItemRequirement.Create([ItemID.DirtBlock], 0);
		}));
	}

	[TestMethod]
	public void Test_CreatePlayerInstanceInUnitTest()
	{
		var player = new Player();
		player.inventory = new Item[59];
		TestContext.WriteLine("Inventory Size: " + player.inventory.Length.ToString());
	}
}