using Everglow.Commons.Mechanics.Quest.Presentation;
using Everglow.Commons.Mechanics.Quest.Presentation.Views;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.Mechanics.Quest.UI.QuestContainer;

namespace Everglow.Commons.Mechanics.Quest.UI.UIElements.QuestDetail;

public sealed class UIQuestObjectiveItem : BaseElement
{
	private const float TimerWidth = 18f;
	private const float TimerHeight = 34f;
	private const float TimerSpacing = 8f;

	private readonly UITextPlus _text;
	private readonly UIQuestHourglassTimer _timer;

	public UIQuestObjectiveItem(ObjectiveLineView line, float fontSize, float width)
	{
		Info.SetMargin(0f);
		Info.Width.SetValue(Math.Max(1f, width));

		_text = new UITextPlus(string.Empty);
		_text.StringDrawer.DefaultParameters.SetParameter("FontSize", fontSize);
		Register(_text);

		_timer = new UIQuestHourglassTimer();
		_timer.Info.Width.SetValue(TimerWidth);
		_timer.Info.Height.SetValue(TimerHeight);
		_timer.Info.Left.SetValue(-TimerWidth, 1f);
		_timer.Events.OnMouseHover += e =>
		{
			Instance.MouseText = TextDefinition.GetObjectiveTimerTooltip(_timer.Timer);
			_timer.OnSelect = true;
		};
		_timer.Events.OnMouseOut += e => _timer.OnSelect = false;
		Register(_timer);

		SetLine(line);
	}

	public void SetLine(ObjectiveLineView line)
	{
		ArgumentNullException.ThrowIfNull(line);

		TimerView timer = line.Timer;
		_timer.Info.IsVisible = timer is not null;
		_timer.MaxTime = timer?.TimeLimit ?? 0;
		_timer.Timer = timer?.RemainingTime ?? 0;
		if (timer is null)
		{
			_timer.OnSelect = false;
		}

		_text.Text = line.Text;
		float timerSpace = timer is null ? 0f : TimerWidth + TimerSpacing;
		_text.StringDrawer.SetWordWrap(Math.Max(1f, Info.Width.Pixel - timerSpace));
		_text.Calculation();

		float height = Math.Max(_text.Info.Height.Pixel, timer is null ? 0f : TimerHeight);
		Info.Height.SetValue(height);
		_text.Info.Top.SetValue((height - _text.Info.Height.Pixel) * 0.5f);
		_timer.Info.Top.SetValue((height - TimerHeight) * 0.5f);
		Calculation();
	}
}

