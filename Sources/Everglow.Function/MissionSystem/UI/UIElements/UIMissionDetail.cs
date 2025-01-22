using Everglow.Commons.MissionSystem.Enums;
using Everglow.Commons.UI.UIContainers.Mission.UIElements;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.MissionSystem.UI.MissionContainer;

namespace Everglow.Commons.MissionSystem.UI.UIElements;

public class UIMissionDetail : BaseElement
{
	public event EventHandler OnClickChange;

	public event EventHandler OnClickYes;

	public event EventHandler OnClickNo;

	private UIBlock _headshot;
	private UIMissionIcon _icon;

	private UIBlock _description;
	private UIContainerPanel _descriptionContainer;
	private UITextVerticalScrollbar _textScrollbar;

	private UIBlock _changeMission;
	private UITextPlus _changeText;
	private UITextPlus _yes;
	private UITextPlus _no;

	public UIMissionItem SelectedItem => MissionContainer.Instance.SelectedItem;

	public override void OnInitialization()
	{
		base.OnInitialization();

		var headshotSize = Info.Width * 0.4f;
		_headshot = new UIBlock();
		_headshot.Info.Width.SetValue(headshotSize);
		_headshot.Info.Height.SetValue(headshotSize);
		_headshot.Info.Left.SetValue((Info.Width - _headshot.Info.Width) / 2);
		_headshot.PanelColor = MissionContainer.Instance.GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		Register(_headshot);

		_icon = new UIMissionIcon(null, Color.White);
		_icon.Info.Width.SetFull();
		_icon.Info.Height.SetFull();
		_headshot.Register(_icon);

		_description = new UIBlock();
		_description.Info.Top.SetValue(_headshot.Info.Top + _headshot.Info.Height + Info.Height * 0.05f);
		_description.Info.Width.SetFull();
		_description.Info.Height.SetValue(PositionStyle.Full - _description.Info.Top - Info.Height * 0.15f);
		_description.PanelColor = MissionContainer.Instance.GetThemeColor(ColorType.Dark, ColorStyle.Dark);
		Register(_description);

		_textScrollbar = new UITextVerticalScrollbar();
		_textScrollbar.Info.Height.SetValue(-16f, 1f);
		_textScrollbar.Info.SetToCenter();
		_textScrollbar.Info.Left.SetValue(-8f, 1f);
		_description.Register(_textScrollbar);

		_descriptionContainer = new UIContainerPanel();
		_descriptionContainer.Info.Width.SetValue(PositionStyle.Full - _textScrollbar.Info.Width - (PositionStyle.Full - _textScrollbar.Info.Left - _textScrollbar.Info.Width) * 3f);
		_descriptionContainer.Info.Height.SetValue(_textScrollbar.Info.Height);
		_descriptionContainer.Info.Left.SetValue(PositionStyle.Full - _textScrollbar.Info.Left - _textScrollbar.Info.Width);
		_descriptionContainer.Info.Top.SetValue(_textScrollbar.Info.Top);
		_descriptionContainer.SetVerticalScrollbar(_textScrollbar);
		_description.Register(_descriptionContainer);

		_changeMission = new UIBlock();
		_changeMission.Info.Width.SetValue(Info.Width * 0.18f);
		_changeMission.Info.Height.SetValue(Info.Height * 0.07f);
		_changeMission.Info.Left.SetValue(_description.Info.Left + (_description.Info.Width - _changeMission.Info.Width) / 2f);
		_changeMission.Info.Top.SetValue(_description.Info.Top + _description.Info.Height +
			(PositionStyle.Full - _description.Info.Top - _description.Info.Height - _changeMission.Info.Height) / 2f);
		_changeMission.Info.IsSensitive = true;
		_changeMission.PanelColor = MissionContainer.Instance.GetThemeColor();
		_changeMission.Events.OnLeftDown += e =>
		{
			OnClickChange.Invoke(this, null);
		};
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
		_yes.Events.OnLeftDown += e =>
		{
			OnClickYes.Invoke(this, null);
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
		_no.Events.OnLeftDown += e =>
		{
			HideYesNo();
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
	}

	public void SetMissionDetail(UIMissionItem missionItem)
	{
		if (missionItem != null)
		{
			_icon.SetIconGroup(missionItem.Mission.Icon);
			_textScrollbar.WheelValue = 0f;

			var des = new UITextPlus(missionItem.Mission.Description);
			des.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
			des.StringDrawer.Init(des.Text);
			_descriptionContainer.ClearAllElements();
			_descriptionContainer.AddElement(des);
			des.StringDrawer.SetWordWrap(_descriptionContainer.HitBox.Width - _textScrollbar.InnerScale.X);
		}
		else
		{
			_icon.SetIconGroup(null);
			_textScrollbar.WheelValue = 0f;
			_descriptionContainer.ClearAllElements();
		}
	}

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