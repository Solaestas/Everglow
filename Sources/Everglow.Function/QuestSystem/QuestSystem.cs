using Everglow.Commons.UI;
using Everglow.Commons.UI.UIContainers.Sidebar;
using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.QuestSystem;

public class QuestSystem : ModSystem
{
	public List<Quest> Quests;

	public override void SaveWorldData(TagCompound tag)
	{
		base.SaveWorldData(tag);
	}

	public override void LoadWorldData(TagCompound tag)
	{
		base.LoadWorldData(tag);
	}

	public override void PostUpdateEverything()
	{
	}

	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
	}
}

public class QuestSystemInterface : UIContainerElement
{
	public static readonly string ContainerFullName = typeof(SidebarContainer).FullName;
	private UIPanel mainPanel;
	private bool open = true;
	private KeyCooldown cardModeVisibleCoolDown = new KeyCooldown(() => true, 14);

	public override bool IsVisible => !Main.playerInventory;

	private string mouseText = string.Empty;

	public override void OnInitialization()
	{
		base.OnInitialization();

		mainPanel = new UIPanel();
		mainPanel.Info.Width.SetValue(Main.screenWidth, 0f);
		mainPanel.Info.Height.SetValue(Main.screenHeight, 0f);
		mainPanel.Info.Top.SetValue(0f, 0f);
		mainPanel.Info.Left.SetValue(0f, 0f);
		mainPanel.Info.SetMargin(0f);
		Register(mainPanel);
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);

		cardModeVisibleCoolDown.Update();

		open = Main.LocalPlayer.HeldItem.type == ModContent.ItemType<QuestTerminal>();
		mainPanel.Calculation();
	}

	public override void Draw(SpriteBatch sb)
	{
		if(!open)
		{
			return;
		}
		base.Draw(sb);

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

			// 绘制四个角
			sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y), new Rectangle(0, 0, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y), new Rectangle(textureSize.X - 6, 0, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X, rectangle.Y + rectangle.Height - 6), new Rectangle(0, textureSize.Y - 6, 6, 6), PanelColor);
			sb.Draw(texture, new Vector2(rectangle.X + rectangle.Width - 6, rectangle.Y + rectangle.Height - 6), new Rectangle(textureSize.X - 6, textureSize.Y - 6, 6, 6), PanelColor);

			// 绘制本体
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y, rectangle.Width - 12, 6), new Rectangle(6, 0, textureSize.X - 12, 6), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + rectangle.Height - 6, rectangle.Width - 12, 6), new Rectangle(6, textureSize.Y - 6, textureSize.X - 12, 6), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(0, 6, 6, textureSize.Y - 12), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - 6, rectangle.Y + 6, 6, rectangle.Height - 12), new Rectangle(textureSize.X - 6, 6, 6, textureSize.Y - 12), PanelColor);
			sb.Draw(texture, new Rectangle(rectangle.X + 6, rectangle.Y + 6, rectangle.Width - 12, rectangle.Height - 12), new Rectangle(6, 6, textureSize.X - 12, textureSize.Y - 12), PanelColor);

			mouseText = string.Empty;
		}
	}
}