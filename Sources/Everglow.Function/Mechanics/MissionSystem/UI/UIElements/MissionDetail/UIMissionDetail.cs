using System.Text;
using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.Mechanics.MissionSystem.UI.MissionContainer;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.MissionDetail;

public class UIMissionDetail : UIBlock
{
	private UIMissionIcon _icon;
	private UIBlock _tree;
	private UIImage _treeIcon;
	private UIBlock _timer;
	private UIImage _timerIcon;

	private UIBlock _description;
	private UIContainerPanel _descriptionContainer;
	private UITextVerticalScrollbar _descriptionTextScrollbar;

	private UIBlock _objective;
	private UIContainerPanel _objectiveContainer;
	private UITextVerticalScrollbar _objectiveTextScrollbar;

	private UIBlock _reward;
	private UIContainerPanel _rewardContainer;
	private UITextVerticalScrollbar _rewardTextScrollbar;

	private UIBlock _changeMission;
	private UITextPlus _changeText;
	private UITextPlus _yes;
	private UITextPlus _no;

	private UIMissionDetailMask _mask;

	public UIMissionItem SelectedItem => Instance.SelectedItem;

	public class ChangeButtonText
	{
		public const string Failed = "失败";
		public const string Overdue = "过期";
		public const string Completed = "完成";
		public const string Cancel = "放弃";
		public const string Commit = "提交";
		public const string Accept = "接取";
		public const string Unknown = "未知";
		public const string Yes = "是";
		public const string No = "否";
	}

	internal void SetMask(UIMissionDetailMask mask)
	{
		_mask = mask;
	}

	public override void OnInitialization()
	{
		var scale = Scale;

		base.OnInitialization();

		// Headshot
		_icon = new UIMissionIcon(null);
		_icon.Info.Width.SetValue(420 * scale);
		_icon.Info.Height.SetValue(256 * scale);
		_icon.Info.Left.SetValue((Info.Width - _icon.Info.Width) / 2);
		_icon.Info.Top.SetValue(18 * scale);
		Register(_icon);

		// Tree
		_tree = new UIBlock();
		_tree.Info.Width.SetValue(81 * scale);
		_tree.Info.Height.SetValue(162 * scale);
		_tree.Info.Left.SetValue(36 * scale);
		_tree.Info.Top.SetValue(60 * scale);
		_tree.Info.SetMargin(0);
		_tree.PanelColor = Color.Transparent;
		_tree.BorderWidth = 0;
		_tree.Info.IsSensitive = true;
		_tree.Events.OnMouseHover += e => Instance.MouseText = "Mission Tree";
		_tree.Events.OnLeftClick += e =>
		{
			_mask.Show<UIMissionTree>(SelectedItem?.Mission);
		};
		Register(_tree);

		_treeIcon = new UIImage(ModAsset.ToMissionTreeSurface.Value, Color.White);
		_treeIcon.Info.Width = _tree.Info.Width;
		_treeIcon.Info.Height = _tree.Info.Height;
		_treeIcon.Events.OnMouseHover += e => _treeIcon.Color = new Color(1f, 1f, 1f, 0f);
		_treeIcon.Events.OnMouseOut += e => _treeIcon.Color = Color.White;
		_tree.Register(_treeIcon);

		// Timer
		_timer = new UIBlock();
		_timer.Info.Width.SetValue(73 * scale);
		_timer.Info.Height.SetValue(71 * scale);
		_timer.Info.Left.SetValue(600 * scale);
		_timer.Info.Top.SetValue(110 * scale);
		_timer.Info.SetMargin(0);
		_timer.PanelColor = Color.Transparent;
		_timer.BorderWidth = 0;
		_timer.Info.IsSensitive = true;
		_timer.Events.OnMouseHover += e => Instance.MouseText = "Mission Timer";
		_timer.Events.OnLeftClick += e =>
		{
			_mask.Show<UIMissionObjectiveTimer>(SelectedItem?.Mission);
		};
		Register(_timer);

		_timerIcon = new UIImage(ModAsset.ToClockSurface.Value, Color.White);
		_timerIcon.Info.Width = _timer.Info.Width;
		_timerIcon.Info.Height = _timer.Info.Height;
		_timerIcon.Events.OnMouseHover += e => _timerIcon.Color = new Color(1f, 1f, 1f, 0f);
		_timerIcon.Events.OnMouseOut += e => _timerIcon.Color = Color.White;
		_timer.Register(_timerIcon);

		var infoColor = new Color(0.2f, 0.2f, 0.2f, 0.005f);

		// Description
		_description = new UIBlock();
		_description.Info.Width.SetValue(210 * scale);
		_description.Info.Height.SetValue(384 * scale);
		_description.Info.Left.SetValue(20 * scale);
		_description.Info.Top.SetValue(314 * scale);
		_description.PanelColor = infoColor;
		_description.BorderColor = Color.Gray;
		Register(_description);

		_descriptionTextScrollbar = new UITextVerticalScrollbar();
		_descriptionTextScrollbar.Info.Height.SetValue(-16f, 1f);
		_descriptionTextScrollbar.Info.SetToCenter();
		_descriptionTextScrollbar.Info.Left.SetValue(-8f, 1f);
		_description.Register(_descriptionTextScrollbar);

		_descriptionContainer = new UIContainerPanel();
		_descriptionContainer.Info.Width.SetValue(PositionStyle.Full - _descriptionTextScrollbar.Info.Width - (PositionStyle.Full - _descriptionTextScrollbar.Info.Left - _descriptionTextScrollbar.Info.Width) * 3f);
		_descriptionContainer.Info.Height.SetValue(_descriptionTextScrollbar.Info.Height);
		_descriptionContainer.Info.Left.SetValue(PositionStyle.Full - _descriptionTextScrollbar.Info.Left - _descriptionTextScrollbar.Info.Width);
		_descriptionContainer.Info.Top.SetValue(_descriptionTextScrollbar.Info.Top);
		_descriptionContainer.SetVerticalScrollbar(_descriptionTextScrollbar);
		_description.Register(_descriptionContainer);

		// Objective
		_objective = new UIBlock();
		_objective = new UIBlock();
		_objective.Info.Width.SetValue(210 * scale);
		_objective.Info.Height.SetValue(384 * scale);
		_objective.Info.Left.SetValue((20 + 210 + 20) * scale);
		_objective.Info.Top.SetValue(_description.Info.Top);
		_objective.PanelColor = infoColor;
		_objective.BorderColor = Color.Gray;
		Register(_objective);

		_objectiveTextScrollbar = new UITextVerticalScrollbar();
		_objectiveTextScrollbar.Info.Height.SetValue(-16f, 1f);
		_objectiveTextScrollbar.Info.SetToCenter();
		_objectiveTextScrollbar.Info.Left.SetValue(-8f, 1f);
		_objective.Register(_objectiveTextScrollbar);

		_objectiveContainer = new UIContainerPanel();
		_objectiveContainer.Info.Width.SetValue(PositionStyle.Full - _descriptionTextScrollbar.Info.Width - (PositionStyle.Full - _descriptionTextScrollbar.Info.Left - _descriptionTextScrollbar.Info.Width) * 3f);
		_objectiveContainer.Info.Height.SetValue(_descriptionTextScrollbar.Info.Height);
		_objectiveContainer.Info.Left.SetValue(PositionStyle.Full - _descriptionTextScrollbar.Info.Left - _descriptionTextScrollbar.Info.Width);
		_objectiveContainer.Info.Top.SetValue(_descriptionTextScrollbar.Info.Top);
		_objectiveContainer.SetVerticalScrollbar(_objectiveTextScrollbar);
		_objective.Register(_objectiveContainer);

		// Reward
		_reward = new UIBlock();
		_reward = new UIBlock();
		_reward.Info.Width.SetValue(210 * scale);
		_reward.Info.Height.SetValue(284 * scale);
		_reward.Info.Left.SetValue((20 + 210 + 20 + 210 + 20) * scale);
		_reward.Info.Top.SetValue(_description.Info.Top);
		_reward.PanelColor = infoColor;
		_reward.BorderColor = Color.Gray;
		Register(_reward);

		_rewardTextScrollbar = new UITextVerticalScrollbar();
		_rewardTextScrollbar.Info.Height.SetValue(-16f, 1f);
		_rewardTextScrollbar.Info.SetToCenter();
		_rewardTextScrollbar.Info.Left.SetValue(-8f, 1f);
		_reward.Register(_rewardTextScrollbar);

		_rewardContainer = new UIContainerPanel();
		_rewardContainer.Info.Width.SetValue(PositionStyle.Full - _descriptionTextScrollbar.Info.Width - (PositionStyle.Full - _descriptionTextScrollbar.Info.Left - _descriptionTextScrollbar.Info.Width) * 3f);
		_rewardContainer.Info.Height.SetValue(_descriptionTextScrollbar.Info.Height);
		_rewardContainer.Info.Left.SetValue(PositionStyle.Full - _descriptionTextScrollbar.Info.Left - _descriptionTextScrollbar.Info.Width);
		_rewardContainer.Info.Top.SetValue(_descriptionTextScrollbar.Info.Top);
		_rewardContainer.SetVerticalScrollbar(_rewardTextScrollbar);
		_reward.Register(_rewardContainer);

		// Button
		_changeMission = new UIBlock();
		_changeMission.Info.Width.SetValue(80 * scale);
		_changeMission.Info.Height.SetValue(40 * scale);
		_changeMission.Info.Left.SetValue((20 + 210 + 20 + 210 + 20 + 105 - 40) * scale);
		_changeMission.Info.Top.SetValue((314 + 284 + 30) * scale);
		_changeMission.Info.IsSensitive = true;
		_changeMission.PanelColor = new Color(0.2f, 0.2f, 0.2f, 0.005f);
		_changeMission.Events.OnLeftDown += OnClickChange;
		Register(_changeMission);

		_yes = new UITextPlus(ChangeButtonText.Yes);
		_yes.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
		_yes.StringDrawer.Init(_yes.Text);
		_yes.Info.IsVisible = false;
		_yes.Events.OnUpdate += (e, gt) =>
		{
			_yes.Info.SetToCenter();
			_yes.Info.Left.Pixel += 44f;
			_yes.Calculation();
		};
		_yes.Events.OnLeftDown += OnClickYes;
		_changeMission.Register(_yes);

		_no = new UITextPlus(ChangeButtonText.No);
		_no.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
		_no.StringDrawer.Init(_no.Text);
		_no.Info.IsVisible = false;
		_no.Events.OnUpdate += (e, gt) =>
		{
			_no.Info.SetToCenter();
			_no.Info.Left.Pixel -= 44f;
			_no.Calculation();
		};
		_no.Events.OnLeftDown += OnClickNo;
		_changeMission.Register(_no);

		_changeText = new UITextPlus(string.Empty);
		_changeText.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
		_changeText.StringDrawer.Init(_changeText.Text);
		_changeMission.Register(_changeText);
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);
	}

	public void HideMissionDetailMask() => _mask.Info.IsVisible = false;

	public void ResetMissionDetail()
	{
		HideMissionDetailMask();

		_icon.SetIconGroup(null);
		_descriptionTextScrollbar.WheelValue = 0f;
		_descriptionContainer.ClearAllElements();

		_objectiveTextScrollbar.WheelValue = 0f;
		_objectiveContainer.ClearAllElements();

		_rewardTextScrollbar.WheelValue = 0f;
		_rewardContainer.ClearAllElements();
	}

	public void SetMissionDetail(UIMissionItem missionItem)
	{
		ResetMissionDetail();

		if (missionItem != null)
		{
			HideMissionDetailMask();

			MissionBase mission = missionItem.Mission;
			_icon.SetIconGroup(mission.Icon);
			_descriptionTextScrollbar.WheelValue = 0f;

			var desText = new StringBuilder();

			// Time limit
			if (mission.TimeMax > 0)
			{
				desText.Append(mission.GetTime() + "\n");
			}

			// Description
			desText.Append("描述：\n");
			if (string.IsNullOrWhiteSpace(mission.Description))
			{
				desText.Append("无\n");
			}
			else
			{
				desText.Append(mission.Description + "\n");
			}
			var des = new UITextPlus(desText.ToString());
			des.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
			des.StringDrawer.Init(des.Text);
			_descriptionContainer.AddElement(des);
			des.StringDrawer.SetWordWrap(_descriptionContainer.HitBox.Width - _descriptionTextScrollbar.InnerScale.X);

			// Objectives
			var objText = new StringBuilder();
			objText.Append("目标：\n");
			foreach (var objective in mission.GetObjectives())
			{
				objText.Append(objective);
			}
			var obj = new UITextPlus(objText.ToString());
			obj.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
			obj.StringDrawer.Init(obj.Text);
			_objectiveContainer.AddElement(obj);
			obj.StringDrawer.SetWordWrap(_objectiveContainer.HitBox.Width - _objectiveTextScrollbar.InnerScale.X);

			// Rewards
			var rewText = new StringBuilder();
			rewText.Append("奖励：\n");
			rewText.Append(mission.GetRewards());
			var rew = new UITextPlus(rewText.ToString());
			rew.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
			rew.StringDrawer.Init(rew.Text);
			_rewardContainer.AddElement(rew);
			rew.StringDrawer.SetWordWrap(_rewardContainer.HitBox.Width - _rewardTextScrollbar.InnerScale.X);
		}
	}

	/// <summary>
	/// Base operations for mission
	/// </summary>
	/// <param name="e"></param>
	public void OnClickChange(BaseElement e)
	{
		if (SelectedItem == null)
		{
			return;
		}

		if (SelectedItem.Mission.PoolType == PoolType.Accepted) // Accepted missions
		{
			if (SelectedItem.Mission.CheckComplete()) // Completed
			{
				// Commit the mission
				SelectedItem.Mission.OnComplete();
				MissionManager.NeedRefresh = true;
			}
			else // Incompleted
			{
				// Discard the mission (Second confirmation)
				ShowYesNo();
			}
		}
		else if (SelectedItem.Mission.PoolType == PoolType.Available) // Available missions
		{
			// Accept the mission
			MissionManager.MoveMission(SelectedItem.Mission, PoolType.Available, PoolType.Accepted);
			MissionManager.NeedRefresh = true;
		}
	}

	public void OnClickYes(BaseElement e)
	{
		if (SelectedItem != null
			&& SelectedItem.Mission.PoolType == PoolType.Accepted
			&& !SelectedItem.Mission.CheckComplete())
		{
			MissionManager.MoveMission(SelectedItem.Mission, PoolType.Accepted, PoolType.Failed);
			MissionManager.NeedRefresh = true;
		}

		HideYesNo();
	}

	public void OnClickNo(BaseElement e) => HideYesNo();

	public void ShowYesNo()
	{
		_yes.Info.IsVisible = _no.Info.IsVisible = true;
	}

	public void HideYesNo()
	{
		_yes.Info.IsVisible = _no.Info.IsVisible = false;
	}

	/// <summary>
	/// 更新按钮的文字
	/// </summary>
	public void UpdateChangeButton()
	{
		if (SelectedItem != null)
		{
			if (SelectedItem.Mission.PoolType == PoolType.Available)
			{
				_changeText.Text = ChangeButtonText.Accept;
			}
			else if (SelectedItem.Mission.PoolType == PoolType.Accepted)
			{
				if (SelectedItem.Mission.CheckComplete())
				{
					_changeText.Text = ChangeButtonText.Commit;
				}
				else
				{
					_changeText.Text = ChangeButtonText.Cancel;
				}
			}
			else if (SelectedItem.Mission.PoolType == PoolType.Completed)
			{
				_changeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Completed}',Color='126,126,126']";
			}
			else if (SelectedItem.Mission.PoolType == PoolType.Overdue)
			{
				_changeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Overdue}',Color='126,126,126']";
			}
			else if (SelectedItem.Mission.PoolType == PoolType.Failed)
			{
				_changeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Failed}',Color='126,126,126']";
			}
			else
			{
				_changeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Unknown}',Color='126,126,126']";
			}

			_yes.Info.IsVisible = _no.Info.IsVisible = false;

			_changeText.Calculation();
			_changeText.Info.SetToCenter();
		}
		else
		{
			_changeText.Text = "[TextDrawer,Text='',Color='126,126,126']";
			_yes.Info.IsVisible = _no.Info.IsVisible = false;
		}
	}
}