using Everglow.Commons.MissionSystem;
using Everglow.Commons.UI.UIContainers.Mission.UIElements;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.MissionSystem.MissionManager;

namespace Everglow.Commons.UI.UIContainers.Mission;

public class MissionContainer : UIContainerElement
{
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

	public enum ColorType
	{
		Dark,
		Light,
	}

	public enum ColorStyle
	{
		Dark,
		Normal,
		Light,
	}

	public static MissionContainer Instance => (MissionContainer)UISystem.EverglowUISystem.Elements[typeof(MissionContainer).FullName];

	private UIBlock _panel;
	private UIBlock _headshot;
	private UIMissionFilter _missionFilter;
	private UIBlock _missionPanel;
	private UIBlock _description;
	private UIBlock _changeMission;
	private UIMissionItem _mission;
	private UIMissionVerticalScrollbar _missionScrollbar;
	private UITextVerticalScrollbar _textScrollbar;
	private UIBlock _closeButton;
	private UIBlock _maximizeButton;
	private UIContainerPanel _missionContainer;
	private UIImage _close;
	private UIImage _maximization;
	private UIMissionIcon _icon;
	private UIContainerPanel _descriptionContainer;
	private UITextPlus _changeText;
	private UITextPlus _yes;
	private UITextPlus _no;

	private bool nPCMode = false;
	private int sourceNPC = 0;

	/// <summary>
	/// 选中的任务
	/// <para/>注: 请通过<see cref="ChangeSelectedItem(UIMissionItem)"/>来修改此属性
	/// </summary>
	public UIMissionItem SelectedItem { get; private set; }

	public Color GetThemeColor(ColorType type = ColorType.Dark, ColorStyle style = ColorStyle.Normal)
	{
		Color color = type switch
		{
			ColorType.Light => new Color(196, 241, 255),
			_ => new Color(68, 53, 51),
		};
		float factor = style switch
		{
			ColorStyle.Dark => 0.66f,
			ColorStyle.Light => 1.7f,
			_ => 1f,
		};
		color *= factor;
		color.A = 255;
		return color;
	}

	public override void OnInitialization()
	{
		base.OnInitialization();

		// 在进入世界时关闭UI
		Player.Hooks.OnEnterWorld += p =>
		{
			if (p.whoAmI == Main.myPlayer)
			{
				Close();
			}
		};

		_panel = new UIBlock();
		_panel.Info.Width.SetValue(527f, 0f);
		_panel.Info.Height.SetValue(369f, 0f);
		_panel.PanelColor = GetThemeColor();
		_panel.Info.SetToCenter();
		_panel.CanDrag = true;
		Register(_panel);

		_headshot = new UIBlock();
		_headshot.Info.Width.SetValue(82f, 0f);
		_headshot.Info.Height.SetValue(82f, 0f);
		_headshot.Info.Left.SetValue(90f, 0f);
		_headshot.Info.Top.SetValue(54f, 0f);
		_headshot.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_panel.Register(_headshot);

		_icon = new UIMissionIcon(null, Color.White);
		_icon.Info.Width.SetFull();
		_icon.Info.Height.SetFull();
		_headshot.Register(_icon);

		_description = new UIBlock();
		_description.Info.Left.SetValue(38f, 0f);
		_description.Info.Top.SetValue(_headshot.Info.Top + _headshot.Info.Height + (20f, 0f));
		_description.Info.Width.SetValue(_headshot.Info.Left * 2f + _headshot.Info.Width - _description.Info.Left * 2f);
		_description.Info.Height.SetValue(PositionStyle.Full - _description.Info.Top - (66f, 0f));
		_description.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_panel.Register(_description);

		_missionPanel = new UIBlock();
		_panel.Register(_missionPanel);

		_missionContainer = new UIContainerPanel();
		_missionContainer.Info.SetMargin(0f);
		_missionPanel.Register(_missionContainer);

		_missionScrollbar = new UIMissionVerticalScrollbar();
		_missionScrollbar.Info.Left.SetValue(PositionStyle.Full - _missionScrollbar.Info.Width - (10f, 0f));
		_missionContainer.SetVerticalScrollbar(_missionScrollbar);
		_panel.Register(_missionScrollbar);

		_missionFilter = new UIMissionFilter();
		_missionFilter.StatusFilterChanged += (_, type) =>
		{
			RefreshList();
		};
		_panel.Register(_missionFilter);

		_missionFilter.Info.Top.SetValue(_headshot.Info.Top - (16f, 0f));
		_missionFilter.Info.Left.SetValue(_description.Info.Left * 2f + _description.Info.Width);
		_missionFilter.Info.Width.SetValue(PositionStyle.Full - _description.Info.Left * 2f - _description.Info.Width - _missionScrollbar.Info.Width - (PositionStyle.Full - _missionScrollbar.Info.Width - _missionScrollbar.Info.Left) * 2f);
		_missionFilter.Info.Height.SetValue(40f, 0f);
		_missionFilter.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_missionFilter.ShowBorder.BottomBorder = false;

		_missionPanel.Info.Top.SetValue(_missionFilter.Info.Top + _missionFilter.Info.Height);
		_missionPanel.Info.Left.SetValue(_description.Info.Left * 2f + _description.Info.Width);
		_missionPanel.Info.Width.SetValue(PositionStyle.Full - _description.Info.Left * 2f - _description.Info.Width - _missionScrollbar.Info.Width - (PositionStyle.Full - _missionScrollbar.Info.Width - _missionScrollbar.Info.Left) * 2f);
		_missionPanel.Info.Height.SetValue(_panel.Info.Height - _missionFilter.Info.Height - (62f, 0f));
		_missionPanel.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_missionPanel.ShowBorder.TopBorder = false;

		_missionScrollbar.Info.Height.SetValue(_missionPanel.Info.Height);
		_missionScrollbar.Info.Top.SetValue(_missionPanel.Info.Top);

		_textScrollbar = new UITextVerticalScrollbar();
		_textScrollbar.Info.Height.SetValue(-16f, 1f);
		_textScrollbar.Info.SetToCenter();
		_textScrollbar.Info.Left.SetValue(-8f, 1f);
		_description.Register(_textScrollbar);

		_descriptionContainer = new UIContainerPanel();
		_descriptionContainer.Info.Width.SetValue(PositionStyle.Full - _textScrollbar.Info.Width -
			(PositionStyle.Full - _textScrollbar.Info.Left - _textScrollbar.Info.Width) * 3f);
		_descriptionContainer.Info.Height.SetValue(_textScrollbar.Info.Height);
		_descriptionContainer.Info.Left.SetValue(PositionStyle.Full - _textScrollbar.Info.Left - _textScrollbar.Info.Width);
		_descriptionContainer.Info.Top.SetValue(_textScrollbar.Info.Top);
		_descriptionContainer.SetVerticalScrollbar(_textScrollbar);
		_description.Register(_descriptionContainer);

		_closeButton = new UIBlock();
		_closeButton.Info.Width.SetValue(36f, 0f);
		_closeButton.Info.Height.SetValue(22f, 0f);
		_closeButton.Info.Left.SetValue(PositionStyle.Full - _closeButton.Info.Width - (6f, 0f));
		_closeButton.Info.Top.SetValue(6f, 0f);
		_closeButton.PanelColor = GetThemeColor();
		_closeButton.BorderColor = GetThemeColor(ColorType.Light, ColorStyle.Normal);
		_closeButton.Info.IsSensitive = true;
		_closeButton.Events.OnLeftClick += e => Close();
		_closeButton.Events.OnMouseHover += e => _closeButton.PanelColor = Color.Gray;
		_closeButton.Events.OnMouseOver += e => _closeButton.PanelColor = Color.Gray;
		_closeButton.Events.OnMouseOut += e => _closeButton.PanelColor = GetThemeColor();
		_panel.Register(_closeButton);

		_close = new UIImage(ModAsset.MissionClose.Value, Color.White);
		_close.Info.SetToCenter();
		_closeButton.Register(_close);

		// _maximizeButton = new UIBlock();
		// _maximizeButton.Info.Width.SetValue(_closeButton.Info.Width);
		// _maximizeButton.Info.Height.SetValue(_closeButton.Info.Height);
		// _maximizeButton.Info.Left.SetValue(_closeButton.Info.Left - _maximizeButton.Info.Width - (2f, 0f));
		// _maximizeButton.Info.Top.SetValue(_closeButton.Info.Top);
		// _maximizeButton.PanelColor = GetThemeColor();
		// _maximizeButton.BorderColor = GetThemeColor(ColorType.Light, ColorStyle.Normal);
		// _panel.Register(_maximizeButton);

		// _maximization = new UIImage(ModAsset.MissionMaximization.Value, Color.White);
		// _maximization.Info.SetToCenter();
		// _maximizeButton.Register(_maximization);

		_changeMission = new UIBlock();
		_changeMission.Info.Width.SetValue(48f, 0f);
		_changeMission.Info.Height.SetValue(26f, 0f);
		_changeMission.Info.Left.SetValue(_description.Info.Left + (_description.Info.Width - _changeMission.Info.Width) / 2f);
		_changeMission.Info.Top.SetValue(_description.Info.Top + _description.Info.Height +
			((PositionStyle.Full - _description.Info.Top - _description.Info.Height) - _changeMission.Info.Height) / 2f);
		_changeMission.Info.IsSensitive = true;
		_changeMission.PanelColor = GetThemeColor();
		_changeMission.Events.OnLeftClick += e =>
		{
			if (SelectedItem != null)
			{
				if (SelectedItem.Mission.PoolType == PoolType.Accepted)
				{
					_yes.Info.IsVisible = _no.Info.IsVisible = true;
				}
				else if (SelectedItem.Mission.PoolType == PoolType.Available)
				{
					MissionManager.Instance.MoveMission(SelectedItem.Mission, PoolType.Available, PoolType.Accepted);
					ChangeSelectedItem(SelectedItem);
				}
			}
		};
		_panel.Register(_changeMission);

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
		_yes.Events.OnLeftClick += e =>
		{
			if (SelectedItem != null && SelectedItem.Mission.PoolType == PoolType.Accepted)
			{
				if (SelectedItem.Mission.CheckComplete())
				{
					SelectedItem.Mission.OnComplete();
					ChangeSelectedItem(SelectedItem);
					_yes.Info.IsVisible = _no.Info.IsVisible = false;
				}
				else
				{
					MissionManager.Instance.MoveMission(SelectedItem.Mission, PoolType.Accepted, PoolType.Failed);
					ChangeSelectedItem(SelectedItem);
					_yes.Info.IsVisible = _no.Info.IsVisible = false;
				}
			}
		};
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
		_no.Events.OnLeftClick += e =>
		{
			_yes.Info.IsVisible = _no.Info.IsVisible = false;
		};
		_changeMission.Register(_no);

		_changeText = new UITextPlus(string.Empty);
		_changeText.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
		_changeText.StringDrawer.Init(_changeText.Text);
		_changeMission.Register(_changeText);
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);

		if (NeedRefresh)
		{
			RefreshList();

			NeedRefresh = false;
		}

		Calculation();
	}

	/// <summary>
	/// 打开任务面板
	/// 该方法用于显示任务面板，并根据传入的参数设置 NPC 模式和 NPC 来源。
	/// <para/>打开全局任务面板：无需参数
	/// <para/>打开NPC任务面板：
	/// 1. nPCMode (bool): 表示是否启用 NPC 模式。
	/// 2. nPCSource (int): 表示来源 NPC 的 ID。
	/// </summary>
	/// <param name="args"></param>
	/// <exception cref="ArgumentException">
	/// 如果参数类型不正确，抛出此异常。
	/// </exception>
	public override void Show(params object[] args)
	{
		// 检查参数数量是否足够
		if (args.Length == 1)
		{
			// 使用模式匹配提取并转换参数
			if (args[0] is int npcSource)
			{
				// 设置 NPC 模式和来源 NPC
				nPCMode = true;
				sourceNPC = npcSource;
			}
			else
			{
				// 如果参数类型不正确，抛出异常
				throw new ArgumentException("Invalid argument types. Expected: nPCMode (bool) and nPCSource (int).");
			}
		}
		else
		{
			nPCMode = false;
		}

		// 调用基类的 Show 方法
		base.Show(args);

		// 刷新任务列表
		RefreshList();
	}

	/// <summary>
	/// 打开任务面板，同时选中指定任务
	/// </summary>
	/// <param name="missionName"></param>
	public void ShowWithMission(string missionName)
	{
		Show();

		foreach (var e in _missionContainer.Elements)
		{
			if (e is UIMissionItem missionItem)
			{
				if (missionItem.Mission.Name == missionName)
				{
					ChangeSelectedItem(missionItem);
					return;
				}
			}
		}
	}

	/// <summary>
	/// 刷新任务列表
	/// </summary>
	public void RefreshList()
	{
		PositionStyle IteratePool(List<BaseElement> elements, PositionStyle top, List<MissionBase> mp)
		{
			foreach (var m in mp)
			{
				if (!m.IsVisible)
				{
					continue;
				}

				// NPC模式，去掉非对应NPC的
				if (nPCMode)
				{
					if (sourceNPC != m.SourceNPC)
					{
						continue;
					}
				}
				else // 全局模式，去掉未接取中有来源NPC的
				{
					if (m.PoolType is PoolType.Available && m.SourceNPC >= 0)
					{
						continue;
					}
				}

				BaseElement element;

				element = (BaseElement)Activator.CreateInstance(m.BindingUIItem, [m]);

				element.Info.Top.SetValue(top);
				element.Events.OnLeftClick += e =>
				{
					if (SelectedItem != e)
					{
						ChangeSelectedItem((UIMissionItem)e);
					}
				};

				elements.Add(element);

				top += element.Info.Height;
				top.Pixel += 2f;
			}

			return top;
		}

		List<BaseElement> elements = [];
		PositionStyle top = (2f, 0f);
		if (_missionFilter.PoolType.HasValue)
		{
			var mp = MissionManager.Instance.GetMissionPool(_missionFilter.PoolType.Value);
			top = IteratePool(elements, top, mp);
		}
		else
		{
			foreach (var type in Enum.GetValues<PoolType>())
			{
				// NPC模式，只看未接取的
				if (nPCMode && type is not PoolType.Available)
				{
					continue;
				}

				var mp = MissionManager.Instance.GetMissionPool(type);
				top = IteratePool(elements, top, mp);
			}
		}

		ChangeSelectedItem(null);
		_missionContainer.ClearAllElements();
		_missionContainer.AddElements(elements);
	}

	/// <summary>
	/// 改变选中的任务
	/// </summary>
	/// <param name="item"></param>
	public void ChangeSelectedItem(UIMissionItem item)
	{
		// 更新选中的任务
		var oldSelectedItem = SelectedItem;
		SelectedItem = item;

		// 更新选中的任务的颜色
		oldSelectedItem?.OnUnselected();
		SelectedItem?.OnSelected();

		UpdateChangeButton();

		if (SelectedItem != null)
		{
			_icon.SetIconGroup(SelectedItem.Mission.Icon);
			_textScrollbar.WheelValue = 0f;

			UITextPlus des = new UITextPlus(SelectedItem.Mission.Description);
			des.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
			des.StringDrawer.Init(des.Text);
			_descriptionContainer.ClearAllElements();
			_descriptionContainer.AddElement(des);
		}
		else
		{
			_icon.SetIconGroup(null);
			_textScrollbar.WheelValue = 0f;
			_descriptionContainer.ClearAllElements();
		}
	}

	/// <summary>
	/// 更新按钮的状态
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