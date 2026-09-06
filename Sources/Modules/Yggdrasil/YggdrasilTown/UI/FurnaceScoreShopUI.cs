using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Everglow.Yggdrasil.YggdrasilTown.UI;

public class FurnaceScoreShopUI : UIContainerElement
{
	public static FurnaceScoreShopUI Instance => (FurnaceScoreShopUI)UISystem.EverglowUISystem.Elements[typeof(FurnaceScoreShopUI).FullName];

	// ==================== UI elements ==================== //
	public UIItemSlot[] FurnaceScoreShopItemSlots;

	public FurnaceScoreMilestoneRewardUI _mileStoneRewardUI;

	public override void OnInitialization()
	{
		base.OnInitialization();
		FurnaceScoreShopItemSlots = new UIItemSlot[28];
		for (int i = 0; i < FurnaceScoreShopItemSlots.Length; i++)
		{
			FurnaceScoreShopItemSlots[i] = new UIItemSlot()
			{
				SlotBackTexture = ModAsset.FurnaceShop_Normal.Value,
				ContainedItem = new Item(i, 1),
				CanPutInSlot = (item) => false,
				CanTakeOutSlot = (item) => false,
				CornerSize = new Vector2(10, 10),
				DrawColor = Color.White,
				Tooltip = "Furnace Score Shop Item Slot",
			};
			Register(FurnaceScoreShopItemSlots[i]);
		}
		_mileStoneRewardUI = new FurnaceScoreMilestoneRewardUI()
		{
			CanDrag = false,
		};
		Register(_mileStoneRewardUI);
	}

	public override void Calculation()
	{
		base.Calculation();
		Info.Left.SetValue(0, 0f);
		Info.Top.SetValue(240, 0f);
		Info.Width.SetValue(480, 0f);
		Info.Height.SetValue(300, 0f);
		for (int i = 0; i < FurnaceScoreShopItemSlots.Length; i++)
		{
			FurnaceScoreShopItemSlots[i].Info.Left.SetValue(20 + i % 7 * 40, 0f);
			FurnaceScoreShopItemSlots[i].Info.Top.SetValue(20 + i / 7 * 40, 0f);
			FurnaceScoreShopItemSlots[i].Info.Width.SetValue(36, 0f);
			FurnaceScoreShopItemSlots[i].Info.Height.SetValue(36, 0f);
			FurnaceScoreShopItemSlots[i].DrawColor = new Color(0.66f, 0.66f, 0.66f, 0.66f);
		}
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);
		Calculation();
	}

	public override void Draw(SpriteBatch sb)
	{
		Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "Furnace Score Shop & Milestone Reward", 504f, Info.Top.Pixel + 18, Color.White * ((float)Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, 1f);
		base.Draw(sb);
	}

}
