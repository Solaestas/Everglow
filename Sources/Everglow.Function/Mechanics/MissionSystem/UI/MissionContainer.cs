using Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using ReLogic.Graphics;
using Terraria.GameContent;
using static Everglow.Commons.Mechanics.MissionSystem.Core.MissionManager;

namespace Everglow.Commons.Mechanics.MissionSystem.UI;

public class MissionContainer : UIContainerElement, ILoadable
{
	public const int BaseResolutionX = 2880;
	public const int BaseResolutionY = 1800;
	public const int PanelWidth = 1360;
	public const int PanelHeight = 800;

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

	public static float Scale => Instance.ResolutionFactor;

	public static MissionContainer Instance => (MissionContainer)UISystem.EverglowUISystem.Elements[typeof(MissionContainer).FullName];

	public float ResolutionFactor
	{
		get => resolutionFactor;
		private set
		{
			resolutionFactor = Math.Max(value, 0.5f);
		}
	}

	public string MouseText { get; set; } = string.Empty;

	private UIBlock _panel;
	private UIMissionBackground _panelBackground;

	private UIBlock _panelCoverContainer;
	private UIImage _panelCover;

	private UIMissionDetail _missionDetail;

	private UIMissionList _missionListBlock;

	private UIMissionFilter _missionFilter;

	private UIBlock _closeButton;

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
		_panel.PanelColor = Color.Transparent;
		_panel.Info.SetToCenter();
		_panel.CanDrag = true;
		Register(_panel);

		// Background image
		_panelBackground = new UIMissionBackground();
		_panelBackground.Info.Width.SetFull();
		_panelBackground.Info.Height.SetFull();
		_panelBackground.Info.HiddenOverflow = true;
		_panelBackground.Info.SetMargin(0);
		_panelBackground.Info.CanBeInteract = false;
		_panelBackground.ShowBorder = (false, false, false, false);
		_panel.Register(_panelBackground);

		// Mission filter
		_missionFilter = new UIMissionFilter();
		_missionFilter.Info.Top.SetValue(35 * Scale);
		_missionFilter.Info.Left.SetValue(95 * Scale);
		_panel.Register(_missionFilter);

		// Mission details
		_missionDetail = new UIMissionDetail();
		_missionDetail.Info.Left.SetValue(608 * ResolutionFactor);
		_missionDetail.Info.Top.SetValue(46 * ResolutionFactor);
		_missionDetail.Info.Width.SetValue(710 * ResolutionFactor, 0f);
		_missionDetail.Info.Height.SetValue(724 * ResolutionFactor, 0f);
		_missionDetail.PanelColor = Color.Transparent;
		_missionDetail.BorderWidth = 0;
		_panel.Register(_missionDetail);

		// Mission list
		_missionListBlock = new UIMissionList();
		_missionListBlock.Info.Top.SetValue(410f * ResolutionFactor, 0);
		_missionListBlock.Info.Left.SetValue(80f * ResolutionFactor, 0);
		_missionListBlock.Info.Width.SetValue(384f * ResolutionFactor, 0f);
		_missionListBlock.Info.Height.SetValue(360f * ResolutionFactor, 0f);
		_missionListBlock.PanelColor = Color.Transparent;
		_missionListBlock.BorderWidth = 0;
		_panel.Register(_missionListBlock);

		// Close button
		_closeButton = new UIBlock();
		_closeButton.Info.Width.SetValue(88 * Scale);
		_closeButton.Info.Height.SetValue(38 * Scale);
		_closeButton.Info.Left.SetValue(PositionStyle.Full - _closeButton.Info.Width + (1, 0));
		_closeButton.PanelColor = Color.Transparent;
		_closeButton.BorderColor = Color.Transparent;
		_closeButton.Info.IsSensitive = true;
		_closeButton.Info.SetMargin(0);
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

		// Cover image
		_panelCoverContainer = new UIBlock();
		_panelCoverContainer.Info.Width.SetFull();
		_panelCoverContainer.Info.Height.SetFull();
		_panelCoverContainer.Info.HiddenOverflow = true;
		_panelCoverContainer.Info.SetMargin(0);
		_panelCoverContainer.Info.CanBeInteract = false;
		_panelCoverContainer.PanelColor = Color.Transparent;
		_panelCoverContainer.ShowBorder = (false, false, false, false);
		_panel.Register(_panelCoverContainer);

		_panelCover = new UIImage(ModAsset.MissionBoardCoverLayer.Value, Color.White);
		_panelCover.Info.Width.SetFull();
		_panelCover.Info.Height.SetFull();
		_panelCover.Info.CanBeInteract = false;
		_panelCoverContainer.Register(_panelCover);
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

		foreach (var missionItem in _missionListBlock.MissionItems)
		{
			if (missionItem.Mission.Name == missionName)
			{
				ChangeSelectedItem(missionItem);
				return;
			}
		}
	}

	/// <summary>
	/// 刷新任务列表
	/// </summary>
	public void RefreshList()
	{
		_missionListBlock.RefreshList(_missionFilter.PoolTypeValue, _missionFilter.MissionTypeValue, nPCMode, sourceNPC);
		_panelBackground.SetSpectrumColor(_missionFilter.PoolTypeValue, _missionFilter.MissionTypeValue);

		// 重新选取之前选择的任务
		var filteredList = _missionListBlock.MissionItems.Where(e => e.Mission.Name == SelectedItem.Mission.Name);
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