using System.Reflection;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.WorldSide.MissionStructure.Nodes;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class WorldObjectiveNodePresentationStateTest
{
	private sealed class TestObjective : WorldObjectiveBase
	{
		public bool Ready { get; set; }

		public override bool CheckCompletion() => Ready;

		public override void GetObjectivesText()
		{
		}
	}

	[TestMethod]
	public void Leaf_ExposesObjectiveThroughInternalGetter()
	{
		var objective = new TestObjective();
		var node = new WorldLeafNode(objective);
		PropertyInfo property = GetInternalGetter(typeof(WorldLeafNode), "Objective");

		Assert.AreEqual(typeof(WorldObjectiveBase), property.PropertyType);
		Assert.AreSame(objective, property.GetValue(node));
	}

	[TestMethod]
	public void ParallelAndOptional_ExposeOrderedReadOnlyObjectives()
	{
		var first = new TestObjective();
		var second = new TestObjective();

		AssertObjectiveSequence(new WorldParallelNode([first, second]), first, second);
		AssertObjectiveSequence(new WorldOptionalNode([first, second]), first, second);
	}

	[TestMethod]
	public void Branch_ExposesOrderedReadOnlyBranchesAndSelection()
	{
		var first = new TestObjective();
		var second = new TestObjective();
		var third = new TestObjective { Ready = true };
		var node = new WorldBranchNode([[first, second], [third]]);
		PropertyInfo branchesProperty = GetInternalGetter(typeof(WorldBranchNode), "Branches");
		PropertyInfo selectedProperty = GetInternalGetter(typeof(WorldBranchNode), "SelectedBranchIndex");
		var branches = (IReadOnlyList<IReadOnlyList<WorldObjectiveBase>>)branchesProperty.GetValue(node)!;

		Assert.HasCount(2, branches);
		CollectionAssert.AreEqual(new WorldObjectiveBase[] { first, second }, branches[0].ToArray());
		CollectionAssert.AreEqual(new WorldObjectiveBase[] { third }, branches[1].ToArray());
		AssertReadOnly(branches[0]);
		AssertReadOnly(branches[1]);
		var mutableBranches = (IList<IReadOnlyList<WorldObjectiveBase>>)branches;
		Assert.ThrowsExactly<NotSupportedException>(() => mutableBranches[0] = Array.Empty<WorldObjectiveBase>());
		Assert.IsNull(selectedProperty.GetValue(node));

		node.Complete();

		Assert.AreEqual(1, selectedProperty.GetValue(node));
	}

	private static void AssertObjectiveSequence(object node, params WorldObjectiveBase[] expected)
	{
		PropertyInfo property = GetInternalGetter(node.GetType(), "Objectives");
		var objectives = (IReadOnlyList<WorldObjectiveBase>)property.GetValue(node)!;

		CollectionAssert.AreEqual(expected, objectives.ToArray());
		AssertReadOnly(objectives);
	}

	private static void AssertReadOnly(IReadOnlyList<WorldObjectiveBase> objectives)
	{
		var mutable = (IList<WorldObjectiveBase>)objectives;
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
