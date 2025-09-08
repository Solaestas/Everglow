namespace Everglow.Commons.Mechanics.MissionSystem.Core;

public class MissionTreeNode
{
	public MissionTreeNode(MissionBase mission)
	{
		Mission = mission;
	}

	public MissionBase Mission { get; init; }

	public List<MissionTreeNode> Children { get; } = [];

	public void AddChild(MissionTreeNode node)
	{
		Children.Add(node);
	}
}