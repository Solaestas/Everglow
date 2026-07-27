namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Core;

public class MissionTreeNode
{
	public MissionTreeNode(PlayerMissionBase mission)
	{
		Mission = mission;
	}

	public PlayerMissionBase Mission { get; init; }

	public List<MissionTreeNode> Children { get; } = [];

	public void AddChild(MissionTreeNode node)
	{
		Children.Add(node);
	}
}
