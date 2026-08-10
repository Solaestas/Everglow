using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class MissionHintTest
{
	private sealed class PlayerHintMission : PlayerMissionBase
	{
		public override string DisplayName => nameof(PlayerHintMission);

		public string HintValue { get; set; } = string.Empty;

		public override string Hint => HintValue;
	}

	private sealed class WorldHintMission : WorldMissionBase
	{
		public string HintValue { get; set; } = string.Empty;

		public override string Hint => HintValue;
	}

	[TestMethod]
	public void MissionBases_DefaultHint_IsEmpty()
	{
		Assert.AreEqual(string.Empty, new DefaultPlayerMission().Hint);
		Assert.AreEqual(string.Empty, new DefaultWorldMission().Hint);
	}

	[TestMethod]
	public void DerivedMissions_CanProvideDynamicHints()
	{
		var playerMission = new PlayerHintMission { HintValue = "player hint" };
		var worldMission = new WorldHintMission { HintValue = "world hint" };

		Assert.AreEqual("player hint", playerMission.Hint);
		Assert.AreEqual("world hint", worldMission.Hint);

		playerMission.HintValue = "updated player hint";
		worldMission.HintValue = "updated world hint";

		Assert.AreEqual("updated player hint", playerMission.Hint);
		Assert.AreEqual("updated world hint", worldMission.Hint);
	}

	[TestMethod]
	public void MaskedHint_HasCanonicalText()
	{
		Assert.AreEqual("???", MissionHintText.Masked);
	}

	private sealed class DefaultPlayerMission : PlayerMissionBase
	{
		public override string DisplayName => nameof(DefaultPlayerMission);
	}

	private sealed class DefaultWorldMission : WorldMissionBase
	{
	}
}
