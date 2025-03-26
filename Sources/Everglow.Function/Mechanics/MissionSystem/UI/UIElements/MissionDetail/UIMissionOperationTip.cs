using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.MissionDetail;

public class UIMissionOperationTip : UIMissionDetailMaskContentBase<UIMissionDetailTipContent>
{
	public enum TipType
	{
		Information,
		Warning,
		Confirmation,
	}

	private UIBlock _main;

	private UIBlock _tip;
	private UITextPlus _tipText;

	private UIBlock _yes;
	private UITextPlus _yesText;

	private UIBlock _no;
	private UITextPlus _noText;

	public UIMissionOperationTip()
	{
	}

	public override void OnInitialization()
	{
		base.OnInitialization();
		Info.HiddenOverflow = true;
		Info.SetMargin(0);

		var scale = MissionContainer.Scale;

		_main = new UIBlock();
		_main.Info.Width.SetValue(360 * scale);
		_main.Info.Height.SetValue(240 * scale);
		_main.PanelColor = Color.Gray;
		Register(_main);
		_main.Info.SetToCenter();

		_tip = new UIBlock();
		_tip.Info.Width.SetValue(300 * scale);
		_tip.Info.Height.SetValue(120 * scale);
		_tip.Info.Top.SetValue(30 * scale);
		_tip.Info.Left.SetValue(30 * scale);
		_tip.PanelColor = Color.Blue;
		_tip.Info.SetMargin(0);
		_main.Register(_tip);

		_tipText = new UITextPlus("你好！");
		_tipText.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * scale);
		_tipText.StringDrawer.Init(_tipText.Text);
		_tipText.Info.Width.SetFull();
		_tipText.Info.Height.SetFull();
		_tip.Register(_tipText);

		_yes = new UIBlock();
		_yes.Info.Width.SetValue(120 * scale);
		_yes.Info.Height.SetValue(40 * scale);
		_yes.Info.Left.SetValue(30 * scale);
		_yes.Info.Top.SetValue(170 * scale);
		_yes.Info.SetMargin(0);
		_yes.Info.IsSensitive = true;
		_yes.Events.OnLeftClick += Hide;
		_main.Register(_yes);

		_yesText = new UITextPlus("OK");
		_yesText.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * scale);
		_yesText.StringDrawer.Init(_yesText.Text);
		_yes.Register(_yesText);
		_yesText.Info.SetToCenter();

		_no = new UIBlock();
		_no.Info.Width.SetValue(120 * scale);
		_no.Info.Height.SetValue(40 * scale);
		_no.Info.Left.SetValue(210 * scale);
		_no.Info.Top.SetValue(170 * scale);
		_no.Info.SetMargin(0);
		_no.Info.IsSensitive = true;
		_no.Events.OnLeftClick += Hide;
		_main.Register(_no);

		_noText = new UITextPlus("NO");
		_noText.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * scale);
		_noText.StringDrawer.Init(_noText.Text);
		_no.Register(_noText);
		_noText.Info.SetToCenter();
	}
}