using Everglow.Commons.MissionSystem.Abstracts.Missions;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.UnitTests.Functions.MissionSystem.TestMissions;
using Terraria;
using Terraria.ID;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class GainItemMissionTest
{
	public TestContext TestContext { get; set; } = default!;

	[TestMethod]
	public void Progress_Should_CalculateCorrectly_When_TypeIsSingle()
	{
		int testStack = 97;
		var mission = new UnitTestGainItemMission1([
			GainItemRequirement.Create([ItemID.DirtBlock], testStack),
			]);

		Assert.IsTrue(mission.Progress == 0f);

		for (int stack = 0; stack <= testStack; stack++)
		{
			var inventory = new List<Item>()
			{
				new Item()
				{
					type = ItemID.DirtBlock,
					stack = stack,
				},
			};

			var targetProgress = stack / (float)testStack;
			Assert.IsTrue((mission as IGainItemMission).CalculateProgress(inventory) == targetProgress);
		}
	}

	[TestMethod]
	public void Progress_Should_CalculateCorrectly_When_TypeIsMultiple()
	{
		for (int i = 0; i < 30; i++)
		{
			var random = new Random();

			int testStack1 = (int)random.NextInt64(23, 61);
			int testStack2 = (int)random.NextInt64(23, 61);
			int testStack3 = (int)random.NextInt64(23, 61);

			var mission = new UnitTestGainItemMission1([
				GainItemRequirement.Create([ItemID.DirtBlock], testStack1),
				GainItemRequirement.Create([ItemID.Wood], testStack2),
				GainItemRequirement.Create([ItemID.IronOre], testStack3),
			]);

			Assert.IsTrue(mission.Progress == 0f);

			for (int stack1 = 0; stack1 <= testStack1; stack1++)
			{
				for (int stack2 = 0; stack2 <= testStack2; stack2++)
				{
					for (int stack3 = 0; stack3 <= testStack3; stack3++)
					{
						var inventory = new List<Item>()
						{
							new Item()
							{
								type = ItemID.DirtBlock,
								stack = stack1,
							},
							new Item()
							{
								type = ItemID.Wood,
								stack = stack2,
							},
							new Item()
							{
								type = ItemID.IronOre,
								stack = stack3,
							},
						};
						List<float> floats = [
							stack1 / (float)testStack1,
							stack2 / (float)testStack2,
							stack3 / (float)testStack3,
						];

						Assert.IsTrue((mission as IGainItemMission).CalculateProgress(inventory) == floats.Average());
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

			var mission = new UnitTestGainItemMission1([
				GainItemRequirement.Create([ ItemID.DirtBlock], testStack),
				]);

			Assert.IsTrue(mission.Progress == 0f);

			for (int stack = testStack; stack <= 2 * testStack; stack++)
			{
				var inventory = new List<Item>()
				{
					new Item()
					{
						type = ItemID.DirtBlock,
						stack = stack,
					},
				};

				Assert.IsTrue((mission as IGainItemMission).CalculateProgress(inventory) == 1f);
			}
		}
	}

	[TestMethod]
	public void Progress_Should_SetMax_When_DemandItemsIsEmpty()
	{
		var mission = new UnitTestGainItemMission1([]);
		Assert.IsTrue((mission as IGainItemMission).CalculateProgress([]) == 1f);
	}
}