using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission;

/// <summary>
/// Minimal linear structural objective container.
/// Manages objective nodes, lifecycle transitions, persistence, and netcode.
/// </summary>
public class StructuralObjectiveContainer
{
	/// <summary>
	/// Fired when the current node has been structurally completed.
	/// </summary>
	public event Action<WorldObjectiveNodeBase> OnNodeCompleted;

	/// <summary>
	/// Fired when a node becomes the active node.
	/// </summary>
	public event Action<WorldObjectiveNodeBase> OnObjectiveActivated;

	/// <summary>
	/// Fired when a node is no longer the active node.
	/// </summary>
	public event Action<WorldObjectiveNodeBase> OnObjectiveDeactivated;

	/// <summary>
	/// Fired when an objective requires delta-sync transmission.
	/// </summary>
	public event Action<IDeltaSyncObjective> OnMPSyncTriggered;

	/// <summary>
	/// Fired when the active node is changed due to snapshot synchronization.
	/// </summary>
	public event Action<WorldObjectiveNodeBase> OnObjectiveSynced;

	private readonly List<WorldObjectiveNodeBase> _nodes = [];
	private readonly List<WorldObjectiveBase> _objectives = [];

	/// <summary>
	/// All structural nodes in linear order.
	/// </summary>
	public IReadOnlyList<WorldObjectiveNodeBase> AllNodes => _nodes;

	/// <summary>
	/// All objectives in the order they were added.
	/// </summary>
	public IReadOnlyList<WorldObjectiveBase> AllObjectives => _objectives;

	/// <summary>
	/// The currently active structural node.
	/// </summary>
	public WorldObjectiveNodeBase Current { get; private set; }

	/// <summary>
	/// Whether all nodes have been structurally completed.
	/// </summary>
	public bool Completed => FindCurrentNode() == null;

	#region DSL

	/// <summary>
	/// Adds a linear objective and creates a corresponding leaf node.
	/// </summary>
	public StructuralObjectiveContainer Add(WorldObjectiveBase objective)
	{
		objective.ObjectiveID = _objectives.Count;
		_objectives.Add(objective);

		var leaf = new LeafNode(objective);
		_nodes.Add(leaf);
		return this;
	}

	#endregion

	#region Query

	/// <summary>
	/// Finds the first node that is not structurally completed.
	/// </summary>
	public WorldObjectiveNodeBase FindCurrentNode()
	{
		foreach (var node in _nodes)
		{
			if (!node.Completed)
			{
				return node;
			}
		}
		return null;
	}

	/// <summary>
	/// Returns all active objectives of the current node.
	/// </summary>
	public IEnumerable<WorldObjectiveBase> FindCurrentObjectives()
	{
		var cur = FindCurrentNode();
		if (cur != null && !cur.Completed)
		{
			return cur.FindAllEntrances();
		}

		return [];
	}

	#endregion

	#region Lifecycle

	/// <summary>
	/// Activates the current node and fires activation events.
	/// </summary>
	public void Activate()
	{
		Current = FindCurrentNode();
		OnObjectiveActivated.Invoke(Current);
	}

	/// <summary>
	/// Deactivates the current node and fires deactivation events.
	/// </summary>
	public void Deactivate()
	{
		OnObjectiveDeactivated.Invoke(Current);
	}

	/// <summary>
	/// Forces a transition to the next active node.
	/// </summary>
	private void CheckActiveNode()
	{
		OnObjectiveDeactivated.Invoke(Current);
		Current = FindCurrentNode();
		OnObjectiveActivated.Invoke(Current);
	}

	/// <summary>
	/// Applies a snapshot-based lifecycle correction without executing completion logic.
	/// </summary>
	public void ApplyObjectiveSnapshot(bool activeBefore, bool activeAfter)
	{
		if (Current == FindCurrentNode())
		{
			if (activeBefore == activeAfter)
			{
				return;
			}

			if (!activeBefore && activeAfter)
			{
				OnObjectiveActivated.Invoke(Current);
			}
			else if (activeBefore && !activeAfter)
			{
				OnObjectiveDeactivated.Invoke(Current);
			}
		}
		else
		{
			var oldObjective = Current;
			Current = FindCurrentNode();
			OnObjectiveSynced.Invoke(Current);

			if (activeBefore && activeAfter)
			{
				OnObjectiveDeactivated.Invoke(oldObjective);
				OnObjectiveActivated.Invoke(Current);
			}
			else if (!activeBefore && activeAfter)
			{
				OnObjectiveActivated.Invoke(Current);
			}
			else if (activeBefore && !activeAfter)
			{
				OnObjectiveDeactivated.Invoke(oldObjective);
			}
		}
	}

	/// <summary>
	/// Updates the active node and checks for completion conditions.
	/// </summary>
	public void UpdateNode()
	{
		if (Current.Completed)
		{
			CheckActiveNode();
			return;
		}

		Current.Update();

		if (Current.CheckCompletion())
		{
			CompleteNode();
		}
	}

	/// <summary>
	/// Completes the current node and fires completion events.
	/// </summary>
	public void CompleteNode()
	{
		if (CompleteNodeCore())
		{
			OnNodeCompleted.Invoke(Current);
		}
	}

	/// <summary>
	/// Core structural completion logic.
	/// </summary>
	private bool CompleteNodeCore()
	{
		if (Current is null)
		{
			return false;
		}

		Current.Complete();
		OnObjectiveDeactivated.Invoke(Current);

		Current = FindCurrentNode();
		OnObjectiveActivated.Invoke(Current);

		return true;
	}

	/// <summary>
	/// Resets all nodes and restores the first node as active.
	/// </summary>
	public void ResetProgress()
	{
		Current = AllNodes[0];
		foreach (var node in AllNodes)
		{
			node.ResetProgress();
		}
	}

	#endregion

	#region Persistence

	private const string ObjectivesSaveKey = "Objectives";

	/// <summary>
	/// Loads objective data for each node.
	/// </summary>
	public void LoadData(TagCompound tag)
	{
		if (tag.TryGet<IList<TagCompound>>(ObjectivesSaveKey, out var oTags))
		{
			for (int i = 0; i < oTags.Count && i < AllNodes.Count; i++)
			{
				AllNodes[i].LoadData(oTags[i]);
			}
		}
	}

	/// <summary>
	/// Saves objective data for each node.
	/// </summary>
	public void SaveData(TagCompound tag)
	{
		var oTags = new List<TagCompound>();
		foreach (var o in AllNodes)
		{
			var ot = new TagCompound();
			o.SaveData(ot);
			oTags.Add(ot);
		}
		tag.Add(ObjectivesSaveKey, oTags);
	}

	#endregion

	#region Netcode

	/// <summary>
	/// Sends netcode data for all nodes.
	/// </summary>
	public void NetSend(BinaryWriter bw)
	{
		foreach (var node in AllNodes)
		{
			node.NetSend(bw);
		}
	}

	/// <summary>
	/// Receives netcode data for all nodes.
	/// </summary>
	public void NetReceive(BinaryReader br)
	{
		foreach (var node in AllNodes)
		{
			node.NetReceive(br);
		}
	}

	/// <summary>
	/// Triggers delta-sync events for all active objectives.
	/// </summary>
	public void OnMPSync()
	{
		foreach (var current in FindCurrentObjectives())
		{
			if (current is IDeltaSyncObjective deltaSync && deltaSync.NeedDeltaSync)
			{
				OnMPSyncTriggered.Invoke(deltaSync);
			}
		}
	}

	#endregion
}

#region Nodes

/// <summary>
/// Base class for structural objective nodes.
/// </summary>
public abstract class WorldObjectiveNodeBase
{
	/// <summary>
	/// Whether this node has been structurally completed.
	/// </summary>
	public abstract bool Completed { get; }

	/// <summary>
	/// Progress value of this node (0–1).
	/// </summary>
	public abstract float Progress { get; }

	/// <summary>
	/// Returns all active objectives belonging to this node.
	/// </summary>
	public abstract List<WorldObjectiveBase> FindAllEntrances();

	/// <summary>
	/// Per-frame update logic.
	/// </summary>
	public abstract void Update();

	/// <summary>
	/// Checks whether this node's completion conditions are met.
	/// </summary>
	public abstract bool CheckCompletion();

	/// <summary>
	/// Marks this node as structurally completed.
	/// </summary>
	public abstract void Complete();

	/// <summary>
	/// Resets this node's progress and completion state.
	/// </summary>
	public abstract void ResetProgress();

	/// <summary>
	/// Saves node-specific data.
	/// </summary>
	public abstract void SaveData(TagCompound tag);

	/// <summary>
	/// Loads node-specific data.
	/// </summary>
	public abstract void LoadData(TagCompound tag);

	/// <summary>
	/// Sends node-specific netcode data.
	/// </summary>
	public abstract void NetSend(BinaryWriter bw);

	/// <summary>
	/// Receives node-specific netcode data.
	/// </summary>
	public abstract void NetReceive(BinaryReader br);
}

/// <summary>
/// Leaf node wrapping a single objective.
/// </summary>
public class LeafNode : WorldObjectiveNodeBase
{
	public WorldObjectiveBase Objective;

	public LeafNode(WorldObjectiveBase obj)
	{
		Objective = obj;
	}

	public override bool Completed => Objective.Completed;

	public override float Progress => Objective.Progress;

	public override List<WorldObjectiveBase> FindAllEntrances() =>
		Objective.Completed ? [] : [Objective];

	public override void Update() => Objective.Update();

	public override bool CheckCompletion() => Objective.CheckCompletion();

	public override void Complete() => Objective.Complete();

	public override void ResetProgress() => Objective.ResetProgress();

	public override void SaveData(TagCompound tag) => Objective.SaveData(tag);

	public override void LoadData(TagCompound tag) => Objective.LoadData(tag);

	public override void NetSend(BinaryWriter bw) => Objective.NetSend(bw);

	public override void NetReceive(BinaryReader br) => Objective.NetReceive(br);
}

#endregion