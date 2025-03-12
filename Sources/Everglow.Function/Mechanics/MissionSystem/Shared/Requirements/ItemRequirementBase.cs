using Everglow.Commons.Mechanics.MissionSystem.Abstracts;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;

public abstract class ItemRequirementBase
{
	protected ItemRequirementBase()
	{
	}

	protected ItemRequirementBase(List<int> items, int requirement)
	{
		Items = items;
		Requirement = requirement;
	}

	/// <summary>
	/// Item types
	/// </summary>
	public abstract List<int> Items { get; init; }

	/// <summary>
	/// Gain count requirement
	/// </summary>
	public abstract int Requirement { get; init; }
}