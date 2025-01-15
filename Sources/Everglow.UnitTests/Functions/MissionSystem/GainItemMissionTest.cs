using Everglow.Commons.MissionSystem;
using Everglow.Commons.MissionSystem.MissionTemplates;
using Terraria;
using Terraria.ID;
using static Everglow.Commons.MissionSystem.MissionTemplates.GainItemMission;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class GainItemMissionTest
{
	public TestContext TestContext { get; set; } = default!;

	[TestMethod]
	public void Progress_Should_CalculateCorrectly_When_TypeIsSingle()
	{
		var mission = new GainItemMission
		{
			PoolType = MissionManager.PoolType.Accepted,
		};

		int testStack = 97;
		mission.DemandGainItems.AddRange([
			GainItemRequirement.Create([ ItemID.DirtBlock], testStack),
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
			mission.UpdateProgress(inventory);

			var targetProgress = stack / (float)testStack;
			Assert.IsTrue(mission.Progress == targetProgress);
		}
	}

	[TestMethod]
	public void Progress_Should_CalculateCorrectly_When_TypeIsMultiple()
	{
		for (int i = 0; i < 30; i++)
		{
			var mission = new GainItemMission
			{
				PoolType = MissionManager.PoolType.Accepted,
			};
			var random = new Random();

			int testStack1 = (int)random.NextInt64(23, 61);
			int testStack2 = (int)random.NextInt64(23, 61);
			int testStack3 = (int)random.NextInt64(23, 61);

			mission.DemandGainItems.AddRange([
				GainItemRequirement.Create([ ItemID.DirtBlock], testStack1),
				GainItemRequirement.Create([ ItemID.Wood], testStack2),
				GainItemRequirement.Create([ ItemID.IronOre], testStack3),
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
						mission.UpdateProgress(inventory);
						List<float> floats = [
							stack1 / (float)testStack1,
							stack2 / (float)testStack2,
							stack3 / (float)testStack3,
						];

						Assert.IsTrue(mission.Progress == floats.Average());
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
			var mission = new GainItemMission
			{
				PoolType = MissionManager.PoolType.Accepted,
			};

			int testStack = (int)new Random().NextInt64(50, 200);
			mission.DemandGainItems.AddRange([
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
				mission.UpdateProgress(inventory);

				Assert.IsTrue(mission.Progress == 1f);
			}
		}
	}

	[TestMethod]
	public void Progress_Should_SetMax_When_DemandItemsIsEmpty()
	{
		var mission = new GainItemMission
		{
			PoolType = MissionManager.PoolType.Accepted,
		};
		Assert.IsTrue(mission.Progress == 0f);

		mission.UpdateProgress(new List<Item>());
		Assert.IsTrue(mission.Progress == 1f);
	}

	[TestMethod]
	public void Progress_Should_RemainUnchanged_When_ParamIsNull()
	{
		var mission = new GainItemMission
		{
			PoolType = MissionManager.PoolType.Accepted,
		};
		Assert.IsTrue(mission.Progress == 0f);

		mission.UpdateProgress();
		Assert.IsTrue(mission.Progress == 0f);
		Assert.IsFalse(mission.Progress == 1f);
	}

	[TestMethod]
	public void Progress_Should_RemainUnchanged_When_StatusIsNotAccepted()
	{
		var mission = new GainItemMission
		{
			PoolType = MissionManager.PoolType.Available,
		};
		Assert.IsTrue(mission.Progress == 0f);

		mission.UpdateProgress(new List<Item>());
		Assert.IsTrue(mission.Progress == 0f);
		Assert.IsFalse(mission.Progress == 1f);
	}
}