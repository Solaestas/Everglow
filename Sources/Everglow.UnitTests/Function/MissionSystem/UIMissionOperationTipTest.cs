using System.Collections;
using System.Reflection;
using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.UI;
using Everglow.Commons.Mechanics.Mission.UI.UIElements;
using Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail;
using Everglow.Commons.UI;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
[DoNotParallelize]
public class UIMissionOperationTipTest
{
	private static readonly FieldInfo UISystemInstanceField = typeof(UISystem).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic)!;

	private bool _originalDedServ;
	private UISystem _originalUISystem;
	private MissionContainer _missionContainer;
	private FontManager _fontManager;

	[TestInitialize]
	public void Initialize()
	{
		Terraria.Program.SavePath = string.Empty;
		_originalDedServ = Terraria.Main.dedServ;
		Terraria.Main.dedServ = true;

		_originalUISystem = UISystem.Instance;
		_ = new UISystem();
		_missionContainer = new MissionContainer();
		UISystem.EverglowUISystem.Elements.Add(typeof(MissionContainer).FullName!, _missionContainer);

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
		var fonts = (IDictionary)typeof(FontManager).GetField("_fonts", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(_fontManager)!;
		fonts.Add("FusionPixel12", fontSystem);
	}

	[TestCleanup]
	public void Cleanup()
	{
		_missionContainer?.Unload();
		_fontManager?.Unload();
		UISystemInstanceField.SetValue(null, _originalUISystem);
		Terraria.Main.dedServ = _originalDedServ;
	}

	[TestMethod]
	public void ConfirmationPassesPresentationEntryToCallback()
	{
		var identity = new MissionIdentity(MissionSide.Player, "TestMission", "instance");
		var entry = new MissionPresentationEntry(
			new MissionView { Identity = identity },
			[new MissionAction(identity, MissionActionType.Cancel)]);
		MissionPresentationEntry receivedEntry = null;
		var tip = new UIMissionOperationTip(
			entry,
			UIMissionOperationTip.TipType.Confirmation,
			"Confirm",
			value => receivedEntry = value);
		tip.OnInitialization();

		var yesButton = (UIMissionButton)typeof(UIMissionOperationTip)
			.GetField("_yes", BindingFlags.Instance | BindingFlags.NonPublic)!
			.GetValue(tip)!;
		yesButton.Events.LeftClick(yesButton);

		Assert.AreSame(entry, receivedEntry);
	}
}
