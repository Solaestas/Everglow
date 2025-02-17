using Everglow.Commons.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;
using ReLogic.Graphics;
using Terraria.GameContent;
using static Everglow.Commons.MissionSystem.MissionManager;

namespace Everglow.Commons.MissionSystem.UI.UIElements;

public class UIMissionTypeFilter : UIBlock
{
	private const string AllStatus = "All";
	private BaseElement selectedTypeItem = null;
	private string mouseText = string.Empty;
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
				mouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedTypeItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOver += e =>
			{
				mouseText = type?.ToString() ?? AllStatus;
				if (statusFilterItem != selectedTypeItem)
				{
					statusFilterItem.PanelColor = Color.Gray;
				}
			};
			statusFilterItem.Events.OnMouseOut += e =>
			{
				mouseText = string.Empty;
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
			var textureSize = new Point(texture.Width, texture.Height);
			var rectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)textSize.X, (int)textSize.Y);

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