using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Vertex;
using Terraria.GameContent;
using UIImage = Everglow.Commons.UI.UIElements.UIImage;
using static Everglow.Commons.Mechanics.MissionSystem.UI.MissionContainer;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

/// <summary>
/// 任务列表<see cref="MissionContainer"/>的任务项
/// </summary>
public class UIMissionItem : UIBlock
{
	private UIBlock block;
	private UIImage _background;
	private UIImage statusBar;
	private UIBlock nameContainer;
	private UITextPlus name;

	private float oldScale;

	public MissionBase Mission { get; private set; }

	public UIMissionItem(MissionBase missionBase)
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

		// 任务项背景
		_background = new UIImage(GetBackground(Mission), Color.White);
		_background.Info.Width.SetFull();
		_background.Info.Height.SetFull();
		_background.Style = UIImage.CalculationStyle.None;
		block.Register(_background);

		// 任务进度
		statusBar = new UIImage(GetMissionStatus(Mission), Color.White);
		statusBar.Info.Top.SetValue(19f * Scale, 0);
		statusBar.Info.Left.SetValue(291f * Scale, 0);
		statusBar.Info.Width.SetValue(12 * Scale, 0);
		statusBar.Info.Height.SetValue(32 * Scale, 0);
		block.Register(statusBar);

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

		Info.Width.SetValue(320f * Scale, 0f);
		Info.Height.SetValue(60f * Scale, 0f);
		Info.Left.SetValue(20 * Scale);

		statusBar.Info.Top.SetValue(19f * Scale, 0);
		statusBar.Info.Left.SetValue(291f * Scale, 0);
		statusBar.Info.Width.SetValue(12 * Scale, 0);
		statusBar.Info.Height.SetValue(32 * Scale, 0);

		nameContainer.Info.Width.SetValue(220 * Scale);
		nameContainer.Info.Height.SetFull();
		nameContainer.Info.SetToCenter();
		nameContainer.Info.Left.SetValue(0, 0.2f);

		if(oldScale != Scale)
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

	private static Texture2D GetBackground(MissionBase mission) => mission.MissionType switch
	{
		MissionType.None => ModAsset.MissionStackPanel_None.Value,
		MissionType.MainStory => ModAsset.MissionStackPanel_MainStory.Value,
		MissionType.SideStory => ModAsset.MissionStackPanel_SideStory.Value,
		MissionType.Achievement => ModAsset.MissionStackPanel_Achievement.Value,
		MissionType.Challenge => ModAsset.MissionStackPanel_Challenge.Value,
		MissionType.Daily => ModAsset.MissionStackPanel_Daily.Value,
		MissionType.Legendary => ModAsset.MissionStackPanel_Legendary.Value,
		_ => ModAsset.MissionStackPanel_None.Value,
	};

	private static Texture2D GetMissionStatus(MissionBase mission) => mission.PoolType switch
	{
		PoolType.Accepted => ModAsset.MissionState_Accepted.Value,
		PoolType.Available => ModAsset.MissionState_Available.Value,
		PoolType.Completed => ModAsset.MissionState_Completed.Value,
		PoolType.Overdue => ModAsset.MissionState_Failed.Value,
		PoolType.Failed => ModAsset.MissionState_Failed.Value,
		_ => ModAsset.MissionState_Accepted.Value,
	};

	/// <summary>
	/// 鼠标悬停时
	/// <para/>更新任务的颜色，但不更新选中的任务的颜色
	/// </summary>
	/// <param name="e"></param>
	private void OnMouseOver(BaseElement e)
	{
		if (MissionContainer.Instance.SelectedItem != this)
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
		if (MissionContainer.Instance.SelectedItem != this)
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
		var width = 15 * MissionContainer.Scale;
		var y1 = 12 * MissionContainer.Scale;
		var y2 = 36 * MissionContainer.Scale;
		var startColor = MissionColorUtils.GetMissionTypeColor(Mission.MissionType) * 0.4f;
		var endColor = Color.Transparent;
		var vertices = new List<Vertex2D>();
		{
			vertices.Add(new Vector2(HitBox.X, HitBox.Y), endColor, new(0, 0, 0));
			vertices.Add(new Vector2(HitBox.X, HitBox.Y + HitBox.Height), endColor, new(0, 0, 0));
			vertices.Add(new Vector2(HitBox.X - width, HitBox.Y + y1), startColor, new(0, 0, 0));
			vertices.Add(new Vector2(HitBox.X - width, HitBox.Y + y2), startColor, new(0, 0, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

		base.DrawChildren(sb);

		DrawTimerProgress();
	}

	private void DrawTimerProgress()
	{
		if (Mission.TimeMax < 0)
		{
			return;
		}

		var progress = 1 - Mission.Time / (float)Mission.TimeMax;
		var colorValue = MathF.Sqrt(progress);
		var offset = (int)(45 * MissionContainer.Scale);
		var dest = new Rectangle(HitBox.X + offset, HitBox.Y, (int)((HitBox.Width - offset) * progress), HitBox.Height);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, dest, new Color(0.5f, colorValue * 0.5f, colorValue * 0.5f, 0.1f));
	}
}