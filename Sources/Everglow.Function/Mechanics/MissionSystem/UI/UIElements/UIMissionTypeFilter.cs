using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.Mechanics.MissionSystem.Core.MissionManager;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionTypeFilter : UIBlock
{
	private const string AllStatus = "All";
	private BaseElement selectedTypeItem = null;
	private MissionType? missionType = null;

	public MissionType? MissionType => missionType;

	public override void OnInitialization()
	{
		var missionTypes = new List<MissionType?>() { null }
			.Concat(Enum.GetValues<MissionType>()
				.Select(x => (MissionType?)x))
			.ToArray();
		var width = MathF.Abs(Info.Width.Pixel) / missionTypes.Length;
		var index = 0;
		foreach (var type in missionTypes)
		{
			var statusFilterItem = new UIBlock();
			ResetPanelColor(statusFilterItem);
			statusFilterItem.Info.IsSensitive = true;
			statusFilterItem.Info.Width.SetValue(0, 1f / missionTypes.Length);
			statusFilterItem.Info.Height.SetValue(0, 1f);
			statusFilterItem.Info.Left.SetValue(0, 1f / missionTypes.Length * index++);
			statusFilterItem.Info.Top.SetValue(0, 0f);
			statusFilterItem.Events.OnLeftDown += e =>
			{
				ChangeType(type, e);
			};
			statusFilterItem.Events.OnMouseHover += e =>
			{
				MissionContainer.Instance.MouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedTypeItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOver += e =>
			{
				MissionContainer.Instance.MouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedTypeItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOut += e =>
			{
				MissionContainer.Instance.MouseText = string.Empty;
				if (statusFilterItem != selectedTypeItem)
				{
					ResetPanelColor(e);
				}
			};
			Register(statusFilterItem);

			var statusFilterItemIcon = new UIImage(IconPicker(type), Color.White);
			statusFilterItem.Register(statusFilterItemIcon);
			statusFilterItemIcon.Info.SetToCenter();
		}

		UpdateBlockColor(ChildrenElements.First());
	}


	public void ChangeType(MissionType? type, BaseElement e)
	{
		UpdateBlockColor(e);

		// Update mission list
		missionType = type;
		NeedRefresh = true;
	}


	private void UpdateBlockColor(BaseElement e)
	{
		if (e is not UIBlock block)
		{
			return;
		}

		// Reset appearance of selected item
		if (selectedTypeItem != null)
		{
			ResetPanelColor(selectedTypeItem);
			(selectedTypeItem as UIBlock).ShowBorder.BottomBorder = true;
		}

		// Change appearance of new selected item
		selectedTypeItem = block;
		block.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Dark);
		block.ShowBorder.BottomBorder = false;
	}

	private static void ResetPanelColor(BaseElement e)
	{
		if (e is not UIBlock block)
		{
			return;
		}
		block.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Light);
	}

	private static Texture2D IconPicker(MissionType? type) => type switch
	{
		Enums.MissionType.MainStory => ModAsset.MissionType_Yellow.Value,
		Enums.MissionType.SideStory => ModAsset.MissionType_Purple.Value,
		Enums.MissionType.Achievement => ModAsset.MissionType_White.Value,
		Enums.MissionType.Challenge => ModAsset.MissionType_Red.Value,
		Enums.MissionType.Daily => ModAsset.MissionType_Blue.Value,
		Enums.MissionType.Legendary => ModAsset.MissionType_Prism.Value,
		null => ModAsset.MissionType_White.Value,
		_ => ModAsset.MissionType_Grey.Value,
	};
}