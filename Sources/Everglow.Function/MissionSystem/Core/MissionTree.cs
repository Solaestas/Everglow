namespace Everglow.Commons.MissionSystem.Core;

public abstract class MissionTree
{
	private MissionTreeNode Root { get; set; }

	public abstract void Initialize();

	public MissionTreeNode GetFirstNode()
	{
		if (Root == null)
		{
			Initialize();
			if (Root == null)
			{
				throw new InvalidProgramException();
			}
		}

		return Root;
	}
}
