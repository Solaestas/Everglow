using System.Text;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Utilities;
using static Everglow.Commons.Mechanics.Mission.UI.MissionContainer;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail;

public class UIMissionDetail : UIBlock, IDrawable_InRt2D
{
	private static readonly Color ComponentColor = new Color(0.2f, 0.2f, 0.2f, 0.005f);
	private static readonly Color ChangeButtonHoverColor = Color.White;
	private static readonly Color MaskButtonColor = Color.White;
	private static readonly Color MaskButtonHoverColor = new Color(1f, 1f, 1f, 0f);

	private static UIMissionItem SelectedItem => Instance.SelectedItem;

	private static float FontSize => 30f * Instance.ResolutionFactor;

	private UIMissionIcon _icon;

	private UIMissionBlock _description;
	private UIContainerPanel _descriptionContainer;
	private UIMissionTextVerticalScrollbar _descriptionTextScrollbar;

	private UIMissionBlock _objective;
	private UIContainerPanel _objectiveContainer;
	private UIMissionTextVerticalScrollbar _objectiveTextScrollbar;
	private UITextPlus _objectiveText;
	private UIMissionHourglassTimer _objectiveTimer;

	private UIMissionDurationBar _objectiveDurationBar;

	private UIBlock _objectiveTree;
	private UIImage _objectiveTreeIcon;

	private UIMissionButton _objectiveChangeMission;
	private UITextPlus _objectiveChangeText;

	private UIRewardsStripe _rewardsPanel;

	// TODO: Add MissionStar to a mission(default 1);
	private UIMissionStarLevel _missionLevel;

	private float oldWidth;

	private float oldHeight;

	/// <summary>
	/// Link with <see cref="AnimationState"/>.
	/// </summary>
	public float AnimationTimer = 0;

	/// <summary>
	/// 0: None, 1:TryToQuit, 2:CompleteAndClear, 3:Fail
	/// </summary>
	public int AnimationState;

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

	public override void OnInitialization()
	{
		base.OnInitialization();

		// Headshot
		_icon = new UIMissionIcon(null);
		Register(_icon);

		// Stars
		_missionLevel = new UIMissionStarLevel();
		_missionLevel.Stars = 3;
		_missionLevel.Info.Width.SetValue(100);
		_missionLevel.Info.Height.SetValue(40);
		Register(_missionLevel);

		// Description
		_description = new UIMissionBlock();
		_description.PanelColor = ComponentColor;
		_description.BorderColor = Color.Gray;
		_description.MissionBlockStyle = 0;
		Register(_description);

		_descriptionTextScrollbar = new UIMissionTextVerticalScrollbar();
		_description.Register(_descriptionTextScrollbar);

		_descriptionContainer = new UIContainerPanel();
		_descriptionContainer.SetVerticalScrollbar(_descriptionTextScrollbar);
		_description.Register(_descriptionContainer);

		// Objective
		_objective = new UIMissionBlock();
		_objective.PanelColor = ComponentColor;
		_objective.BorderColor = Color.Gray;
		_objective.MissionBlockStyle = 1;
		Register(_objective);

		_objectiveTextScrollbar = new UIMissionTextVerticalScrollbar();
		_objective.Register(_objectiveTextScrollbar);

		_objectiveContainer = new UIContainerPanel();
		_objectiveContainer.SetVerticalScrollbar(_objectiveTextScrollbar);
		_objective.Register(_objectiveContainer);

		_objectiveTimer = new UIMissionHourglassTimer();
		_objectiveTimer.MaxTime = 120;
		_objectiveTimer.Events.OnMouseHover += e =>
		{
			Instance.MouseText = "Time Remain: " + (int)(_objectiveTimer.Timer / 60f) + "s";
			_objectiveTimer.OnSelect = true;
		};
		_objectiveTimer.Events.OnMouseOut += e =>
		{
			_objectiveTimer.OnSelect = false;
		};
		_objective.Register(_objectiveTimer);

		_objectiveDurationBar = new UIMissionDurationBar();
		_objectiveDurationBar.Events.OnMouseHover += e =>
		{
			Instance.MouseText = "Duration: " + (int)_objectiveDurationBar.CurrentDuration + "/" + (int)_objectiveDurationBar.MaxDuration;
			_objectiveDurationBar.OnSelect = true;
		};
		_objectiveDurationBar.Events.OnMouseOut += e =>
		{
			_objectiveDurationBar.OnSelect = false;
		};
		_objective.Register(_objectiveDurationBar);

		_objectiveTree = new UIBlock();
		_objectiveTree.Info.SetMargin(0);
		_objectiveTree.PanelColor = Color.Transparent;
		_objectiveTree.BorderWidth = 0;
		_objectiveTree.Info.IsSensitive = true;
		_objectiveTree.Events.OnMouseHover += e => Instance.MouseText = "Mission Tree";
		_objectiveTree.Events.OnLeftClick += e =>
		{
			DetailSub.Show<UIMissionTree>(SelectedItem?.View);
		};
		_objective.Register(_objectiveTree);

		_objectiveTreeIcon = new UIImage(ModAsset.ToMissionTreeSurface.Value, Color.White);
		_objectiveTreeIcon.SourceRectangle = new Rectangle(0, 0, 38, 85);
		_objectiveTreeIcon.Events.OnMouseHover += e =>
		{
			_objectiveTreeIcon.Color = MaskButtonHoverColor;
			_objectiveTreeIcon.SourceRectangle = new Rectangle(38, 0, 38, 85);
		};
		_objectiveTreeIcon.Events.OnMouseOut += e =>
		{
			_objectiveTreeIcon.Color = MaskButtonColor;
			_objectiveTreeIcon.SourceRectangle = new Rectangle(0, 0, 38, 85);
		};
		_objectiveTree.Register(_objectiveTreeIcon);

		// Button
		_objectiveChangeMission = new UIMissionButton();
		_objectiveChangeMission.Info.IsSensitive = true;
		_objectiveChangeMission.PanelColor = ChangeButtonHoverColor;
		_objectiveChangeMission.Events.OnLeftDown += OnClickChange;
		_objectiveChangeMission.Events.OnMouseHover += e =>
		{
			if (SelectedItem != null
				&& SelectedItem.View.State != MissionViewState.Overdue
				&& SelectedItem.View.State != MissionViewState.Failed)
			{
				_objectiveChangeMission.PanelColor = Color.White;
				_objectiveChangeMission.OnSelect = true;
				UpdateChangeButton("255,245,193");
			}
		};
		_objectiveChangeMission.Events.OnMouseOut += e =>
		{
			_objectiveChangeMission.PanelColor = Color.White;
			_objectiveChangeMission.OnSelect = false;
			UpdateChangeButton("45,38,33");
		};
		_objective.Register(_objectiveChangeMission);

		_objectiveChangeText = new UITextPlus(string.Empty);
		_objectiveChangeText.StringDrawer.DefaultParameters.SetParameter("FontSize", FontSize);
		_objectiveChangeText.StringDrawer.Init(_objectiveChangeText.Text);
		_objectiveChangeMission.Register(_objectiveChangeText);

		_rewardsPanel = new UIRewardsStripe();
		Register(_rewardsPanel);
	}

	public override void Calculation()
	{
		base.Calculation();
		Info.CanBeInteract = AnimationState == 0;
		Info.HiddenOverflow = true;

		float detailPanelWidth = (Info.Width.Pixel - 120) / 2f;
		float detailPanelDistance = 40;

		_icon.Info.Width.SetValue(480 * Scale);
		_icon.Info.Height.SetValue(256 * Scale);
		_icon.Info.Left.SetValue(detailPanelDistance + detailPanelWidth / 2f - 240);
		_icon.Info.Top.SetValue(93 * Scale);

		_missionLevel.Info.Width.SetValue(detailPanelWidth * Scale);
		_missionLevel.Info.Height.SetValue(40);
		_missionLevel.Info.Left.SetValue(detailPanelDistance * Scale);
		_missionLevel.Info.Top.SetValue(354);

		_description.Info.Width.SetValue(detailPanelWidth * Scale);
		_description.Info.Height.SetValue((ParentElement.Info.Height.Pixel - 560) * Scale);
		_description.Info.Left.SetValue(detailPanelDistance * Scale);
		_description.Info.Top.SetValue(400);

		_descriptionContainer.Info.Width.SetValue(PositionStyle.Full.Pixel - 54f);
		_descriptionContainer.Info.Height.SetValue(_descriptionTextScrollbar.Info.Height);
		_descriptionContainer.Info.Left.SetValue(PositionStyle.Full - _descriptionTextScrollbar.Info.Left - _descriptionTextScrollbar.Info.Width);
		_descriptionContainer.Info.Top.SetValue(_descriptionTextScrollbar.Info.Top);

		_descriptionTextScrollbar.Info.Height.SetValue(-50f, 1f);
		_descriptionTextScrollbar.Info.SetToCenter();
		_descriptionTextScrollbar.Info.Left.SetValue(-20f, 1f);

		_objective.Info.Width.SetValue(detailPanelWidth * Scale);
		_objective.Info.Height.SetValue((ParentElement.Info.Height.Pixel - 400) * Scale);
		_objective.Info.Left.SetValue((detailPanelDistance + detailPanelWidth + detailPanelDistance) * Scale);
		_objective.Info.Top.SetValue(60);

		_objectiveContainer.Info.Width.SetValue(PositionStyle.Full.Pixel - 54f);
		_objectiveContainer.Info.Height.SetValue(-200, 1f);
		_objectiveContainer.Info.Left.SetValue(30);
		_objectiveContainer.Info.Top.SetValue(_objectiveTextScrollbar.Info.Top);

		_objectiveTextScrollbar.Info.Height.SetValue(-50f, 1f);
		_objectiveTextScrollbar.Info.SetToCenter();
		_objectiveTextScrollbar.Info.Left.SetValue(-20f, 1f);

		_objectiveTimer.Info.Left.SetValue(17f);
		_objectiveTimer.Info.Top.SetValue(-148f, 1f);
		_objectiveTimer.Info.Width.SetValue(62);
		_objectiveTimer.Info.Height.SetValue(116);

		_objectiveTree.Info.Width.SetValue(38 * Scale);
		_objectiveTree.Info.Height.SetValue(85 * Scale);
		_objectiveTree.Info.Left.SetValue(100 * Scale);
		_objectiveTree.Info.Top.SetValue(-130f, 1f);

		_objectiveTreeIcon.Info.Width = _objectiveTree.Info.Width;
		_objectiveTreeIcon.Info.Height = _objectiveTree.Info.Height;

		float changeButtonWidth = (_objective.Info.HitBox.Width - 200) * Scale;
		_objectiveChangeMission.Info.Width.SetValue(changeButtonWidth);
		_objectiveChangeMission.Info.Height.SetValue(40 * Scale);
		_objectiveChangeMission.Info.Left.SetValue((-changeButtonWidth - 50) * Scale, 1);
		_objectiveChangeMission.Info.Top.SetValue(-70 * Scale, 1);

		_objectiveDurationBar.Info.Left.SetValue((-changeButtonWidth - 20) * Scale, 1);
		_objectiveDurationBar.Info.Top.SetValue(-120f, 1f);
		_objectiveDurationBar.Info.Width.SetValue(changeButtonWidth - 60);
		_objectiveDurationBar.Info.Height.SetValue(46);

		_rewardsPanel.Info.Width.SetValue(detailPanelWidth);
		_rewardsPanel.Info.Height.SetValue(256 * Scale);
		_rewardsPanel.Info.Left.SetValue((detailPanelDistance + detailPanelWidth + detailPanelDistance) * Scale);
		_rewardsPanel.Info.Top.SetValue(-240 * Scale, 1f);

		if (oldWidth != Info.Width.Pixel || oldHeight != Info.Height.Pixel)
		{
			if (SelectedItem != null)
			{
				ResetTexts();
				SetTexts(SelectedItem.View);
			}
		}

		oldWidth = Info.Width.Pixel;
		oldHeight = Info.Height.Pixel;
	}

	public static void HideMissionSubContent() => DetailSub.HideCurrent();

	public static void HideMissionTip() => DetailTip.HideCurrent();

	public void ResetMissionDetail()
	{
		HideMissionSubContent();
		HideMissionTip();

		_icon.SetIconGroup(null);
		_rewardsPanel.SetIconGroup(null);
		ResetTexts();
	}

	public void SetMissionDetail(UIMissionItem missionItem)
	{
		ResetMissionDetail();

		if (missionItem != null)
		{
			HideMissionSubContent();

			MissionView mission = missionItem.View;
			var iconGroup = new MissionIconGroup(mission.Icons);
			_icon.SetIconGroup(iconGroup);
			_rewardsPanel.SetIconGroup(iconGroup);
			_descriptionTextScrollbar.WheelValue = 0f;

			SetTexts(mission);
		}
	}

	public void SetTexts(MissionView mission)
	{
		var desText = new StringBuilder();

		// Time limit
		if (mission.TimeLimit.HasValue)
		{
			desText.Append($"[TimerIconDrawer,MissionName='{mission.Identity.DefinitionId}'] 剩余时间:[TimerStringDrawer,MissionName='{mission.Identity.DefinitionId}']\n\n");
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
		des.StringDrawer.DefaultParameters.SetParameter("FontSize", FontSize);
		des.StringDrawer.Init(des.Text);
		_descriptionContainer.AddElement(des);
		des.StringDrawer.SetWordWrap(_descriptionContainer.HitBox.Width - _descriptionTextScrollbar.InnerScale.X);

		SetObjectiveText(mission);
	}

	public void RefreshObjectives(MissionView mission) => SetObjectiveText(mission);

	private void SetObjectiveText(MissionView mission)
	{
		var objText = new StringBuilder();
		objText.Append("目标：\n");
		int mainIndex = 1;
		foreach (ObjectiveNodeView node in mission.ObjectiveNodes)
		{
			int subIndex = 1;
			bool completed = IsCompleted(node);
			foreach (string objective in GetObjectiveLines(node))
			{
				string text = completed
					? $"[TextDrawer,Text='(已完成)',Color='100,100,100,255'] {objective}"
					: objective;
				objText.Append($"{mainIndex}.{subIndex++} {text}");
				if (!text.EndsWith('\n'))
				{
					objText.Append('\n');
				}
			}
			mainIndex++;
		}

		if (_objectiveText is null)
		{
			_objectiveText = new UITextPlus(objText.ToString());
			_objectiveText.StringDrawer.DefaultParameters.SetParameter("FontSize", FontSize);
			_objectiveContainer.AddElement(_objectiveText);
		}
		else
		{
			_objectiveText.Text = objText.ToString();
		}

		_objectiveText.StringDrawer.Init(_objectiveText.Text);
		_objectiveText.StringDrawer.SetWordWrap(_objectiveContainer.HitBox.Width - _objectiveTextScrollbar.InnerScale.X);
		_objectiveText.Calculation();

	}

	private static IEnumerable<string> GetObjectiveLines(ObjectiveNodeView node)
	{
		return node switch
		{
			LeafObjectiveNodeView leaf => [leaf.Objective.Description],
			ParallelObjectiveNodeView parallel => parallel.Objectives.Select(objective => objective.Description),
			AnyOfObjectiveNodeView anyOf => anyOf.Objectives.Select(objective => objective.Description),
			BranchObjectiveNodeView branch => branch.Branches.SelectMany((branchView, branchIndex) =>
				branchView.Objectives.Select(objective =>
					$"[TextDrawer,Text='(Branch {branchIndex + 1})',Color='{GetBranchColor(branchView.State)}'] {objective.Description}")),
			_ => [],
		};
	}

	private static bool IsCompleted(ObjectiveNodeView node)
	{
		return node switch
		{
			LeafObjectiveNodeView leaf => leaf.Objective.State == ObjectiveViewState.Completed,
			ParallelObjectiveNodeView parallel => parallel.Objectives.All(objective => objective.State == ObjectiveViewState.Completed),
			AnyOfObjectiveNodeView anyOf => anyOf.Objectives.Any(objective => objective.State == ObjectiveViewState.Completed),
			BranchObjectiveNodeView branch => branch.Branches.Any(branchView =>
				branchView.State == ObjectiveBranchState.Selected
				&& branchView.Objectives.All(objective => objective.State == ObjectiveViewState.Completed)),
			_ => false,
		};
	}

	private static string GetBranchColor(ObjectiveBranchState state) => state switch
	{
		ObjectiveBranchState.Candidate => "100,180,120,255",
		ObjectiveBranchState.Selected => "100,255,100,255",
		ObjectiveBranchState.Skipped => "100,100,100,255",
		_ => "100,100,100,255",
	};

	private void ResetTexts()
	{
		_descriptionTextScrollbar.WheelValue = 0f;
		_descriptionContainer.ClearAllElements();

		_objectiveTextScrollbar.WheelValue = 0f;
		_objectiveContainer.ClearAllElements();
		_objectiveText = null;

		// _rewardTextScrollbar.WheelValue = 0f;
		// _rewardContainer.ClearAllElements();
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

		MissionAction? submit = FindAction(MissionActionType.Submit);
		if (submit.HasValue)
		{
			Service.TryExecute(submit.Value);
		}
		else if (SelectedItem.View.State == MissionViewState.Active)
		{
			MissionAction? cancel = FindAction(MissionActionType.Cancel);
			AnimationState = 1;
			UIMissionOperationTip tip;
			if (cancel.HasValue)
			{
				tip = new UIMissionOperationTip(SelectedItem.Entry, UIMissionOperationTip.TipType.Confirmation, "是否放弃任务", DiscardMission, "是", "否");
			}
			else
			{
				tip = new UIMissionOperationTip(SelectedItem.Entry, UIMissionOperationTip.TipType.Information, "该任务无法放弃", yesText: "确认");
			}
			tip.HideMask += ClearAnimation;
			DetailTip.Show(tip);
		}
		else
		{
			MissionAction? accept = FindAction(MissionActionType.Accept);
			if (accept.HasValue)
			{
				Service.TryExecute(accept.Value);
			}
		}
	}

	public void ClearAnimation(BaseElement _)
	{
		if (AnimationState == 1)
		{
			AnimationState = 0;
			AnimationTimer = 0;
		}
	}

	/// <summary>
	/// Fail the selected mission if it can be cancelled.
	/// </summary>
	/// <param name="entry"></param>
	public void DiscardMission(MissionPresentationEntry entry)
	{
		AnimationState = 0;
		MissionAction? cancel = FindAction(entry, MissionActionType.Cancel);
		if (cancel.HasValue)
		{
			Service.TryExecute(cancel.Value);
		}
	}

	private static MissionAction? FindAction(MissionActionType type) => FindAction(SelectedItem?.Entry, type);

	private static MissionAction? FindAction(MissionPresentationEntry entry, MissionActionType type)
	{
		if (entry is null)
		{
			return null;
		}

		foreach (MissionAction action in entry.Actions)
		{
			if (action.Type == type)
			{
				return action;
			}
		}

		return null;
	}

	/// <summary>
	/// 更新按钮的文字, color示例:"45,38,33"
	/// </summary>
	public void UpdateChangeButton(string color)
	{
		if (SelectedItem != null)
		{
			if (SelectedItem.View.State == MissionViewState.Available)
			{
				_objectiveChangeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Accept}',Color='{color}']";
			}
			else if (SelectedItem.View.State == MissionViewState.Active)
			{
				if (FindAction(MissionActionType.Submit).HasValue)
				{
					_objectiveChangeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Commit}',Color='{color}']";
				}
				else
				{
					_objectiveChangeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Cancel}',Color='{color}']";
				}
			}
			else if (SelectedItem.View.State == MissionViewState.Completed)
			{
				_objectiveChangeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Completed}',Color='{color}']";
			}
			else if (SelectedItem.View.State == MissionViewState.Overdue)
			{
				_objectiveChangeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Overdue}',Color='{color}']";
			}
			else if (SelectedItem.View.State == MissionViewState.Failed)
			{
				_objectiveChangeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Failed}',Color='{color}']";
			}
			else
			{
				_objectiveChangeText.Text = $"[TextDrawer,Text='{ChangeButtonText.Unknown}',Color='{color}']";
			}
			_objectiveChangeText.Calculation();
			_objectiveChangeText.Info.SetToCenter();
		}
		else
		{
			_objectiveChangeText.Text = "[TextDrawer,Text='',Color='{color}']";
		}
	}

	public override void Draw(SpriteBatch sb)
	{
		if (SelectedItem is null)
		{
			Texture2D tex = ModAsset.MissionIconBoard.Value;
			sb.Draw(tex, Info.TotalHitBox, new Rectangle(16, 16, 16, 16), Color.White);
		}
		else
		{
			var uiSystem = ModContent.GetInstance<UISystem>();

			// Visual effects for current mission detail interface(Submit, Fail, Quiting...)
			if (Ins.VisualQuality.High && uiSystem.UI_Screen is not null)
			{
				SpriteBatchState sBS = GraphicsUtils.GetState(sb).Value;
				if (AnimationState > 0)
				{
					if (AnimationTimer < 60)
					{
						AnimationTimer++;
					}
					sb.End();
					sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, sBS.RasterizerState, null, sBS.TransformMatrix);
					switch (AnimationState)
					{
						case 1:
							float value = AnimationTimer / 8f;
							value = Math.Clamp(value, 0, 1f);
							var effect = ModAsset.MissionDetailBlur.Value;
							effect.Parameters["uSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
							effect.Parameters["uBlurValue"].SetValue(value);
							effect.Parameters["uDelta"].SetValue(3f);
							effect.CurrentTechnique.Passes["Blur"].Apply();
							break;
					}
				}
				else
				{
					if (AnimationTimer > 0)
					{
						AnimationTimer--;
					}
				}
				if (AnimationState < 3)
				{
					sb.Draw(uiSystem.UI_Screen, Vector2.zeroVector, new Color(1f, 1f, 1f, 1f));
				}
				else
				{
					float value = AnimationTimer / 60f;
					value = Math.Clamp(value, 0, 1f);
					sb.Draw(uiSystem.UI_Screen, Vector2.zeroVector, Color.Lerp(Color.White,	Color.Red, value));
				}
				if (AnimationState > 0)
				{
					sb.End();
					sb.Begin(sBS);
				}
			}
			else
			{
				base.Draw(sb);
			}
		}
	}

	public void Draw_InRt2D(SpriteBatch sb)
	{
		base.Draw(sb);
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		Texture2D tex = ModAsset.MissionIconBoard.Value;
		sb.Draw(tex, Info.TotalHitBox, new Rectangle(16, 16, 16, 16), Color.White);
		base.DrawSelf(sb);
	}
}
