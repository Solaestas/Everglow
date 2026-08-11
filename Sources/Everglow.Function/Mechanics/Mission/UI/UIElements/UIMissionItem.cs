using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;
using static Everglow.Commons.Mechanics.Mission.UI.MissionContainer;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements;

/// <summary>
/// 任务列表<see cref="MissionContainer"/>的任务项
/// </summary>
public class UIMissionItem : UIBlock
{
	private UIBlock block;
	private UIBlock nameContainer;
	private UITextPlus name;

	private float oldScale;

	public PlayerMissionBase Mission { get; private set; }

	public UIMissionItem(PlayerMissionBase missionBase)
	{
		Mission = missionBase;
		PanelColor = Color.Transparent;
		BorderWidth = 0;

		// 初始化UI信息
		Info.Width.SetValue(320f * Scale, 0f);
		Info.Height.SetValue(60f * Scale, 0f);
		Info.Left.SetValue(100 * Scale);
		Info.SetMargin(0);
		Info.IsSensitive = true;

		// 鼠标悬停时改变颜色
		Events.OnMouseHover += OnMouseOver;
		Events.OnMouseOver += OnMouseOver;
		Events.OnMouseOut += OnMouseLeave;

		// 任务项容器
		block = new UIBlock();
		block.Info.Width.SetFull();
		block.Info.Height.SetFull();
		block.BorderWidth = 0;
		block.PanelColor = Color.Transparent;
		block.Info.SetMargin(0);
		Register(block);

		// 任务名称
		nameContainer = new UIBlock();
		nameContainer.Info.Width.SetValue(220 * Scale);
		nameContainer.Info.Height.SetFull();
		nameContainer.Info.SetToCenter();
		nameContainer.Info.Left.SetValue(0, 0.2f);
		nameContainer.Info.HiddenOverflow = true;
		nameContainer.PanelColor = Color.Transparent;
		nameContainer.BorderWidth = 0;
		block.Register(nameContainer);

		var font = FontManager.FusionPixel12.GetFont(40f * Scale);
		name = new UITextPlus(Mission.DisplayName);
		name.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * Scale);
		name.StringDrawer.Init(name.Text);
		nameContainer.Register(name);

		name.Info.SetToCenter();
		name.Info.Left.SetEmpty();
		name.Calculation();
	}

	public override void Calculation()
	{
		base.Calculation();
		float missionListWidth = ParentElement.ParentElement.ParentElement.Info.Width.Pixel;
		Info.Width.SetValue(missionListWidth - 120, 0f);
		Info.Height.SetValue(93f * Scale, 0f);
		Info.Left.SetValue(20 * Scale);

		nameContainer.Info.Width.SetValue(missionListWidth - 240);
		nameContainer.Info.Height.SetFull();
		nameContainer.Info.SetToCenter();
		nameContainer.Info.Left.SetValue(90, 0);

		if (oldScale != Scale)
		{
			oldScale = Scale;

			nameContainer.ChildrenElements.RemoveAll(m => m is UITextPlus);
			name = new UITextPlus(Mission.DisplayName);
			name.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * Scale);
			name.StringDrawer.Init(name.Text);
			nameContainer.Register(name);

			name.Info.SetToCenter();
			name.Info.Left.SetEmpty();
			name.Calculation();
		}
	}

	/// <summary>
	/// 鼠标悬停时
	/// <para/>更新任务的颜色，但不更新选中的任务的颜色
	/// </summary>
	/// <param name="e"></param>
	private void OnMouseOver(BaseElement e)
	{
		if (Instance.SelectedItem != this)
		{
			PanelColor = Color.Gray;
		}
	}

	/// <summary>
	/// 鼠标离开时
	/// <para/>更新任务的颜色，但不更新选中的任务的颜色
	/// </summary>
	/// <param name="e"></param>
	private void OnMouseLeave(BaseElement e)
	{
		if (Instance.SelectedItem != this)
		{
			OnUnselected();
		}
	}

	/// <summary>
	/// 选中任务时
	/// <para/>更新任务的颜色
	/// </summary>
	public void OnSelected()
	{
		PanelColor = Color.White;
	}

	/// <summary>
	/// 取消选中任务时
	/// <para/>更新任务的颜色
	/// </summary>
	public void OnUnselected()
	{
		PanelColor = Color.Transparent;
	}

	protected override void DrawChildren(SpriteBatch sb)
	{
		DrawTimerProgress();
		DrawPanel(sb);
		base.DrawChildren(sb);
	}

	private void DrawPanel(SpriteBatch sb)
	{
		if (ParentElement.ParentElement.ParentElement is UIMissionList uiml)
		{
			int index = uiml.MissionItems.IndexOf(this);
			Vector2 coord = new Vector2(HitBox.X, index * 93 + ParentElement.ParentElement.HitBox.Y) - ParentElement.ParentElement.ParentElement.ParentElement.HitBox.TopLeft();
			Main.NewText(coord);
			var panel_texRectangle = new Rectangle((int)coord.X, (int)coord.Y, HitBox.Width, HitBox.Height);
			Texture2D background = ModAsset.Marble_Texture.Value;
			sb.Draw(background, HitBox, panel_texRectangle, Color.White);
		}
		Texture2D tex = ModAsset.MissionStackPanel.Value;
		var drawBox = HitBox;
		Draw9Piece_MissionStackPanel7x7(sb, drawBox, 41, 38);
		drawBox.X += 5;
		drawBox.Y += 4;
		drawBox.Width -= 10;
		drawBox.Height -= 8;
		Draw9Piece_MissionStackPanel7x7(sb, drawBox, 59, 38);
		drawBox.X += 68;
		drawBox.Y += 10;
		drawBox.Width -= 79;
		drawBox.Height -= 20;
		Draw9Piece_MissionStackPanel7x7(sb, drawBox, 41, 47);

		var gem_frame = ColorDefinition.GetGemFrame(Mission.Type);
		sb.Draw(tex, HitBox.Left() + new Vector2(40, 0), gem_frame, Color.White, 0, gem_frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		gem_frame = new Rectangle(0, 36, 39, 39);
		sb.Draw(tex, HitBox.Left() + new Vector2(40, 0), gem_frame, Color.White, 0, gem_frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
	}

	private void Draw9Piece_MissionStackPanel7x7(SpriteBatch sb, Rectangle hitbox, int frameX, int frameY)
	{
		Texture2D tex = ModAsset.MissionStackPanel.Value;

		// row1
		Rectangle frame = new Rectangle(frameX, frameY, 3, 3);
		Rectangle des = hitbox;
		des.Width = 3;
		des.Height = 3;
		sb.Draw(tex, des, frame, Color.White);

		frame = new Rectangle(frameX + 3, frameY, 1, 3);
		des.Width = hitbox.Width - 6;
		des.X += 3;
		sb.Draw(tex, des, frame, Color.White);

		frame = new Rectangle(frameX + 4, frameY, 3, 3);
		des.Width = 3;
		des.X = hitbox.Right - 3;
		sb.Draw(tex, des, frame, Color.White);

		// row2
		frame = new Rectangle(frameX, frameY + 3, 3, 1);
		des = hitbox;
		des.Width = 3;
		des.Height = hitbox.Height - 6;
		des.Y += 3;
		sb.Draw(tex, des, frame, Color.White);

		frame = new Rectangle(frameX + 3, frameY + 3, 1, 1);
		des.Width = hitbox.Width - 6;
		des.Height = hitbox.Height - 6;
		des.X += 3;
		sb.Draw(tex, des, frame, Color.White);

		frame = new Rectangle(frameX + 4, frameY + 3, 3, 1);
		des.Width = 3;
		des.Height = hitbox.Height - 6;
		des.X = hitbox.Right - 3;
		sb.Draw(tex, des, frame, Color.White);

		// row3
		frame = new Rectangle(frameX, frameY + 4, 3, 3);
		des = hitbox;
		des.Width = 3;
		des.Height = 3;
		des.Y = hitbox.Bottom - 3;
		sb.Draw(tex, des, frame, Color.White);

		frame = new Rectangle(frameX + 3, frameY + 4, 1, 3);
		des.Width = hitbox.Width - 6;
		des.Height = 3;
		des.X += 3;
		sb.Draw(tex, des, frame, Color.White);

		frame = new Rectangle(frameX + 4, frameY + 4, 3, 3);
		des.Width = 3;
		des.X = hitbox.Right - 3;
		des.Height = 3;
		sb.Draw(tex, des, frame, Color.White);
	}

	private void DrawTimerProgress()
	{
		if (Mission.TimeLimit < 0)
		{
			return;
		}

		var progress = 1 - Mission.Time / (float)Mission.TimeLimit;
		var colorValue = MathF.Sqrt(progress);
		var offset = (int)(45 * MissionContainer.Scale);
		var dest = new Rectangle(HitBox.X + offset, HitBox.Y, (int)((HitBox.Width - offset) * progress), HitBox.Height);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, dest, new Color(0.5f, colorValue * 0.5f, colorValue * 0.5f, 0.1f));
	}
}
