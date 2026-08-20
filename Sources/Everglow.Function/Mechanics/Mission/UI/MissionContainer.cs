using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.UI.UIElements;
using Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using Microsoft.CodeAnalysis;
using ReLogic.Graphics;
using Spine;
using Terraria.GameContent;

namespace Everglow.Commons.Mechanics.Mission.UI;

public class MissionContainer : UIContainerElement
{
	public static MissionContainer Instance => (MissionContainer)UISystem.EverglowUISystem.Elements[typeof(MissionContainer).FullName];

	public static UIMissionDetailSubContent DetailSub => Instance._missionDetailSubContent;

	public static UIMissionDetailTipContent DetailTip => Instance._missionDetailTip;

	public static UIMissionFilter Filter => Instance._missionFilter;

	public static MissionPresentationService Service => ModContent.GetInstance<MissionPresentationSystem>().Service;

	/// <summary>
	/// Scale factor for all UI elements in the mission system
	/// </summary>
	public static float Scale => Instance.ResolutionFactor;

	public int CurrentPanelWidth = 1840;

	public int CurrentPanelHeight = 1024;

	// ==================== UI elements ==================== //
	private UIBlock _panel;
	private UIMissionBackground _panelBackground;

	// private UIBlock _panelCoverContainer;
	// private UIImage _panelCover;
	private UIMissionDetail _missionDetail;
	private UIMissionDetailSubContent _missionDetailSubContent;
	private UIMissionDetailTipContent _missionDetailTip;

	private UIMissionList _missionList;
	private UIMissionFilter _missionFilter;
	private UIMissionSource _missionSourceHeadshot;

	private UIBlock _close;

	// ==================== Private data fields ==================== //
	private float resolutionFactor = 1;

	/// <summary>
	/// The factor used to scale the UI elements inside the mission system.
	/// </summary>
	public float ResolutionFactor
	{
		get => resolutionFactor;
		private set
		{
			resolutionFactor = Math.Max(value, 0.5f);
		}
	}

	/// <summary>
	/// Mouse text for tooltip
	/// </summary>
	public string MouseText { get; set; } = string.Empty;

	private MissionIdentity? _selectedMission;

	/// <summary>
	/// UI instance of the selected mission.
	/// <para/>Use <see cref="ChangeSelectedItem(UIMissionItem)"/> to change this value.
	/// <para/>If <c>null</c>, no mission item is selected.
	/// </summary>
	public UIMissionItem SelectedItem => _selectedMission is MissionIdentity identity
		? _missionList?.MissionItems.FirstOrDefault(item => item.View.Identity == identity)
		: null;

	public MissionContainer()
	{
		Player.Hooks.OnEnterWorld += OnEnterWorld;
		Main.OnResolutionChanged += OnResolutionChanged_Adapt;
	}

	public void SubscribePresentationEvents(MissionPresentationSystem presentationSystem)
	{
		presentationSystem.MissionAdded += _ => RefreshMissionContainer();
		presentationSystem.MissionRemoved += _ => RefreshMissionContainer();
		presentationSystem.MissionStatusUpdated += _ => RefreshMissionContainer();
		presentationSystem.MissionObjectiveUpdated += OnMissionObjectiveUpdated;
	}

	public void Unload()
	{
		Player.Hooks.OnEnterWorld -= OnEnterWorld;
		Main.OnResolutionChanged -= OnResolutionChanged_Adapt;
	}

	/// <summary>
	/// Close mission panel on enter world
	/// </summary>
	/// <param name="player"></param>
	private void OnEnterWorld(Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			Close();
			RefreshMissionContainer();
			_panel.Info.SetToCenter();
		}
	}

	/// <summary>
	/// Update resolution factor and refresh ui on resolution changed
	/// </summary>
	/// <param name="resolution"></param>
	private void OnResolutionChanged_Adapt(Vector2 resolution)
	{
		_panel.Info.SetToCenter();
	}

	private void RefreshMissionContainer()
	{
		if (!Main.gameMenu)
		{
			RefreshList();
		}
	}

	public override void OnInitialization()
	{
		base.OnInitialization();

		_panel = new UIBlock();
		_panel.PanelColor = Color.Transparent;
		_panel.CanDrag = true;
		_panel.CanLeftResize = true;
		_panel.CanRightResize = true;
		_panel.CanTopResize = true;
		_panel.CanBottomResize = true;
		_panel.MinWidthPixel = 1200;
		_panel.MinHeightPixel = 640;
		_panel.Info.Width.SetValue(2048);
		_panel.Info.Height.SetValue(1440);
		_panel.Info.SetToCenter();
		Register(_panel);

		// Background image
		_panelBackground = new UIMissionBackground();
		_panelBackground.Info.HiddenOverflow = true;
		_panelBackground.Info.SetMargin(0);
		_panelBackground.Info.CanBeInteract = false;
		_panelBackground.ShowBorder = (false, false, false, false);
		_panel.Register(_panelBackground);

		// Mission filter
		_missionFilter = new UIMissionFilter();
		_panel.Register(_missionFilter);

		// Mission source headshot
		_missionSourceHeadshot = new UIMissionSource();
		_panel.Register(_missionSourceHeadshot);

		// Mission details
		_missionDetail = new UIMissionDetail();
		_missionDetail.PanelColor = Color.Transparent;
		_missionDetail.BorderWidth = 0;
		_panel.Register(_missionDetail);

		// Mission detail mask
		_missionDetailSubContent = new UIMissionDetailSubContent();
		_missionDetailSubContent.BorderWidth = 0;
		_missionDetailSubContent.Info.InteractiveMask = true;
		_missionDetailSubContent.Info.IsVisible = false;
		_panel.Register(_missionDetailSubContent);

		// Mission detail tip
		_missionDetailTip = new UIMissionDetailTipContent();
		_missionDetailTip.BorderWidth = 0;
		_missionDetailTip.Info.InteractiveMask = true;
		_missionDetailTip.Info.IsVisible = false;
		_panel.Register(_missionDetailTip);

		// Mission list
		_missionList = new UIMissionList();
		_missionList.PanelColor = Color.Transparent;
		_missionList.BorderWidth = 0;
		_panel.Register(_missionList);

		// Close button
		_close = new UIBlock();
		_close.PanelColor = Color.Transparent;
		_close.BorderColor = Color.Transparent;
		_close.Info.IsSensitive = true;
		_close.Info.SetMargin(0);
		_close.Events.OnLeftDown += e => Close();
		_close.Events.OnMouseHover += e =>
		{
			MouseText = "Close";
			_close.PanelColor = Color.Gray;
		};
		_close.Events.OnMouseOver += e => _close.PanelColor = Color.Gray;
		_close.Events.OnMouseOut += e =>
		{
			MouseText = string.Empty;
			_close.PanelColor = Color.Transparent;
		};
		_panel.Register(_close);
	}

	public override void Calculation()
	{
		base.Calculation();
		bool shouldSetToCenter = false;
		if (CurrentPanelWidth > Main.screenWidth)
		{
			_panel.Info.Width.SetValue(Main.screenWidth, 0);
			shouldSetToCenter = true;
		}
		if (CurrentPanelHeight > Main.screenHeight)
		{
			_panel.Info.Height.SetValue(Main.screenHeight, 0);
			shouldSetToCenter = true;
		}
		if (_panel.Info.Width.Pixel > 0 && _panel.Info.Height.Pixel > 0)
		{
			CurrentPanelWidth = (int)_panel.Info.Width.Pixel;
			CurrentPanelHeight = (int)_panel.Info.Height.Pixel;
			if (CurrentPanelWidth < _panel.MinWidthPixel)
			{
				CurrentPanelWidth = _panel.MinWidthPixel;
				shouldSetToCenter = true;
			}
			if (CurrentPanelHeight < _panel.MinHeightPixel)
			{
				CurrentPanelHeight = _panel.MinHeightPixel;
				shouldSetToCenter = true;
			}
		}
		float width = CurrentPanelWidth;
		float height = CurrentPanelHeight;

		_panel.Info.Width.SetValue(width, 0f);
		_panel.Info.Height.SetValue(height, 0f);
		if (shouldSetToCenter)
		{
			_panel.Info.SetToCenter();
		}

		_panelBackground.Info.Width.SetFull();
		_panelBackground.Info.Height.SetFull();

		_missionSourceHeadshot.Info.Top.SetValue((210 - 40) * ResolutionFactor);
		_missionSourceHeadshot.Info.Left.SetValue((270 - 40) * ResolutionFactor);

		int squzzeLeftLimit = 1500;
		float leftPartWidth = 740;
		float detailWidth = width - 800;
		if (width < squzzeLeftLimit)
		{
			leftPartWidth -= squzzeLeftLimit - width;
			detailWidth = squzzeLeftLimit - 800;
		}
		_missionDetail.Info.Left.SetValue(leftPartWidth, 0);
		_missionDetail.Info.Top.SetValue(60);
		_missionDetail.Info.Width.SetValue(detailWidth);
		_missionDetail.Info.Height.SetValue(height - 120);

		_missionDetailSubContent.Info.Left.SetValue(leftPartWidth, 0);
		_missionDetailSubContent.Info.Top.SetValue(60);
		_missionDetailSubContent.Info.Width.SetValue(detailWidth);
		_missionDetailSubContent.Info.Height.SetValue(height - 120);

		_missionDetailTip.Info.Left.SetValue(leftPartWidth, 0);
		_missionDetailTip.Info.Top.SetValue(60);
		_missionDetailTip.Info.Width.SetValue(detailWidth);
		_missionDetailTip.Info.Height.SetValue(height - 120);

		float missionListWidth = 660;
		if (width < squzzeLeftLimit)
		{
			missionListWidth = 660 - (squzzeLeftLimit - width);
		}

		_missionList.Info.Top.SetValue(410f * ResolutionFactor, 0);
		_missionList.Info.Left.SetValue(40f * ResolutionFactor, 0);
		_missionList.Info.Width.SetValue(missionListWidth, 0f);
		_missionList.Info.Height.SetValue(height - 480 * ResolutionFactor, 0f);

		_missionFilter.Info.Top.SetValue(35);
		_missionFilter.Info.Left.SetValue(leftPartWidth * 0.5f - 350 * 0.5f);
		_missionFilter.Info.Width.SetValue(350);
		_missionFilter.Info.Height.SetValue(350);

		_close.Info.Width.SetValue(88 * ResolutionFactor);
		_close.Info.Height.SetValue(38 * ResolutionFactor);
		_close.Info.Left.SetValue(PositionStyle.Full - _close.Info.Width + (1, 0));

		// _panelCoverContainer.Info.Width.SetFull();
		// _panelCoverContainer.Info.Height.SetFull();

		// _panelCover.Info.Width.SetFull();
		// _panelCover.Info.Height.SetFull();
		// _panelCover.SourceRectangle = new Rectangle(0, 0, (int)(CurrentPanelWidth * ResolutionFactor), (int)(CurrentPanelHeight * ResolutionFactor));
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);
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
		// Open mission panel in different mode based on the arguments.
		if (args.Length == 1) // Open NPC mission panel
		{
			// Take the first argument as the NPC mode.
			if (args[0] is MissionSourceBase source)
			{
				// Set NPC mode and source NPC.
				_missionSourceHeadshot.Source = source;
			}
			else
			{
				// Throw an exception if the argument type is incorrect.
				throw new ArgumentException("Invalid argument types. Expected: nPCMode (bool) and nPCSource (int).");
			}
		}
		else // Open global mission panel
		{
			_missionSourceHeadshot.Source = null;
		}

		RefreshMissionContainer();

		// Display the mission panel.
		base.Show(args);
	}

	/// <summary>
	/// 打开任务面板，同时选中指定任务
	/// </summary>
	/// <param name="missionName"></param>
	public void ShowWithMission(string missionName)
	{
		Show();

		foreach (var missionItem in _missionList.MissionItems)
		{
			if (missionItem.View.Identity.DefinitionId == missionName)
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
		MissionIdentity? selectedMission = _selectedMission;
		_missionList.RefreshList(_missionFilter.MissionStateValue, _missionFilter.MissionTypeValue, _missionSourceHeadshot.Source);

		if (selectedMission is MissionIdentity identity)
		{
			UIMissionItem selectedItem = _missionList.MissionItems.FirstOrDefault(item => item.View.Identity == identity);
			ChangeSelectedItem(selectedItem);
		}
		else
		{
			ChangeSelectedItem(null);
		}
	}

	private void OnMissionObjectiveUpdated(MissionIdentity identity)
	{
		if (SelectedItem is not { } selectedItem
			|| identity.Side != MissionSide.Player
			|| selectedItem.View.Identity != identity
			|| !Service.TryGet(identity, out MissionPresentationEntry entry))
		{
			return;
		}

		selectedItem.UpdateEntry(entry);
		_missionDetail.RefreshObjectives(entry.View);
	}

	/// <summary>
	/// 改变选中的任务
	/// </summary>
	/// <param name="item"></param>
	public void ChangeSelectedItem(UIMissionItem item)
	{
		// 更新选中的任务
		var oldSelectedItem = SelectedItem;
		_selectedMission = item?.View.Identity;

		// 更新选中的任务的颜色
		oldSelectedItem?.OnUnselected();
		SelectedItem?.OnSelected();

		_missionDetail.UpdateChangeButton("45,38,33");
		_missionDetail.SetMissionDetail(item);

		if (item is not null && item.View.State == MissionViewState.Failed)
		{
			_missionDetail.AnimationState = 3;
			var fail = new UIMissionOperationFail("任务失败", yesText: "确认");
			DetailTip.Show(fail);
		}
		else
		{
			_missionDetail.AnimationState = 0;
			_missionDetail.AnimationTimer = 0;
			DetailTip.HideCurrent();
		}
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		// Draw tooltip
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
