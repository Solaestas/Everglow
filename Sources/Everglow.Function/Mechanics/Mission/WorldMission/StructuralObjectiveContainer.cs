using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission;

/// <summary>
/// Minimal version
/// </summary>
public class StructuralObjectiveContainer
{
	public event Action<WorldObjectiveNodeBase> OnNodeCompleted;

	public event Action<WorldObjectiveNodeBase> OnObjectiveActivated;

	public event Action<WorldObjectiveNodeBase> OnObjectiveDectivated;

	public event Action<IDeltaSyncObjective> OnMPSyncTriggerred;

	public event Action<WorldObjectiveNodeBase> OnObjectiveSynced;

	private readonly List<WorldObjectiveNodeBase> _nodes = [];
	private readonly List<WorldObjectiveBase> _objectives = [];

	public IReadOnlyList<WorldObjectiveNodeBase> AllNodes => _nodes;

	public IReadOnlyList<WorldObjectiveBase> AllObjectives => _objectives;

	public WorldObjectiveNodeBase Current { get; private set; }

	public bool Completed => FindCurrentNode() == null;

	#region DSL

	/// <summary>添加一个线性目标。</summary>
	public StructuralObjectiveContainer Add(WorldObjectiveBase objective)
	{
		objective.ObjectiveID = _objectives.Count;
		_objectives.Add(objective);

		var leaf = new LeafNode(objective);
		_nodes.Add(leaf);
		return this;
	}

	#endregion

	#region 查询

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

	public IEnumerable<WorldObjectiveBase> FindOpenEntrances()
	{
		var cur = FindCurrentNode();
		if (cur != null && !cur.Completed)
		{
			return cur.FindAllEntrances();
		}

		return [];
	}

	#endregion

	#region 生命周期

	public void Activate()
	{
		Current = FindCurrentNode();
		OnObjectiveActivated.Invoke(Current);
	}

	public void Deactivate()
	{
		OnObjectiveDectivated.Invoke(Current);
	}

	public void CheckActiveNode()
	{
		OnObjectiveDectivated.Invoke(Current);
		Current = FindCurrentNode();
		OnObjectiveActivated.Invoke(Current);
	}

	public void HandleObjectiveLifecycle(bool activeBefore, bool activeAfter)
	{
		if (Current == FindCurrentNode())
		{
			// If current is correct, change its state
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
				OnObjectiveDectivated.Invoke(Current);
			}
		}
		else
		{
			// If
			var oldObjective = Current;
			Current = FindCurrentNode();
			OnObjectiveSynced.Invoke(Current);
			if (activeBefore && activeAfter)
			{
				OnObjectiveDectivated.Invoke(oldObjective);
				OnObjectiveActivated.Invoke(Current);
			}
			else if (!activeBefore && activeAfter)
			{
				OnObjectiveActivated.Invoke(Current);
			}
			else if (activeBefore && !activeAfter)
			{
				OnObjectiveDectivated.Invoke(oldObjective);
			}
		}
	}

	public void UpdateCurrentNode()
	{
		// Pass the completed objective.
		// This block often happens when the mission just got loaded, or synced from server.
		if (Current.Completed)
		{
			CheckActiveNode();
			return;
		}

		Current.Update();

		// Check objective completion.
		if (Current.CheckCompletion())
		{
			CompleteNode();
		}
	}

	public void CompleteNode()
	{
		if (CompleteNodeCore())
		{
			OnNodeCompleted.Invoke(Current);
		}
	}

	private bool CompleteNodeCore()
	{
		if (Current is null)
		{
			return false;
		}

		Current.Complete();
		OnObjectiveDectivated.Invoke(Current);
		Current = FindCurrentNode();
		OnObjectiveActivated.Invoke(Current);

		return true;
	}

	public void ResetProgress()
	{
		Current = AllNodes[0];
		foreach (var node in AllNodes)
		{
			node.ResetProgress();
		}
	}

	#endregion

	#region 持久化

	private const string ObjectivesSaveKey = "Objectives";

	public void LoadData(TagCompound tag)
	{
		if (tag.TryGet<IList<TagCompound>>(ObjectivesSaveKey, out var oTags))
		{
			for (int i = 0; i < oTags.Count; i++)
			{
				if (i >= AllNodes.Count)
				{
					break;
				}

				var oTag = oTags[i];
				var node = AllNodes[i];

				node.LoadData(oTag);
			}
		}
	}

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

	public void NetSend(BinaryWriter bw)
	{
		foreach (var node in AllNodes)
		{
			node.NetSend(bw);
		}
	}

	public void NetReceive(BinaryReader br)
	{
		foreach (var node in AllNodes)
		{
			node.NetReceive(br);
		}
	}

	public void OnMPSync()
	{
		var currents = FindOpenEntrances();
		foreach (var current in currents)
		{
			if (current is not null and IDeltaSyncObjective deltaSync)
			{
				if (deltaSync.NeedDeltaSync)
				{
					OnMPSyncTriggerred.Invoke(deltaSync);
				}
			}
		}
	}

	#endregion
}

#region Nodes

public abstract class WorldObjectiveNodeBase
{
	public abstract bool Completed { get; }

	public abstract float Progress { get; }

	public abstract List<WorldObjectiveBase> FindAllEntrances();

	public abstract void Update();

	public abstract bool CheckCompletion();

	public abstract void Complete();

	public abstract void ResetProgress();

	public abstract void SaveData(TagCompound tag);

	public abstract void LoadData(TagCompound tag);

	public abstract void NetSend(BinaryWriter bw);

	public abstract void NetReceive(BinaryReader br);
}

public class LeafNode : WorldObjectiveNodeBase
{
	public WorldObjectiveBase Objective;

	public LeafNode(WorldObjectiveBase obj)
	{
		Objective = obj;
	}

	public override bool Completed => Objective.Completed;

	public override float Progress => Objective.Progress;

	public override List<WorldObjectiveBase> FindAllEntrances() => Objective.Completed ? [] : [Objective];

	public override void Update() => Objective.Update();

	public override bool CheckCompletion() => Objective.CheckCompletion();

	public override void Complete() => Objective.Complete();

	public override void ResetProgress() => Objective.ResetProgress();

	public override void SaveData(TagCompound tag)
	{
		Objective.SaveData(tag);
	}

	public override void LoadData(TagCompound tag)
	{
		Objective.LoadData(tag);
	}

	public override void NetSend(BinaryWriter bw)
	{
		Objective.NetSend(bw);
	}

	public override void NetReceive(BinaryReader br)
	{
		Objective.NetReceive(br);
	}
}

#endregion