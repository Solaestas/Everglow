using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Everglow.Commons.TmlHooks;

internal class UIModItemPatch : ILoadable
{
	private List<Action> UnLoadActions = new List<Action>();

	public void Load(Mod mod)
	{
		On_Main.Draw += On_Main_Draw;
	}

	private void On_Main_Draw(On_Main.orig_Draw orig, Main self, GameTime gameTime)
	{
		orig(self, gameTime);
		if (Main.MenuUI._history.Find(x => x is UIMods uiMods && uiMods.items != null) is UIMods uiMods &&
			uiMods.items.Find(x => x._mod.Name == ModIns.Mod.Name) is UIModItem modItem)
		{
			if (modItem.Elements.Find(x => x is ClearCacheButton) is ClearCacheButton clearButton)
			{
				clearButton.Left = new StyleDimension(modItem._uiModStateText.Left.Pixels + modItem._uiModStateText.Width.Pixels + 6f, 0f);
				clearButton.Top = modItem._uiModStateText.Top;
			}
			else
			{
				UIPanel panel = null;

				ClearCacheButton button = new ClearCacheButton();
				button.Left = new StyleDimension(modItem._uiModStateText.Left.Pixels + modItem._uiModStateText.Width.Pixels + 10f, 0f);
				button.Top = modItem._uiModStateText.Top;
				button.OnLeftClick += delegate
				{
					if (panel != null)
						return;
					panel = new UIPanel();
					panel.Width.Set(332f, 0f);
					panel.Height.Set(184f, 0f);
					panel.Left.Set(-panel.Width.Pixels / 2f, 0.5f);
					panel.Top.Set(-panel.Height.Pixels / 2f, 0.5f);
					uiMods.Append(panel);

					UIText title = new UIText(Language.GetTextValue("警告"), 0.6f, true);
					title.Left.Set(-title.MinWidth.Pixels / 2f, 0.5f);
					title.Top.Set(-title.MinHeight.Pixels / 2f, 0.2f);
					title.TextColor = Color.Red;
					panel.Append(title);

					UIText content = new UIText(Language.GetTextValue("此操作会删除缓存文件，是否继续？"));
					content.Left.Set(-content.MinWidth.Pixels / 2f, 0.5f);
					content.Top.Set(-content.MinHeight.Pixels / 2f, 0.5f);
					panel.Append(content);

					UITextPanel<string> yes = new UITextPanel<string>(Language.GetTextValue("确定"));
					var color = yes.BackgroundColor;
					yes.Left.Set(-yes.MinWidth.Pixels / 2f, 0.32f);
					yes.Top.Set(-yes.MinHeight.Pixels / 2f, 0.8f);
					yes.TextColor = Color.Red;
					yes.OnLeftClick += delegate
					{
						uiMods.RemoveChild(panel);
						uiMods.Recalculate();
						panel = null;
						if (!Directory.Exists(ModIns.ModCachePath))
							return;
						var files = Directory.GetFiles(ModIns.ModCachePath);
						foreach (var file in files)
						{
							try
							{
								File.Delete(file);
							}
							catch (Exception e)
							{
								Console.WriteLine(e);
							}
						}
						var paths = Directory.GetDirectories(ModIns.ModCachePath);
						foreach (var path in paths)
						{
							try
							{
								Directory.Delete(path);
							}
							catch (Exception e)
							{
								Console.WriteLine(e);
							}
						}
					};
					yes.OnMouseOver += delegate
					{
						yes.BackgroundColor = Color.Lerp(color, Color.White, 0.3f);
					};
					yes.OnMouseOut += delegate
					{
						yes.BackgroundColor = color;
					};
					panel.Append(yes);

					UITextPanel<string> no = new UITextPanel<string>(Language.GetTextValue("取消"));
					no.Left.Set(-no.MinWidth.Pixels / 2f, 0.68f);
					no.Top.Set(-no.MinHeight.Pixels / 2f, 0.8f);
					no.TextColor = Color.Green;
					no.OnLeftClick += delegate
					{
						uiMods.RemoveChild(panel);
						uiMods.Recalculate();
						panel = null;
					};
					no.OnMouseOver += delegate
					{
						no.BackgroundColor = Color.Lerp(color, Color.White, 0.3f);
					};
					no.OnMouseOut += delegate
					{
						no.BackgroundColor = color;
					};
					panel.Append(no);
					uiMods.Recalculate();
				};
				modItem.Append(button);
				UnLoadActions.Add(() =>
				{
					if (panel != null)
						modItem?.RemoveChild(panel);
				});
				UnLoadActions.Add(() =>
				{
					modItem?.RemoveChild(button);
				});
			}
		}
	}

	public void Unload()
	{
		On_Main.Draw -= On_Main_Draw;

		UnLoadActions.ForEach(a => a());
		UnLoadActions.Clear();
	}

	private class ClearCacheButton : UIElement
	{
		public string DisplayText
		{
			get
			{
				return Language.GetTextValue("清理缓存");
			}
		}

		public Color DisplayColor
		{
			get
			{
				return Color.Red;
			}
		}

		public ClearCacheButton()
		{
			PaddingLeft = (PaddingRight = 5f);
			PaddingBottom = (PaddingTop = 10f);
		}

		public override void Recalculate()
		{
			Vector2 vector = new Vector2(FontAssets.MouseText.Value.MeasureString(DisplayText).X, 16f);
			Width.Set(vector.X + PaddingLeft + PaddingRight, 0f);
			Height.Set(vector.Y + PaddingTop + PaddingBottom, 0f);
			base.Recalculate();
		}

		public override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			DrawPanel(spriteBatch);
			DrawEnabledText(spriteBatch);
		}

		public void DrawPanel(SpriteBatch spriteBatch)
		{
			Vector2 position = GetDimensions().Position();
			float pixels = Width.Pixels;
			spriteBatch.Draw(UICommon.InnerPanelTexture.Value, position, new Rectangle(0, 0, 8, UICommon.InnerPanelTexture.Height()), Color.White);
			spriteBatch.Draw(UICommon.InnerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, UICommon.InnerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2((pixels - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(UICommon.InnerPanelTexture.Value, new Vector2(position.X + pixels - 8f, position.Y), new Rectangle(16, 0, 8, UICommon.InnerPanelTexture.Height()), Color.White);
		}

		public void DrawEnabledText(SpriteBatch spriteBatch)
		{
			Vector2 pos = GetDimensions().Position() + new Vector2(PaddingLeft, PaddingTop * 0.5f);
			Utils.DrawBorderString(spriteBatch, DisplayText, pos, DisplayColor);
		}
	}
}