using System.Collections;
using System.Reflection;
using Everglow.Commons.Mechanics.Quest.Presentation.Views;
using Everglow.Commons.Mechanics.Quest.UI.UIElements;
using Everglow.Commons.Mechanics.Quest.UI.UIElements.QuestDetail;
using Everglow.Commons.UI;

namespace Everglow.UnitTests.Function.QuestSystem;

[TestClass]
[DoNotParallelize]
public class UIQuestObjectiveRetryTest
{
	private FontManager _fontManager;

	[TestInitialize]
	public void Initialize()
	{
		Terraria.Program.SavePath = string.Empty;
		string fontAssemblyPath = Path.GetFullPath(Path.Combine(
			AppContext.BaseDirectory,
			"..", "..", "..", "..", "Everglow", "lib", "FontStashSharp.FNA.dll"));
		Assembly fontAssembly = Assembly.LoadFrom(fontAssemblyPath);
		_fontManager = new FontManager();
		Type fontSystemType = fontAssembly.GetType("FontStashSharp.FontSystem")!;
		object fontSystem = Activator.CreateInstance(fontSystemType)!;
		string fontPath = Path.GetFullPath(Path.Combine(
			AppContext.BaseDirectory,
			"..", "..", "..", "..", "Everglow.Function", "UI", "Fonts", "FusionPixel12.ttf"));
		fontSystemType.GetMethod("AddFont", [typeof(byte[])])!.Invoke(fontSystem, [File.ReadAllBytes(fontPath)]);
		var fonts = (IDictionary)typeof(FontManager)
			.GetField("_fonts", BindingFlags.Instance | BindingFlags.NonPublic)!
			.GetValue(_fontManager)!;
		fonts.Add("FusionPixel12", fontSystem);
	}

	[TestCleanup]
	public void Cleanup()
	{
		_fontManager?.Unload();
	}

	[TestMethod]
	public void TimerLeftClickPassesObjectiveIdToRetryCallback()
	{
		var objective = new ObjectiveView
		{
			Id = 7,
			State = ObjectiveViewState.TimedOut,
			Timer = new TimerView { TimeLimit = 60, ElapsedTime = 60 },
		};
		var line = new ObjectiveLineView(objective, "Timed objective");
		int? receivedObjectiveId = null;
		var item = new UIQuestObjectiveItem(
			line,
			30f,
			400f,
			objectiveId => receivedObjectiveId = objectiveId);
		var timer = (UIQuestHourglassTimer)typeof(UIQuestObjectiveItem)
			.GetField("_timer", BindingFlags.Instance | BindingFlags.NonPublic)!
			.GetValue(item)!;

		timer.Events.LeftClick(timer);

		Assert.AreEqual(7, receivedObjectiveId);
	}

	[TestMethod]
	public void ReusedItemLeftClickUsesLatestObjectiveId()
	{
		var originalLine = new ObjectiveLineView(
			new ObjectiveView
			{
				Id = 3,
				State = ObjectiveViewState.TimedOut,
				Timer = new TimerView { TimeLimit = 60, ElapsedTime = 60 },
			},
			"Original objective");
		var latestLine = new ObjectiveLineView(
			new ObjectiveView
			{
				Id = 9,
				State = ObjectiveViewState.TimedOut,
				Timer = new TimerView { TimeLimit = 60, ElapsedTime = 60 },
			},
			"Latest objective");
		int? receivedObjectiveId = null;
		var item = new UIQuestObjectiveItem(
			originalLine,
			30f,
			400f,
			objectiveId => receivedObjectiveId = objectiveId);
		item.SetLine(latestLine);
		var timer = (UIQuestHourglassTimer)typeof(UIQuestObjectiveItem)
			.GetField("_timer", BindingFlags.Instance | BindingFlags.NonPublic)!
			.GetValue(item)!;

		timer.Events.LeftClick(timer);

		Assert.AreEqual(9, receivedObjectiveId);
	}
}
