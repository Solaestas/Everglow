using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.UI.StringDrawerSystem;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.TextDrawers;
using FontStashSharp;

namespace Everglow.Commons.Mechanics.Mission.UI.Drawers;

internal class TimerStringDrawer : TextDrawer
{
	public string MissionName;
	public int TimerStyle = 0;

	protected override Vector2 GetTextSize(string text)
	{
		if (!TryGetMission(out MissionView mission))
			return Vector2.Zero;
		if (!mission.TimeLimit.HasValue)
		{
			text = "Indefinitely";
		}
		else
		{
			var time = new TimeSpan(0, 0, mission.RemainingTime.Value / 60);
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

	}

	public override void Draw(SpriteBatch sb)
	{
		if (!TryGetMission(out MissionView mission))
			return;
		var pos = Position;
		var text = "Indefinitely";
		if (mission.TimeLimit.HasValue)
		{
			var time = new TimeSpan(0, 0, mission.RemainingTime.Value / 60);
			text = $"{(int)time.TotalMinutes}Min {time.Seconds}s";
		}
		sb.DrawString(Font, text, Position + Offset, Color, Scale, Rotation,
			Origin, LayerDepth, CharacterSpacing, 0, TextStyle,
			FontSystemEffect, EffectAmount);
	}

	private bool TryGetMission(out MissionView mission)
	{
		mission = null;
		MissionPresentationService service = MissionContainer.Service;
		if (service is null)
		{
			return false;
		}

		mission = service.GetAll()
			.FirstOrDefault(entry => entry.View.Identity.Side == MissionSide.Player && entry.View.Identity.DefinitionId == MissionName)
			?.View;
		return mission is not null;
	}
}
