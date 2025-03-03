using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using ReLogic.Graphics;
using Terraria.GameContent;
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

	private float ResolutionFactor
	{
		get => resolutionFactor;
		set
		{
			resolutionFactor = Math.Max(value, 1.1f);
		}
	}

	public string MouseText { get; set; } = string.Empty;

	private UIBlock _panel;

	private UIMissionDetail _missionDetail;

	private UIBlock _missionListContainer;
	private UIBlock _missionListContent;
	private UIContainerPanel _missionList;

	private UIMissionVerticalScrollbar _missionScrollbar;
	private UIMissionStatusFilter _missionFilter;
	private UIMissionTypeFilter _missionTypeFilter;

	private UIBlock _closeButton;
	private UIImage _close;

	private bool nPCMode = false;
	private int sourceNPC = 0;
	private float resolutionFactor;

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

		PositionStyle top = new(height * 0.08f, 0f);
		PositionStyle left = new(width * 0.06f, 0f);

		float detailWidthRatio = 0.38f;

		// Mission list container
		_missionListContainer = new UIBlock();
		_missionListContainer.Info.Top.SetValue(top);
		_missionListContainer.Info.Left.SetValue(left);
		_missionListContainer.Info.Width.SetValue(PositionStyle.Full - left * 3f - (width * detailWidthRatio, 0));
		_missionListContainer.Info.Height.SetValue(_panel.Info.Height * (1f - 0.15f));
		_missionListContainer.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_missionListContainer.ShowBorder = (false, false, false, false);
		_panel.Register(_missionListContainer);

		// Mission details
		_missionDetail = new UIMissionDetail();
		_missionDetail.Info.Left.SetValue(left * 2f + _missionListContainer.Info.Width);
		_missionDetail.Info.Top.SetValue(top);
		_missionDetail.Info.Width.SetValue(width * detailWidthRatio, 0f);
		_missionDetail.Info.Height.SetValue(height * 0.9f, 0f);
		_panel.Register(_missionDetail);

		// Mission list status filter
		_missionFilter = new UIMissionStatusFilter();
		_missionFilter.Info.Left.SetValue(0);
		_missionFilter.Info.Width.SetValue(PositionStyle.Full);
		_missionFilter.Info.Height.SetValue(40f, 0f);
		_missionFilter.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_missionFilter.ShowBorder = (false, false, false, false);
		_missionListContainer.Register(_missionFilter);

		// Mission list type filter
		_missionTypeFilter = new UIMissionTypeFilter();
		_missionTypeFilter.Info.Top.SetValue(_missionFilter.Info.Height);
		_missionTypeFilter.Info.Left.SetValue(0);
		_missionTypeFilter.Info.Width.SetValue(PositionStyle.Full);
		_missionTypeFilter.Info.Height.SetValue(40f, 0f);
		_missionTypeFilter.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_missionTypeFilter.ShowBorder = (false, false, false, false);
		_missionListContainer.Register(_missionTypeFilter);

		// Mission list content
		_missionListContent = new UIBlock();
		_missionListContent.Info.Width.SetValue(PositionStyle.Full);
		_missionListContent.Info.Height.SetValue(PositionStyle.Full - _missionFilter.Info.Height - _missionTypeFilter.Info.Height);
		_missionListContent.Info.Top.SetValue(_missionFilter.Info.Height + _missionTypeFilter.Info.Height);
		_missionListContent.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_missionListContent.ShowBorder = (false, false, false, false);
		_missionListContainer.Register(_missionListContent);

		// Mission list
		_missionList = new UIContainerPanel();
		_missionListContent.Register(_missionList);

		// Mission list scrollbar
		_missionScrollbar = new UIMissionVerticalScrollbar();
		_missionScrollbar.Info.Left.SetValue(PositionStyle.Full - _missionScrollbar.Info.Width - (10f, 0f));
		_missionScrollbar.Info.Height.SetValue(PositionStyle.Full - (20, 0f));
		_missionList.SetVerticalScrollbar(_missionScrollbar);
		_missionListContent.Register(_missionScrollbar);

		_missionList.Info.Width.SetValue(_missionScrollbar.Info.Left);

		// Close button
		_closeButton = new UIBlock();
		_closeButton.Info.Width.SetValue(36f, 0f);
		_closeButton.Info.Height.SetValue(22f, 0f);
		_closeButton.Info.Left.SetValue(PositionStyle.Full - _closeButton.Info.Width - (6f, 0f));
		_closeButton.Info.Top.SetValue(6f, 0f);
		_closeButton.PanelColor = GetThemeColor();
		_closeButton.BorderColor = GetThemeColor(ColorType.Light, ColorStyle.Normal);
		_closeButton.Info.IsSensitive = true;
		_closeButton.Events.OnLeftDown += e => Close();
		_closeButton.Events.OnMouseHover += e =>
		{
			MouseText = "Close";
			_closeButton.PanelColor = Color.Gray;
		};
		_closeButton.Events.OnMouseOver += e => _closeButton.PanelColor = Color.Gray;
		_closeButton.Events.OnMouseOut += e =>
		{
			MouseText = string.Empty;
			_closeButton.PanelColor = GetThemeColor();
		};
		_panel.Register(_closeButton);

		_close = new UIImage(ModAsset.MissionClose.Value, Color.White);
		_close.Info.SetToCenter();
		_closeButton.Register(_close);
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

		ChildrenElements.Clear();
		OnInitialization();

		// 刷新任务列表
		RefreshList();

		// 调用基类的 Show 方法
		base.Show(args);
	}

	/// <summary>
	/// 打开任务面板，同时选中指定任务
	/// </summary>
	/// <param name="missionName"></param>
	public void ShowWithMission(string missionName)
	{
		Show();

		foreach (var e in _missionList.Elements)
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

		_missionList.ClearAllElements();
		_missionList.AddElements(elements);

		// 重新选取之前选择的任务
		var filteredList = elements.ConvertAll(x => x as UIMissionItem).Where(e => e.Mission.Name == SelectedItem.Mission.Name);
		if (SelectedItem != null && filteredList.Any())
		{
			ChangeSelectedItem(filteredList.First());
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

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		// Draw filter item tooltip
		if (!string.IsNullOrEmpty(MouseText))
		{
			var pos = Main.MouseScreen + new Vector2(10f, 18f);
			var textSize = FontAssets.MouseText.Value.MeasureString(MouseText);

			if (pos.X + textSize.X > Main.screenWidth)
			{
				pos.X = Main.screenWidth - textSize.X;
			}
			if (pos.Y + textSize.Y > Main.screenHeight)
			{
				pos.Y = Main.screenHeight - textSize.Y;
			}
			if (pos.X < 0)
			{
				pos.X = 0;
			}
			if (pos.Y < 0)
			{
				pos.Y = 0;
			}

			var PanelColor = new Color(191, 106, 106);
			Texture2D texture = ModAsset.Panel.Value;
			var textureSize = new Point(texture.Width, texture.Height);
			var rectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)textSize.X, (int)textSize.Y);

			// Draw 4 corners
			sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y), new Rectangle(0, 0, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y), new Rectangle(textureSize.X - 6, 0, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y + rectangle.Height - 6), new Rectangle(0, textureSize.Y - 6, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y + rectangle.Height - 6), new Rectangle(textureSize.X - 6, textureSize.Y - 6, 6, 6), PanelColor);

			// Draw main part
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y, rectangle.Width - 12, 6), new Rectangle(6, 0, textureSize.X - 12, 6), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + rectangle.Height - 6, rectangle.Width - 12, 6), new Rectangle(6, textureSize.Y - 6, textureSize.X - 12, 6), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(0, 6, 6, textureSize.Y - 12), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - 6, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(textureSize.X - 6, 6, 6, textureSize.Y - 12), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + 6, rectangle.Width - 12, rectangle.Height - 12), new Rectangle(6, 6, textureSize.X - 12, textureSize.Y - 12), PanelColor);

			// Draw text
			sb.DrawString(FontAssets.MouseText.Value, MouseText, pos + new Vector2(0f, 5f), Color.Cyan);

			MouseText = string.Empty;
		}
	}
}