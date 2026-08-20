using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;

public abstract partial class PlayerMissionBase : ITagCompoundEntity
{
	/// <summary>
	/// 任务图标
	/// <br>!为null时不显示</br>
	/// </summary>
	public virtual MissionIconGroup Icon => GetIcons(new());

	public virtual MissionIconGroup GetIcons(MissionIconGroup iconGroup)
	{
		iconGroup.Add(MissionSourceIcon.Create(Source, SubSource));
		Objectives.GetObjectivesIcon(iconGroup);

		return iconGroup;
	}

	/// <summary>
	/// 获取任务目标文本
	/// </summary>
	/// <returns></returns>
	public virtual IEnumerable<string> GetObjectives()
	{
		var mainIndex = 1;
		var lines = new List<string>();
		foreach (var (completed, objectiveLines) in Objectives.GetObjectivesText())
		{
			int subIndex = 1;
			for (int i = 0; i < objectiveLines.Count; i++)
			{
				if (completed)
				{
					objectiveLines[i] = $"[TextDrawer,Text='(已完成)',Color='100,100,100,255']" + " " + objectiveLines[i];
				}

				objectiveLines[i] = $"{mainIndex}.{subIndex++} " + objectiveLines[i];
			}

			lines.AddRange(objectiveLines);
			mainIndex++;
		}

		return lines;
	}

	/// <summary>
	/// 获取奖励文本
	/// </summary>
	/// <returns></returns>
	public virtual string GetRewards() => string.Join(' ', RewardItems.ConvertAll(i => ItemDrawer.Create(i.type, i.stack, new Color(196, 241, 255))));
}
