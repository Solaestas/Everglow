using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail;

public class UIMissionOperationFail : UIMissionDetailMaskContentBase<UIMissionDetailTipContent>
{
	private static readonly Color ButtonInitialColor = new Color(0.0f, 0.0f, 0.0f, 0.1f);
	private static readonly Color ButtonHoverColor = new Color(0f, 0f, 0f, 0.5f);

	private string _tipTextStr;
	private string _yesTextStr;
	private Action<PlayerMissionBase> _yesAction;

	private UIBlock _main;

	private UIMissionBlock _tip;
	private UITextPlus _tipText;

	private UIMissionButton _yes;
	private UITextPlus _yesText;

	public UIMissionOperationFail()
	{
	}

	public UIMissionOperationFail(PlayerMissionBase? mission, string tipText, Action<PlayerMissionBase> yes = null, string yesText = null)
	{
		_mission = mission;
		_tipTextStr = tipText;
		_yesTextStr = yesText;
		_yesAction = yes;
	}

	public override void OnInitialization()
	{
		base.OnInitialization();
		Info.HiddenOverflow = true;
		Info.SetMargin(0);

		var scale = MissionContainer.Scale;

		_main = new UIBlock();
		_main.PanelColor = Color.Transparent;
		Register(_main);
		_main.Info.Width.SetFull();
		_main.Info.Height.SetFull();
		_main.Info.SetToCenter();

		_tip = new UIMissionBlock();
		_tip.Info.Width.SetValue(320);
		_tip.Info.Height.SetValue(200);
		_tip.Info.Left.SetValue(-_tip.Info.Width.Pixel * 0.5f, 0.5f);
		_tip.Info.Top.SetValue(-120, 0.5f);
		_tip.PanelColor = ButtonHoverColor;
		_tip.Info.SetMargin(0);
		_tip.Info.HiddenOverflow = true;
		_main.Register(_tip);

		_tipText = new UITextPlus(_tipTextStr ?? "你好！");
		_tipText.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * scale);
		_tipText.StringDrawer.Init(_tipText.Text);
		_tipText.StringDrawer.SetWordWrap(_tip.Info.Width.Pixel);
		_tipText.Info.Width.SetFull();
		_tipText.Info.Height.SetFull();
		_tipText.Info.SetMargin(5 * scale);
		_tip.Register(_tipText);

		_yes = NewButton();
		_yes.Events.OnLeftClick += e =>
		{
			_yesAction?.Invoke(_mission);
			Hide(e);
		};
		_yes.Events.OnMouseHover += e =>
		{
			_yes.OnSelect = true;
		};
		_yes.Events.OnMouseOut += e =>
		{
			_yes.OnSelect = false;
		};
		_main.Register(_yes);

		_yesText = new UITextPlus(_yesTextStr ?? "OK");
		_yesText.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * scale);
		_yesText.StringDrawer.Init(_yesText.Text);
		_yes.Register(_yesText);
		_yesText.Info.SetToCenter();
	}

	public UIMissionButton NewButton()
	{
		var button = new UIMissionButton();
		button.Info.Width.SetValue(120);
		button.Info.Height.SetValue(40);
		button.Info.Left.SetValue(Info.HitBox.Width / 2f - 120 - 40);
		button.Info.Top.SetValue(Info.HitBox.Height * 0.63f, 0);
		button.Info.SetMargin(0);
		button.Info.IsSensitive = true;
		button.PanelColor = Color.White;
		return button;
	}

	public override void Calculation()
	{
		_main.Info.Width.SetFull();
		_main.Info.Height.SetFull();
		_main.Info.SetToCenter();

		_tip.Info.Width.SetValue(320);
		_tip.Info.Height.SetValue(200);
		_tip.Info.Left.SetValue(-_tip.Info.Width.Pixel * 0.5f, 0.5f);
		_tip.Info.Top.SetValue(-120, 0.5f);

		_tipText.Info.Width.SetValue(-48, 1);
		_tipText.Info.Height.SetValue(-48, 1);
		_tipText.Info.SetToCenter();

		_yes.Info.Width.SetValue(120);
		_yes.Info.Height.SetValue(40);
		_yes.Info.Left.SetValue(Info.HitBox.Width / 2f - 60);
		_yes.Info.Top.SetValue(Info.HitBox.Height * 0.63f, 0);
		_yes.PanelColor = Color.White;

		if (_yes.OnSelect)
		{
			_yesText.Text = $"[TextDrawer,Text='{_yesTextStr}',Color='{"255,245,193"}']";
		}
		if (!_yes.OnSelect)
		{
			_yesText.Text = $"[TextDrawer,Text='{_yesTextStr}',Color='{"45,38,33"}']";
		}
		base.Calculation();
	}

	public override void Draw(SpriteBatch sb)
	{
		Texture2D tex = ModAsset.MissionFail_Icon.Value;
		int timeTick = (int)((Main.GlobalTimeWrappedHourly * 20f) % 10);
		var frame = new Rectangle(0, 720 * timeTick, 720, 720);
		sb.Draw(tex, HitBox.Center(), frame, Color.White * 0.25f, 0, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);

		Texture2D band = ModAsset.MissionFail_Stripe.Value;
		frame = new Rectangle((int)(Main.GlobalTimeWrappedHourly * 40), 0, HitBox.Width, 78);
		sb.Draw(band, HitBox.Center() + new Vector2(0, HitBox.Height * 0.5f - 40), frame, Color.White * 0.25f, 0, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		sb.Draw(band, HitBox.Center() - new Vector2(0, HitBox.Height * 0.5f - 40), frame, Color.White * 0.25f, 0, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		base.Draw(sb);
	}

	protected override void DrawChildren(SpriteBatch sb)
	{
		base.DrawChildren(sb);
	}
}
