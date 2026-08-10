using System.Reflection;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.PlayerSide.MissionStructure.Nodes;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class PlayerObjectiveNodePresentationStateTest
{
	private sealed class TestObjective : PlayerObjectiveBase
	{
		public bool Ready { get; set; }

		public override bool CheckCompletion() => Ready;

		public override void GetObjectivesIcon(MissionIconGroup iconGroup)
		{
		}

		public override void GetObjectivesText(List<string> lines)
		{
		}
	}

	[TestMethod]
	public void Leaf_ExposesObjectiveThroughInternalGetter()
	{
		var objective = new TestObjective();
		var node = new PlayerLeafNode(objective);
		PropertyInfo property = GetInternalGetter(typeof(PlayerLeafNode), "Objective");

		Assert.AreEqual(typeof(PlayerObjectiveBase), property.PropertyType);
		Assert.AreSame(objective, property.GetValue(node));
	}

	[TestMethod]
	public void ParallelAndOptional_ExposeOrderedReadOnlyObjectives()
	{
		var first = new TestObjective();
		var second = new TestObjective();

		AssertObjectiveSequence(new PlayerParallelNode([first, second]), first, second);
		AssertObjectiveSequence(new PlayerOptionalNode([first, second]), first, second);
	}

	[TestMethod]
	public void Branch_ExposesOrderedReadOnlyBranchesAndSelection()
	{
		var first = new TestObjective();
		var second = new TestObjective();
		var third = new TestObjective { Ready = true };
		var node = new PlayerBranchNode([[first, second], [third]]);
		PropertyInfo branchesProperty = GetInternalGetter(typeof(PlayerBranchNode), "Branches");
		PropertyInfo selectedProperty = GetInternalGetter(typeof(PlayerBranchNode), "SelectedBranchIndex");
		var branches = (IReadOnlyList<IReadOnlyList<PlayerObjectiveBase>>)branchesProperty.GetValue(node)!;

		Assert.HasCount(2, branches);
		CollectionAssert.AreEqual(new PlayerObjectiveBase[] { first, second }, branches[0].ToArray());
		CollectionAssert.AreEqual(new PlayerObjectiveBase[] { third }, branches[1].ToArray());
		AssertReadOnly(branches[0]);
		AssertReadOnly(branches[1]);
		var mutableBranches = (IList<IReadOnlyList<PlayerObjectiveBase>>)branches;
		Assert.ThrowsExactly<NotSupportedException>(() => mutableBranches[0] = Array.Empty<PlayerObjectiveBase>());
		Assert.IsNull(selectedProperty.GetValue(node));

		node.Complete();

		Assert.AreEqual(1, selectedProperty.GetValue(node));
	}

	private static void AssertObjectiveSequence(object node, params PlayerObjectiveBase[] expected)
	{
		PropertyInfo property = GetInternalGetter(node.GetType(), "Objectives");
		var objectives = (IReadOnlyList<PlayerObjectiveBase>)property.GetValue(node)!;

		CollectionAssert.AreEqual(expected, objectives.ToArray());
		AssertReadOnly(objectives);
	}

	private static void AssertReadOnly(IReadOnlyList<PlayerObjectiveBase> objectives)
	{
		var mutable = (IList<PlayerObjectiveBase>)objectives;
		Assert.ThrowsExactly<NotSupportedException>(() => mutable.Add(new TestObjective()));
		Assert.ThrowsExactly<NotSupportedException>(() => mutable[0] = new TestObjective());
	}

	private static PropertyInfo GetInternalGetter(Type type, string name)
	{
		PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic);
		Assert.IsNotNull(property);
		Assert.IsNotNull(property.GetMethod);
		Assert.IsTrue(property.GetMethod.IsAssembly);
		Assert.IsFalse(property.CanWrite);
		return property;
	}
}
