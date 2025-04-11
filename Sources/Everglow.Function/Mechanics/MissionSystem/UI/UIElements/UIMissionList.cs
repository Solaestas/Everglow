using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionList : UIBlock
{
	private UIContainerPanel _missionList;
	private UIMissionListScrollbar _missionScrollbar;

	public List<UIMissionItem> MissionItems => _missionList.Elements.ConvertAll(x => x as UIMissionItem);

	public override void OnInitialization()
	{
		Info.SetMargin(0);

		// Mission list
		_missionList = new UIContainerPanel();
		_missionList.Events.OnUpdate += (e, gt) => MissionContainer.Background?.SetChainValue(_missionList.VerticalScrollDistance);
		Register(_missionList);

		// Mission list scrollbar
		_missionScrollbar = new UIMissionListScrollbar();
		_missionScrollbar.Info.Left.SetValue(PositionStyle.Full - _missionScrollbar.Info.Width - (4f, 0f));
		_missionScrollbar.Info.Height.SetValue(PositionStyle.Full - (20, 0f));
		_missionList.SetVerticalScrollbar(_missionScrollbar);
		Register(_missionScrollbar);

		_missionList.Info.Width.SetValue(_missionScrollbar.Info.Left);
	}

	/// <summary>
	/// 刷新任务列表
	/// </summary>
	public void RefreshList(PoolType? poolType, MissionType? missionType, MissionSourceBase missionSource)
	{
		// 筛选任务状态，获得初始列表
		var missions = poolType.HasValue
			? MissionManager.GetMissionPool(poolType.Value)
			: Enum.GetValues<PoolType>().Select(MissionManager.GetMissionPool).SelectMany(x => x);

		// 筛选来源NPC
		if (missionSource is not null) // NPC模式，去掉非对应NPC的任务
		{
			missions = missions.Where(m => m.Source == missionSource || m.SubSource == missionSource);
		}
		else // 全局模式，去掉有来源NPC的未接取任务
		{
			missions = missions.Where(m => !(m.PoolType is PoolType.Available && m.Source is not null && m.Source != MissionSourceBase.Default));
		}

		// 筛选任务类型
		if (missionType.HasValue)
		{
			missions = missions.Where(m => m.MissionType == missionType);
		}

		// 排序
		missions = missions.Order(MissionComparer.Instance);

		// 生成任务UI元素
		List<BaseElement> elements = [];
		float ElementSpacing = 10 * MissionContainer.Scale;
		PositionStyle top = (4 * MissionContainer.Scale, 0f);
		foreach (var m in missions.ToList())
		{
			if (!m.IsVisible)
			{
				continue;
			}

			var element = (BaseElement)Activator.CreateInstance(m.BindingUIItem, [m]);
			element.OnInitialization();
			element.Info.Top.SetValue(top);
			element.Events.OnLeftDown += e =>
			{
				if (MissionContainer.Instance.SelectedItem != e)
				{
					MissionContainer.Instance.ChangeSelectedItem((UIMissionItem)e);
				}
			};

			elements.Add(element);

			top += element.Info.Height;
			top.Pixel += ElementSpacing;
		}

		_missionList.ClearAllElements();
		_missionList.AddElements(elements);
	}
}