using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;
using UIImage = Everglow.Commons.UI.UIElements.UIImage;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

/// <summary>
/// 任务列表<see cref="MissionContainer"/>的任务项
/// </summary>
public class UIMissionItem : UIBlock
{
	private UIBlock block;

	public MissionBase Mission { get; private set; }

	public UIMissionItem(MissionBase missionBase)
	{
		Mission = missionBase;
		PanelColor = Color.Transparent;
		BorderWidth = 0;

		var scale = MissionContainer.Scale;

		// 初始化UI信息
		Info.Width.SetValue(320f * scale, 0f);
		Info.Height.SetValue(60f * scale, 0f);
		Info.SetToCenter();
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
		var _background = new UIImage(GetBackground(Mission), Color.White);
		_background.Info.Width.SetFull();
		_background.Info.Height.SetFull();
		_background.Style = UIImage.CalculationStyle.None;
		block.Register(_background);

		// 任务进度
		var statusBar = new UIImage(GetMissionStatus(Mission), Color.White);
		statusBar.Info.Top.SetValue(19f * scale, 0);
		statusBar.Info.Left.SetValue(291f * scale, 0);
		statusBar.Info.Width.SetValue(12 * scale, 0);
		statusBar.Info.Height.SetValue(32 * scale, 0);
		block.Register(statusBar);

		// 任务名称
		var nameContainer = new UIBlock();
		nameContainer.Info.Width.SetValue(220 * scale);
		nameContainer.Info.Height.SetFull();
		nameContainer.Info.SetToCenter();
		nameContainer.Info.Left.SetValue(0, 0.2f);
		nameContainer.Info.HiddenOverflow = true;
		nameContainer.PanelColor = Color.Transparent;
		nameContainer.BorderWidth = 0;
		block.Register(nameContainer);

		var font = FontManager.FusionPixel12.GetFont(40f * scale);
		var name = new UITextPlus(Mission.DisplayName);
		name.StringDrawer.DefaultParameters.SetParameter("FontSize", 36f * scale);
		name.StringDrawer.Init(name.Text);
		nameContainer.Register(name);

		name.Info.SetToCenter();
		name.Info.Left.SetEmpty();
		name.Calculation();
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