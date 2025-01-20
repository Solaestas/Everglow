using Everglow.Commons.MissionSystem;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.TextDrawers;
using FontStashSharp;

namespace Everglow.Commons.UI.StringDrawerSystem.DrawerItems.MissionDrawer;

internal class TimerStringDrawer : TextDrawer
{
	public string MissionName;
	public int TimerStyle = 0;
	private MissionBase _mission;

	protected override Vector2 GetTextSize(string text)
	{
		if (_mission == null)
			return Vector2.Zero;
		if (_mission.TimeMax < 0)
		{
			text = "Indefinitely";
		}
		else
		{
			var time = new TimeSpan(0, 0, (int)((_mission.TimeMax - _mission.Time) / 60));
			text = $"{(int)time.TotalMinutes}Min {time.Seconds}s";
		}
		return base.GetTextSize(text);
	}

	public override void Init(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
	{
		base.Init(stringDrawer, originalText, name, stringParameters);
		if (stringParameters == null)
			return;
		MissionName = stringParameters.GetString("MissionName",
			stringDrawer.DefaultParameters.GetString("MSTMissionName", string.Empty));
		TimerStyle = stringParameters.GetInt("TimerStyle",
			stringDrawer.DefaultParameters.GetInt("MSTTimerStyle", 0));

		_mission = MissionManager.GetMission(MissionName);
	}

	public override void Draw(SpriteBatch sb)
	{
		if (_mission == null)
			return;
		var pos = Position;
		var text = "Indefinitely";
		if (_mission.TimeMax >= 0)
		{
			var time = new TimeSpan(0, 0, (int)((_mission.TimeMax - _mission.Time) / 60));
			text = $"{(int)time.TotalMinutes}Min {time.Seconds}s";
		}
		sb.DrawString(Font, text, Position + Offset, Color, Scale, Rotation,
			Origin, LayerDepth, CharacterSpacing, 0, TextStyle,
			FontSystemEffect, EffectAmount);
	}
}