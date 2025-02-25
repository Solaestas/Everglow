using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.Mechanics.MissionSystem.Core.MissionManager;

namespace Everglow.Commons.Mechanics.MissionSystem.UI;

public class MissionContainer : UIContainerElement, ILoadable
{
	public const int BaseResolutionX = 1200;
	public const int BaseResolutionY = 675;
	public const int PanelWidth = 600;
	public const int PanelHeight = 400;

	public MissionContainer()
	{
		Player.Hooks.OnEnterWorld += OnEnterWorld_Close;
		Main.OnResolutionChanged += OnResolutionChanged_Adapt;

		// Initial resolution factor
		if (Main.LastLoadedResolution.X / Main.LastLoadedResolution.Y > 16f / 9f)
		{
			ResolutionFactor = Main.LastLoadedResolution.Y / BaseResolutionY;
		}
		else
		{
			ResolutionFactor = Main.LastLoadedResolution.X / BaseResolutionX;
		}
	}

	/// <summary>
	/// Close mission panel on enter world
	/// </summary>
	/// <param name="player"></param>
	private void OnEnterWorld_Close(Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			Close();
		}
	}

	/// <summary>
	/// Update resolution factor and refresh ui on resolution changed
	/// </summary>
	/// <param name="resolution"></param>
	private void OnResolutionChanged_Adapt(Vector2 resolution)
	{
		if (resolution.X / resolution.Y > 16f / 9f)
		{
			ResolutionFactor = resolution.Y / BaseResolutionY;
		}
		else
		{
			ResolutionFactor = resolution.X / BaseResolutionX;
		}

		ChildrenElements.Clear();
		OnInitialization();
		if (!Main.gameMenu)
		{
			RefreshList();
		}
	}

	public void Load(Mod mod)
	{
	}

	public void Unload()
	{
		Player.Hooks.OnEnterWorld -= OnEnterWorld_Close;
		Main.OnResolutionChanged -= OnResolutionChanged_Adapt;
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

	private float ResolutionFactor { get; set; }

	private UIBlock _panel;

	private UIMissionDetail _missionDetail;

	private UIMissionStatusFilter _missionFilter;
	private UIMissionTypeFilter _missionTypeFilter;

	private UIBlock _missionPanel;
	private UIMissionVerticalScrollbar _missionScrollbar;
	private UIContainerPanel _missionContainer;

	private UIBlock _closeButton;
	private UIImage _close;
	private UIBlock _maximizeButton;
	private UIImage _maximization;

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

		float width = PanelWidth * ResolutionFactor;
		float height = PanelHeight * ResolutionFactor;

		_panel = new UIBlock();
		_panel.Info.Width.SetValue(width, 0f);
		_panel.Info.Height.SetValue(height, 0f);
		_panel.PanelColor = GetThemeColor();
		_panel.Info.SetToCenter();
		_panel.CanDrag = true;
		Register(_panel);

		_missionDetail = new UIMissionDetail();
		_missionDetail.Info.Left.SetValue(width * 0.06f, 0f);
		_missionDetail.Info.Top.SetValue(height * 0.09f, 0f);
		_missionDetail.Info.Width.SetValue(width * 0.38f, 0f);
		_missionDetail.Info.Height.SetValue(height * 0.9f, 0f);
		_missionDetail.OnClickChange += (_, _) =>
		{
			if (SelectedItem != null)
			{
				if (SelectedItem.Mission.PoolType == PoolType.Accepted)
				{
					_missionDetail.ShowYesNo();
				}
				else if (SelectedItem.Mission.PoolType == PoolType.Available)
				{
					MoveMission(SelectedItem.Mission, PoolType.Available, PoolType.Accepted);
					ChangeSelectedItem(SelectedItem);
				}
			}
		};
		_missionDetail.OnClickYes += (_, _) =>
		{
			if (SelectedItem != null && SelectedItem.Mission.PoolType == PoolType.Accepted)
			{
				if (SelectedItem.Mission.CheckComplete())
				{
					SelectedItem.Mission.OnComplete();
					ChangeSelectedItem(SelectedItem);
					_missionDetail.HideYesNo();
				}
				else
				{
					MoveMission(SelectedItem.Mission, PoolType.Accepted, PoolType.Failed);
					ChangeSelectedItem(SelectedItem);
					_missionDetail.HideYesNo();
				}
			}
		};
		_panel.Register(_missionDetail);

		_missionPanel = new UIBlock();
		_panel.Register(_missionPanel);

		_missionContainer = new UIContainerPanel();
		_missionContainer.Info.SetMargin(0f);
		_missionPanel.Register(_missionContainer);

		_missionScrollbar = new UIMissionVerticalScrollbar();
		_missionScrollbar.Info.Left.SetValue(PositionStyle.Full - _missionScrollbar.Info.Width - (10f, 0f));
		_missionContainer.SetVerticalScrollbar(_missionScrollbar);
		_panel.Register(_missionScrollbar);

		_missionFilter = new UIMissionStatusFilter();
		_panel.Register(_missionFilter);

		_missionFilter.Info.Top.SetValue(_missionDetail.Info.Top);
		_missionFilter.Info.Left.SetValue(_missionDetail.Info.Left * 2f + _missionDetail.Info.Width);
		_missionFilter.Info.Width.SetValue(PositionStyle.Full - _missionDetail.Info.Left * 2f - _missionDetail.Info.Width - _missionScrollbar.Info.Width - (PositionStyle.Full - _missionScrollbar.Info.Width - _missionScrollbar.Info.Left) * 2f);
		_missionFilter.Info.Height.SetValue(40f, 0f);
		_missionFilter.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);

		_missionTypeFilter = new UIMissionTypeFilter();
		_panel.Register(_missionTypeFilter);

		_missionTypeFilter.Info.Top.SetValue(_missionFilter.Info.Top + _missionFilter.Info.Height);
		_missionTypeFilter.Info.Left.SetValue(_missionDetail.Info.Left * 2f + _missionDetail.Info.Width);
		_missionTypeFilter.Info.Width.SetValue(PositionStyle.Full - _missionDetail.Info.Left * 2f - _missionDetail.Info.Width - _missionScrollbar.Info.Width - (PositionStyle.Full - _missionScrollbar.Info.Width - _missionScrollbar.Info.Left) * 2f);
		_missionTypeFilter.Info.Height.SetValue(40f, 0f);
		_missionTypeFilter.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);

		_missionPanel.Info.Top.SetValue(_missionTypeFilter.Info.Top + _missionTypeFilter.Info.Height);
		_missionPanel.Info.Left.SetValue(_missionDetail.Info.Left * 2f + _missionDetail.Info.Width);
		_missionPanel.Info.Width.SetValue(PositionStyle.Full - _missionDetail.Info.Left * 2f - _missionDetail.Info.Width - _missionScrollbar.Info.Width - (PositionStyle.Full - _missionScrollbar.Info.Width - _missionScrollbar.Info.Left) * 2f);
		_missionPanel.Info.Height.SetValue(_panel.Info.Height - _missionFilter.Info.Height - _missionTypeFilter.Info.Height - _panel.Info.Height * 0.15f);
		_missionPanel.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_missionPanel.ShowBorder.TopBorder = false;

		_missionScrollbar.Info.Height.SetValue(_missionPanel.Info.Height);
		_missionScrollbar.Info.Top.SetValue(_missionPanel.Info.Top);

		_closeButton = new UIBlock();
		_closeButton.Info.Width.SetValue(36f, 0f);
		_closeButton.Info.Height.SetValue(22f, 0f);
		_closeButton.Info.Left.SetValue(PositionStyle.Full - _closeButton.Info.Width - (6f, 0f));
		_closeButton.Info.Top.SetValue(6f, 0f);
		_closeButton.PanelColor = GetThemeColor();
		_closeButton.BorderColor = GetThemeColor(ColorType.Light, ColorStyle.Normal);
		_closeButton.Info.IsSensitive = true;
		_closeButton.Events.OnLeftDown += e => base.Close();
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
		// 筛选任务状态，获得初始列表
		var missions = _missionFilter.PoolType.HasValue
			? GetMissionPool(_missionFilter.PoolType.Value)
			: Enum.GetValues<PoolType>().Select(GetMissionPool).SelectMany(x => x);

		// 筛选来源NPC
		if (nPCMode) // NPC模式，去掉非对应NPC的未接取任务
		{
			missions = missions.Where(m => m.PoolType is not PoolType.Available && m.SourceNPC == sourceNPC);
		}
		else // 全局模式，去掉有来源NPC的未接取任务
		{
			missions = missions.Where(m => !(m.PoolType is PoolType.Available && m.SourceNPC >= 0));
		}

		// 筛选任务类型
		if (_missionTypeFilter.MissionType.HasValue)
		{
			missions = missions.Where(m => m.MissionType == _missionTypeFilter.MissionType);
		}

		// 生成任务UI元素
		List<BaseElement> elements = [];
		const int ElementSpacing = 2;
		PositionStyle top = (ElementSpacing, 0f);
		foreach (var m in missions.ToList())
		{
			if (!m.IsVisible)
			{
				continue;
			}

			var element = (BaseElement)Activator.CreateInstance(m.BindingUIItem, [m]);
			element.Info.Top.SetValue(top);
			element.Events.OnLeftDown += e =>
			{
				if (SelectedItem != e)
				{
					ChangeSelectedItem((UIMissionItem)e);
				}
			};

			elements.Add(element);

			top += element.Info.Height;
			top.Pixel += ElementSpacing;
		}

		_missionContainer.ClearAllElements();
		_missionContainer.AddElements(elements);

		// 重新选取之前选择的任务
		if (SelectedItem != null && elements.Count != 0)
		{
			var target = elements.ConvertAll(x => x as UIMissionItem).Where(e => e.Mission.Name == SelectedItem.Mission.Name).First();
			ChangeSelectedItem(target);
		}
		else
		{
			ChangeSelectedItem(null);
		}
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

		_missionDetail.UpdateChangeButton();
		_missionDetail.SetMissionDetail(item);
	}
}