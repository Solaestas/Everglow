using Everglow.Commons.TmlHooks.SwitchWorldItems;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Everglow.Commons.TmlHooks;

internal class UIWorldListItemPatch : ILoadable
{
	private List<Action> UnLoadActions = new List<Action>();
	private Dictionary<string, SwitchWorldItemBase> worldSwitchStatus = new Dictionary<string, SwitchWorldItemBase>();
	private static List<SwitchWorldItemBase> SwitchWorldItems;

	public void Load(Mod mod)
	{
		SwitchWorldItems = Ins.ModuleManager.CreateInstances<SwitchWorldItemBase>().ToList();
		SwitchWorldItems.Sort((i1, i2) => i1.Compare(i2));

		On_UIWorldListItem.ctor += On_UIWorldListItem_ctor;
		On_UIWorldListItem.PlayGame += On_UIWorldListItem_PlayGame;
		On_AWorldListItem.UpdateGlitchAnimation += On_AWorldListItem_UpdateGlitchAnimation;
	}

	private void On_UIWorldListItem_PlayGame(On_UIWorldListItem.orig_PlayGame orig, UIWorldListItem self, UIMouseEvent evt, UIElement listeningElement)
	{
		orig(self, evt, listeningElement);
		if (worldSwitchStatus.TryGetValue(self.Data.Path, out SwitchWorldItemBase switchWorldItem))
			switchWorldItem.Enter(self.Data);
	}

	private void On_AWorldListItem_UpdateGlitchAnimation(On_AWorldListItem.orig_UpdateGlitchAnimation orig, AWorldListItem self, UIElement affectedElement)
	{
		if (!worldSwitchStatus.TryGetValue(self.Data.Path, out SwitchWorldItemBase switchWorldItem) ||
			switchWorldItem is not OriginalWorldItem)
			orig(self, affectedElement);
	}

	private void On_UIWorldListItem_ctor(On_UIWorldListItem.orig_ctor orig, UIWorldListItem self, Terraria.IO.WorldFileData data, int orderInList, bool canBePlayed)
	{
		orig(self, data, orderInList, canBePlayed);
		Asset<Texture2D> originAsset = null;
		Rectangle originFrame = default;
		worldSwitchStatus.TryAdd(self.Data.Path, new OriginalWorldItem());
		int switchIndex = 0;
		if (self._worldIcon is UIImageFramed uIImageFramed)
		{
			originAsset = uIImageFramed._texture;
			originFrame = uIImageFramed._frame;
		}
		else if (self._worldIcon is UIImage image)
		{
			originAsset = image._texture;
		}
		foreach (var item in self.Elements)
		{
			if (item is UIImageButton button && button._texture == self._buttonRenameTexture)
			{
				UIImageButton changeButton = new UIImageButton(ModAsset.NextWorld);
				changeButton.Left = button.Left;
				changeButton.Left.Pixels += 24f;
				changeButton.Top = button.Top;
				changeButton.VAlign = button.VAlign;
				changeButton.OnLeftClick += delegate
				{
					switchIndex++;
					switchIndex %= SwitchWorldItems.Count;
					if (!worldSwitchStatus.TryAdd(self.Data.Path, SwitchWorldItems[switchIndex]))
					{
						worldSwitchStatus[self.Data.Path] = SwitchWorldItems[switchIndex];
					}
					if (worldSwitchStatus.TryGetValue(self.Data.Path, out var item) && item is not OriginalWorldItem)
					{
						if (self._worldIcon is UIImageFramed uIImageFramed)
						{
							uIImageFramed._texture = item.Icon;
							uIImageFramed.SetFrame(new Rectangle(0, 0, item.Icon.Width(), item.Icon.Height()));
						}
						else if (self._worldIcon is UIImage image)
						{
							image._texture = item.Icon;
						}
					}
					else
					{
						if (self._worldIcon is UIImageFramed uIImageFramed)
						{
							uIImageFramed._texture = originAsset;
							uIImageFramed.SetFrame(originFrame);
						}
						else if (self._worldIcon is UIImage image)
						{
							image._texture = originAsset;
						}
					}
				};
				changeButton.OnMouseOver += delegate
				{
					self._buttonLabel.SetText(Language.GetTextValue("换乘"));
				};
				changeButton.OnMouseOut += delegate
				{
					self._buttonLabel.SetText(string.Empty);
				};
				self.Append(changeButton);
				self._buttonLabel.Left.Set(changeButton.Left.Pixels + changeButton.Width.Pixels, 0f);

				UnLoadActions.Add(() =>
				{
					if (self == null)
						return;
					self.RemoveChild(changeButton);
					if (self._worldIcon is UIImageFramed uIImageFramed)
					{
						uIImageFramed._texture = originAsset;
						uIImageFramed.SetFrame(originFrame);
					}
					else if (self._worldIcon is UIImage image)
					{
						image._texture = originAsset;
					}
					self._buttonLabel.Left.Set(button.Left.Pixels + button.Width.Pixels, 0f);
					self._buttonLabel.SetText(Language.GetTextValue("换乘"));
				});
				break;
			}
		}
	}

	public void Unload()
	{
		On_UIWorldListItem.ctor -= On_UIWorldListItem_ctor;
		On_UIWorldListItem.PlayGame -= On_UIWorldListItem_PlayGame;
		On_AWorldListItem.UpdateGlitchAnimation -= On_AWorldListItem_UpdateGlitchAnimation;

		UnLoadActions.ForEach(a => a());
		UnLoadActions.Clear();
	}
}