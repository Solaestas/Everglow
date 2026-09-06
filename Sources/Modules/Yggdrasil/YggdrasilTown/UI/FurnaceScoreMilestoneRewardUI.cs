using Everglow.Commons.UI.UIElements;

namespace Everglow.Yggdrasil.YggdrasilTown.UI;

public class FurnaceScoreMilestoneRewardUI : UIBlock
{
	// ==================== UI elements ==================== //
	public UIItemSlot[] FurnaceScoreMilestoneRewardSlots;

	public UIVerticalScrollbar FurnaceScoreMilestoneRewardScrollbar;

	public static List<int> MilestoneRewards = new List<int> { 1000, 2000, 5000, 7000, 10000, 14000, 18000, 22000, 26000, 35000, 45000, 60000, 80000, 100000 };

	public override void OnInitialization()
	{
		base.OnInitialization();
		Info.HiddenOverflow = true;
		FurnaceScoreMilestoneRewardSlots = new UIItemSlot[14];
		for (int i = 0; i < FurnaceScoreMilestoneRewardSlots.Length; i++)
		{
			FurnaceScoreMilestoneRewardSlots[i] = new UIItemSlot()
			{
				SlotBackTexture = ModAsset.FurnaceShop_Normal.Value,
				ContainedItem = new Item(i, 1),
				CanPutInSlot = (item) => false,
				CanTakeOutSlot = (item) => false,
				CornerSize = new Vector2(10, 10),
				DrawColor = Color.White,
				Tooltip = "Furnace Score Milestone Reward Slot",
			};
			Register(FurnaceScoreMilestoneRewardSlots[i]);
		}
		FurnaceScoreMilestoneRewardScrollbar = new UIVerticalScrollbar();
		Register(FurnaceScoreMilestoneRewardScrollbar);
	}

	public override void Calculation()
	{
		base.Calculation();
		Info.Left.SetValue(300, 0f);
		Info.Top.SetValue(20, 0f);
		Info.Width.SetValue(192, 0f);
		Info.Height.SetValue(158, 0f);
		for (int i = 0; i < FurnaceScoreMilestoneRewardSlots.Length; i++)
		{
			FurnaceScoreMilestoneRewardSlots[i].Info.Left.SetValue(80, 0f);
			FurnaceScoreMilestoneRewardSlots[i].Info.Top.SetValue(120 - i * 40 + FurnaceScoreMilestoneRewardScrollbar.WheelValue * 800, 0f);
			FurnaceScoreMilestoneRewardSlots[i].Info.Width.SetValue(40, 0f);
			FurnaceScoreMilestoneRewardSlots[i].Info.Height.SetValue(40, 0f);
			if (YggdrasilTownFurnaceSystem.CurrentScore >= MilestoneRewards[i])
			{
				FurnaceScoreMilestoneRewardSlots[i].SlotBackTexture = ModAsset.FurnaceScoreMilestoneRewardUI_Enable.Value;
			}
			else
			{
				FurnaceScoreMilestoneRewardSlots[i].SlotBackTexture = ModAsset.FurnaceScoreMilestoneRewardUI_Lock.Value;
			}
		}
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);
		Calculation();
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		Rectangle destRec = Info.HitBox;
		destRec.Y -= 2;
		destRec.Height += 2;
		Draw9Piece_Board(sb, destRec, new Color(0.66f, 0.66f, 0.66f, 0.66f));
	}

	public void Draw9Piece_Board(SpriteBatch spriteBatch, Rectangle destinationBox, Color drawColor)
	{
		Texture2D tex = ModAsset.FurnaceShop_Normal.Value;
		int margin = 4;
		spriteBatch.Draw(tex, new Rectangle(destinationBox.X, destinationBox.Y, margin, margin), new Rectangle(0, 0, margin, margin), drawColor);
		spriteBatch.Draw(tex, new Rectangle(destinationBox.X + margin, destinationBox.Y, destinationBox.Width - margin * 2, margin), new Rectangle(margin, 0, 2, margin), drawColor);
		spriteBatch.Draw(tex, new Rectangle(destinationBox.X + destinationBox.Width - margin, destinationBox.Y, margin, margin), new Rectangle(tex.Width - margin, 0, margin, margin), drawColor);

		spriteBatch.Draw(tex, new Rectangle(destinationBox.X, destinationBox.Y + margin, margin, destinationBox.Height - margin * 2), new Rectangle(0, margin, margin, 2), drawColor);
		spriteBatch.Draw(tex, new Rectangle(destinationBox.X + margin, destinationBox.Y + margin, destinationBox.Width - margin * 2, destinationBox.Height - margin * 2), new Rectangle(margin, margin, 2, 2), drawColor);
		spriteBatch.Draw(tex, new Rectangle(destinationBox.X + destinationBox.Width - margin, destinationBox.Y + margin, margin, destinationBox.Height - margin * 2), new Rectangle(tex.Width - margin, margin, margin, 2), drawColor);

		spriteBatch.Draw(tex, new Rectangle(destinationBox.X, destinationBox.Y + destinationBox.Height - margin, margin, margin), new Rectangle(0, tex.Height - margin, margin, margin), drawColor);
		spriteBatch.Draw(tex, new Rectangle(destinationBox.X + margin, destinationBox.Y + destinationBox.Height - margin, destinationBox.Width - margin * 2, margin), new Rectangle(margin, tex.Height - margin, 2, margin), drawColor);
		spriteBatch.Draw(tex, new Rectangle(destinationBox.X + destinationBox.Width - margin, destinationBox.Y + destinationBox.Height - margin, margin, margin), new Rectangle(tex.Width - margin, tex.Height - margin, margin, margin), drawColor);
	}
}
