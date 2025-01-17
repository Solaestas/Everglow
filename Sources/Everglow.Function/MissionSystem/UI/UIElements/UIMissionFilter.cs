using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;
using static Everglow.Commons.MissionSystem.MissionManager;
using ReLogic.Graphics;

namespace Everglow.Commons.UI.UIContainers.Mission.UIElements;

/// <summary>
/// The filter bar of mission panel
/// </summary>
public class UIMissionFilter : UIBlock
{
	public event EventHandler<PoolType?> StatusFilterChanged;

	private const string AllStatus = "All";
	private BaseElement selectedStatusItem = null;
	private string mouseText = string.Empty;
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
				mouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedStatusItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOver += e =>
			{
				mouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedStatusItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOut += e =>
			{
				mouseText = string.Empty;
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
		MissionSystem.MissionManager.PoolType.Accepted => ModAsset.MissionStatus_Accepted.Value,
		MissionSystem.MissionManager.PoolType.Available => ModAsset.MissionStatus_Available.Value,
		MissionSystem.MissionManager.PoolType.Completed => ModAsset.MissionStatus_Completed.Value,
		MissionSystem.MissionManager.PoolType.Overdue => ModAsset.MissionStatus_Failed.Value,
		MissionSystem.MissionManager.PoolType.Failed => ModAsset.MissionStatus_Failed.Value,
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
		StatusFilterChanged.Invoke(this, type);
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

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		// Draw filter item tooltip
		if (!string.IsNullOrEmpty(mouseText))
		{
			var pos = Main.MouseScreen + new Vector2(10f, 18f);
			var textSize = FontAssets.MouseText.Value.MeasureString(mouseText);

			if (pos.X + textSize.X > Main.screenWidth)
			{
				pos.X = Main.screenWidth - textSize.X;
			}
			if (pos.Y + textSize.Y > Main.screenHeight)
			{
				pos.Y = Main.screenHeight - textSize.Y;
			}
			if (pos.X < 0)
			{
				pos.X = 0;
			}
			if (pos.Y < 0)
			{
				pos.Y = 0;
			}

			var PanelColor = new Color(191, 106, 106);
			Texture2D texture = ModAsset.Panel.Value;
			Point textureSize = new Point(texture.Width, texture.Height);
			Rectangle rectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)textSize.X, (int)textSize.Y);

			// Draw 4 corners
			sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y), new Rectangle(0, 0, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y), new Rectangle(textureSize.X - 6, 0, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y + rectangle.Height - 6), new Rectangle(0, textureSize.Y - 6, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y + rectangle.Height - 6), new Rectangle(textureSize.X - 6, textureSize.Y - 6, 6, 6), PanelColor);

			// Draw main part
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y, rectangle.Width - 12, 6), new Rectangle(6, 0, textureSize.X - 12, 6), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + rectangle.Height - 6, rectangle.Width - 12, 6), new Rectangle(6, textureSize.Y - 6, textureSize.X - 12, 6), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(0, 6, 6, textureSize.Y - 12), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - 6, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(textureSize.X - 6, 6, 6, textureSize.Y - 12), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + 6, rectangle.Width - 12, rectangle.Height - 12), new Rectangle(6, 6, textureSize.X - 12, textureSize.Y - 12), PanelColor);

			// Draw text
			sb.DrawString(FontAssets.MouseText.Value, mouseText, pos + new Vector2(0f, 5f), Color.Cyan);

			mouseText = string.Empty;
		}
	}
}