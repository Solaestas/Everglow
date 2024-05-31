namespace Everglow.Commons.QuestSystem;

public class LinearQuest : Quest
{
	public List<Quest> ParentQuest;

	public List<Quest> ChildQuest { get; }

	public bool TryActivate()
	{
		foreach (var Quest in ParentQuest)
		{
			if (!Quest.IsFinished)
			{
				return false;
			}
		}
		Hide = false;
		return true;
	}

	public override void Settle()
	{
		SpawnRewards();
		IsFinished = true;
		Hide = true;
	}
}