using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements;

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
		_missionList.SetVerticalScrollbar(_missionScrollbar);
		Register(_missionScrollbar);

		_missionList.Info.Width.SetValue(_missionScrollbar.Info.Left);
	}

	public override void Calculation()
	{
		base.Calculation();
		_missionScrollbar.Info.Left.SetValue(PositionStyle.Full - _missionScrollbar.Info.Width - (64f, 0f));
		_missionScrollbar.Info.Top.SetValue(8, 0);
		_missionScrollbar.Info.Width.SetValue(33);
		_missionScrollbar.Info.Height.SetValue(PositionStyle.Full - (8, 0));
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);

		if (_missionList is not null)
		{
			var hideList = MissionContainer.Filter.SpectrumBlockedAtInner || MissionContainer.Filter.SpectrumBlockedAtOuter;
			_missionList.Info.IsHidden = hideList;
			_missionList.Info.IsVisible = !hideList;
			PositionStyle top = (6 * MissionContainer.Scale, 0f);
			foreach (var mI in _missionList.Elements)
			{
				mI.OnInitialization();
				mI.Info.Top.SetValue(top);

				top += mI.Info.Height;
			}
		}
	}

	public override void Draw(SpriteBatch sb)
	{
		Texture2D background = Commons.ModAsset.Drop.Value;
		var newHitBox = HitBox;
		newHitBox.Y += 8;
		newHitBox.Height -= 8;
		newHitBox.X += 24;
		newHitBox.Width -= 120;
		sb.Draw(background, newHitBox, new Rectangle(0, 0, 1, 1), Color.White * 0.5f);
		base.Draw(sb);
	}

	/// <summary>
	/// 刷新任务列表
	/// </summary>
	public void RefreshList(MissionViewState? poolType, MissionType? missionType, MissionSourceBase missionSource)
	{
		// 筛选任务状态，获得初始列表
		IEnumerable<MissionPresentationEntry> missions = MissionContainer.Service.GetAll()
			.Where(entry => entry.View.Identity.Side == MissionSide.Player);
		if (poolType.HasValue)
		{
			missions = missions.Where(entry => entry.View.State == poolType.Value);
		}

		// 筛选来源NPC
		if (missionSource is not null) // NPC模式，去掉非对应NPC的任务
		{
			missions = missions.Where(entry => entry.View.Source == missionSource || entry.View.SubSource == missionSource);
		}
		else // 全局模式，去掉有来源NPC的未接取任务
		{
			missions = missions.Where(entry => !(entry.View.State is MissionViewState.Available && entry.View.Source is not null && entry.View.Source != MissionSourceBase.Default));
		}

		// 筛选任务类型
		if (missionType.HasValue)
		{
			missions = missions.Where(entry => entry.View.Type == missionType);
		}

		// 排序
		missions = missions.OrderBy(entry => entry.View, PlayerMissionComparer.Instance);

		// 生成任务UI元素
		List<BaseElement> elements = [];
		float ElementSpacing = 10 * MissionContainer.Scale;
		PositionStyle top = (4 * MissionContainer.Scale, 0f);
		foreach (MissionPresentationEntry entry in missions)
		{
			if (!entry.View.Visible)
			{
				continue;
			}

			var element = new UIMissionItem(entry);
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
