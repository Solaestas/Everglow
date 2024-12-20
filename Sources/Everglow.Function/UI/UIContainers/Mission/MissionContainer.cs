using Everglow.Commons.MissionSystem;
using Everglow.Commons.UI.UIContainers.Mission.UIElements;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.MissionSystem.MissionManager;

namespace Everglow.Commons.UI.UIContainers.Mission;

public class MissionContainer : UIContainerElement
{
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
	private UIBlock _missionPanel;
	private UIBlock _description;
	private UIBlock _changeMission;
	private UIMissionItem _mission;
	private UIMissionVerticalScrollbar _missionScrollbar;
	private UITextVerticalScrollbar _textScrollbal;
	private UIBlock _closeButton;
	private UIBlock _maximizeButton;
	private UIContainerPanel _missionContainer;
	private UIImage _close;
	private UIImage _maximization;
	private UIImage _icon;
	private UIContainerPanel _descriptionContainer;
	private UITextPlus _changeText;
	private UITextPlus _yes;
	private UITextPlus _no;

	public UIMissionItem SelectedItem;

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

		_icon = new UIImage(null, Color.White);
		_icon.Info.Width.SetFull();
		_icon.Info.Height.SetFull();
		_headshot.Register(_icon);

		_missionPanel = new UIBlock();
		_panel.Register(_missionPanel);

		_missionContainer = new UIContainerPanel();
		_missionContainer.Info.SetMargin(0f);
		_missionPanel.Register(_missionContainer);

		_description = new UIBlock();
		_description.Info.Left.SetValue(38f, 0f);
		_description.Info.Top.SetValue(_headshot.Info.Top + _headshot.Info.Height + (20f, 0f));
		_description.Info.Width.SetValue(_headshot.Info.Left * 2f + _headshot.Info.Width - _description.Info.Left * 2f);
		_description.Info.Height.SetValue(PositionStyle.Full - _description.Info.Top - (66f, 0f));
		_description.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		_panel.Register(_description);

		_missionScrollbar = new UIMissionVerticalScrollbar();
		_missionScrollbar.Info.Left.SetValue(PositionStyle.Full - _missionScrollbar.Info.Width - (10f, 0f));
		_missionContainer.SetVerticalScrollbar(_missionScrollbar);
		_panel.Register(_missionScrollbar);

		_missionPanel.Info.Top.SetValue(_headshot.Info.Top - (20f, 0f));
		_missionPanel.Info.Left.SetValue(_description.Info.Left * 2f + _description.Info.Width);
		_missionPanel.Info.Width.SetValue(PositionStyle.Full - _description.Info.Left * 2f - _description.Info.Width - _missionScrollbar.Info.Width - (PositionStyle.Full - _missionScrollbar.Info.Width - _missionScrollbar.Info.Left) * 2f);
		_missionPanel.Info.Height.SetValue(_description.Info.Top + _description.Info.Height - _missionPanel.Info.Top);
		_missionPanel.PanelColor = GetThemeColor(ColorType.Dark, ColorStyle.Dark);

		_missionScrollbar.Info.Height.SetValue(_missionPanel.Info.Height);
		_missionScrollbar.Info.Top.SetValue(_missionPanel.Info.Top);

		_textScrollbal = new UITextVerticalScrollbar();
		_textScrollbal.Info.Height.SetValue(-16f, 1f);
		_textScrollbal.Info.SetToCenter();
		_textScrollbal.Info.Left.SetValue(-8f, 1f);
		_description.Register(_textScrollbal);

		_descriptionContainer = new UIContainerPanel();
		_descriptionContainer.Info.Width.SetValue(PositionStyle.Full - _textScrollbal.Info.Width -
			(PositionStyle.Full - _textScrollbal.Info.Left - _textScrollbal.Info.Width) * 3f);
		_descriptionContainer.Info.Height.SetValue(_textScrollbal.Info.Height);
		_descriptionContainer.Info.Left.SetValue(PositionStyle.Full - _textScrollbal.Info.Left - _textScrollbal.Info.Width);
		_descriptionContainer.Info.Top.SetValue(_textScrollbal.Info.Top);
		_description.Register(_descriptionContainer);

		_closeButton = new UIBlock();
		_closeButton.Info.Width.SetValue(36f, 0f);
		_closeButton.Info.Height.SetValue(22f, 0f);
		_closeButton.Info.Left.SetValue(PositionStyle.Full - _closeButton.Info.Width - (2f, 0f));
		_closeButton.Info.Top.SetValue(6f, 0f);
		_closeButton.PanelColor = GetThemeColor();
		_closeButton.BorderColor = GetThemeColor(ColorType.Light, ColorStyle.Normal);
		_closeButton.Info.IsSensitive = true;
		_closeButton.Events.OnLeftClick += e => Close();
		_panel.Register(_closeButton);

		_close = new UIImage(ModAsset.MissionClose.Value, Color.White);
		_close.Info.SetToCenter();
		_closeButton.Register(_close);

		_maximizeButton = new UIBlock();
		_maximizeButton.Info.Width.SetValue(_closeButton.Info.Width);
		_maximizeButton.Info.Height.SetValue(_closeButton.Info.Height);
		_maximizeButton.Info.Left.SetValue(_closeButton.Info.Left - _maximizeButton.Info.Width - (2f, 0f));
		_maximizeButton.Info.Top.SetValue(_closeButton.Info.Top);
		_maximizeButton.PanelColor = GetThemeColor();
		_maximizeButton.BorderColor = GetThemeColor(ColorType.Light, ColorStyle.Normal);
		_panel.Register(_maximizeButton);

		_maximization = new UIImage(ModAsset.MissionMaximization.Value, Color.White);
		_maximization.Info.SetToCenter();
		_maximizeButton.Register(_maximization);

		_changeMission = new UIBlock();
		_changeMission.Info.Width.SetValue(48f, 0f);
		_changeMission.Info.Height.SetValue(26f, 0f);
		_changeMission.Info.Left.SetValue(_description.Info.Left + (_description.Info.Width - _changeMission.Info.Width) / 2f);
		_changeMission.Info.Top.SetValue(_description.Info.Top + _description.Info.Height +
			((PositionStyle.Full - _description.Info.Top - _description.Info.Height) - _changeMission.Info.Height) / 2f);
		_changeMission.Info.IsSensitive = true;
		_changeMission.PanelColor = Instance.GetThemeColor();
		_changeMission.Events.OnLeftClick += e =>
		{
			if (SelectedItem != null)
			{
				if (SelectedItem.Mission.PoolType == PoolType.BeenTaken)
				{
					_yes.Info.IsVisible = _no.Info.IsVisible = true;
				}
				else if (SelectedItem.Mission.PoolType == PoolType.CanTaken)
				{
					MissionManager.Instance.MoveMission(SelectedItem.Mission, PoolType.CanTaken, PoolType.BeenTaken);
					ChangeSelectedItem(SelectedItem);
				}
			}
		};
		_panel.Register(_changeMission);

		_yes = new UITextPlus("是");
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
			if (SelectedItem != null && SelectedItem.Mission.PoolType == PoolType.BeenTaken)
			{
				MissionManager.Instance.MoveMission(SelectedItem.Mission, PoolType.BeenTaken, PoolType.Fail);
				ChangeSelectedItem(SelectedItem);
				_yes.Info.IsVisible = _no.Info.IsVisible = false;
			}
		};
		_changeMission.Register(_yes);

		_no = new UITextPlus("否");
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

		_changeText = new UITextPlus("接取");
		_changeText.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
		_changeText.StringDrawer.Init(_changeText.Text);
		_changeText.Events.OnUpdate += (e, gt) =>
		{
			_changeText.Info.SetToCenter();
			_changeText.Calculation();
		};
		_changeMission.Register(_changeText);
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);
		Calculation();
	}

	public override void Show(params object[] args)
	{
		base.Show(args);
		RefreshList();
	}

	public void RefreshList()
	{
		List<BaseElement> elements = [];
		PositionStyle top = (2f, 0f);
		for (int i = 0; i < Enum.GetValues<PoolType>().Length; i++)
		{
			var mp = MissionManager.Instance.GetMissionPool((PoolType)i);
			foreach (var m in mp)
			{
				var element = (BaseElement)Activator.CreateInstance(m.BindingUIItem, [m]);
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
		}
		ChangeSelectedItem(null);
		_missionContainer.ClearAllElements();
		_missionContainer.AddElements(elements);
	}

	public void ChangeSelectedItem(UIMissionItem item)
	{
		SelectedItem = item;
		if (SelectedItem != null)
		{
			_icon.Texture = SelectedItem.Mission.Icon;
			_textScrollbal.WheelValue = 0f;

			UITextPlus des = new UITextPlus(SelectedItem.Mission.Description);
			des.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
			des.StringDrawer.Init(des.Text);
			_descriptionContainer.ClearAllElements();
			_descriptionContainer.AddElement(des);

			if (item.Mission.PoolType == PoolType.CanTaken)
			{
				_changeText.Text = "接取";
			}
			else if (item.Mission.PoolType == PoolType.BeenTaken)
			{
				_changeText.Text = "放弃";
			}
			else
			{
				_changeText.Text = "[TextDrawer,Text='接取',Color='126,126,126']";
			}
			_changeText.Info.SetToCenter();

			_yes.Info.IsVisible = _no.Info.IsVisible = false;
		}
		else
		{
			_icon.Texture = null;
			_textScrollbal.WheelValue = 0f;
			_changeText.Text = "[TextDrawer,Text='接取',Color='126,126,126']";
			_changeText.Info.SetToCenter();
			_descriptionContainer.ClearAllElements();
			_yes.Info.IsVisible = _no.Info.IsVisible = false;
		}
	}
}