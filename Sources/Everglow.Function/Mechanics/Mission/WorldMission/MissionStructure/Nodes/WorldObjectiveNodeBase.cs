using Everglow.Commons.Mechanics.Mission.WorldMission.Abstractions;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.MissionStructure.Nodes;

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