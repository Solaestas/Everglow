using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;
using static Everglow.Commons.Mechanics.MissionSystem.Core.MissionManager;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

/// <summary>
/// The filter bar of mission panel
/// </summary>
public class UIMissionStatusFilter : UIBlock
{
	public event EventHandler<PoolType?> StatusFilterChanged;

	private const string AllStatus = "All";
	private BaseElement selectedStatusItem = null;

	private PoolType? poolType = null;

	public PoolType? PoolType => poolType;

	public override void OnInitialization()
	{
		base.OnInitialization();

		var types = new List<PoolType?>() { null }
			.Concat(Enum.GetValues<PoolType>()
				.Select(x => (PoolType?)x))
			.ToArray();

		float width = MathF.Abs(Info.Width.Pixel) / types.Length;
		int index = 0;

		foreach (var type in types)
		{
			var statusFilterItem = new UIBlock();
			ResetPanelColor(statusFilterItem);
			statusFilterItem.Info.IsSensitive = true;
			statusFilterItem.Info.Width.SetValue(0, 1f / types.Length);
			statusFilterItem.Info.Height.SetValue(0, 1f);
			statusFilterItem.Info.Left.SetValue(0, 1f / types.Length * index++);
			statusFilterItem.Info.Top.SetValue(0, 0f);
			statusFilterItem.Events.OnLeftDown += e =>
			{
				ChangeStatus(type, e);
			};
			statusFilterItem.Events.OnMouseHover += e =>
			{
				MissionContainer.Instance.MouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedStatusItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOver += e =>
			{
				MissionContainer.Instance.MouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedStatusItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOut += e =>
			{
				MissionContainer.Instance.MouseText = string.Empty;
				if (statusFilterItem != selectedStatusItem)
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

	private static Texture2D IconPicker(PoolType? type) => type switch
	{
		Enums.PoolType.Accepted => ModAsset.MissionStatus_Accepted.Value,
		Enums.PoolType.Available => ModAsset.MissionStatus_Available.Value,
		Enums.PoolType.Completed => ModAsset.MissionStatus_Completed.Value,
		Enums.PoolType.Overdue => ModAsset.MissionStatus_Failed.Value,
		Enums.PoolType.Failed => ModAsset.MissionStatus_Failed.Value,
		null => ModAsset.MissionStatus_All.Value,
		_ => ModAsset.MissionStatus.Value,
	};

	private static void ResetPanelColor(BaseElement e)
	{
		if (e is not UIBlock block)
		{
			return;
		}
		block.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Light);
	}

	public void ChangeStatus(PoolType? type, BaseElement e)
	{
		UpdateBlockColor(e);

		// Update mission list
		poolType = type;
		NeedRefresh = true;
	}

	private void UpdateBlockColor(BaseElement e)
	{
		if (e is not UIBlock block)
		{
			return;
		}

		// Reset appearance of selected item
		if (selectedStatusItem != null)
		{
			ResetPanelColor(selectedStatusItem);
			(selectedStatusItem as UIBlock).ShowBorder.BottomBorder = true;
		}

		// Change appearance of new selected item
		selectedStatusItem = block;
		block.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Dark);
		block.ShowBorder.BottomBorder = false;
	}
}