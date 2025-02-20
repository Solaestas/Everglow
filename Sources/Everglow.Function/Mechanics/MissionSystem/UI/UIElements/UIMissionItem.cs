using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.UI;
using Everglow.Commons.UI;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Utilities;
using Terraria.GameContent;
using static Everglow.Commons.Mechanics.MissionSystem.MissionManager;

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
		// 初始化属性
		Mission = missionBase;
		PanelColor = MissionContainer.Instance.GetThemeColor();

		// 初始化UI信息
		Info.Width.SetValue(-24f, 1f);
		Info.Height.SetValue(42f, 0f);
		Info.SetToCenter();
		Info.IsSensitive = true;

		// 鼠标悬停时改变颜色
		Events.OnMouseHover += OnMouseOver;
		Events.OnMouseOver += OnMouseOver;
		Events.OnMouseOut += OnMouseLeave;

		// 任务项容器
		block = new UIBlock();
		block.Info.Width.SetValue(-24f - 8f, 1f);
		block.Info.Height.SetValue(32f, 0f);
		block.Info.SetToCenter();
		block.Info.Left.SetValue(24f, 0f);
		block.Info.SetMargin(6f);
		block.PanelColor = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Dark, MissionContainer.ColorStyle.Dark);
		Register(block);

		// 任务进度
		var statusBar = new UIMissionStatus();
		statusBar.Info.SetToCenter();
		statusBar.Info.Left.SetValue(PositionStyle.Full - statusBar.Info.Width - (4f, 0f));
		statusBar.Events.OnUpdate += (e, gt) =>
		{
			statusBar.Progress = Mission.Progress;
			statusBar.Status = Mission.PoolType;
		};
		block.Register(statusBar);

		// 任务类型
		var typeBar = new UIMissionType();
		typeBar.Info.SetToCenter();
		typeBar.Info.Left.SetEmpty();
		typeBar.Events.OnUpdate += (e, gt) =>
		{
			typeBar.Type = Mission.MissionType;
		};
		block.Register(typeBar);

		// 任务名称
		var font = FontManager.FusionPixel12.GetFont(block.Info.Height.Pixel - block.Info.TopMargin.Pixel - block.Info.BottomMargin.Pixel - 2f);
		var name = new UITextPlus(Mission.DisplayName);
		name.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
		name.StringDrawer.Init(name.Text);
		block.Register(name);

		name.Info.SetToCenter();
		name.Info.Left.SetValue((16f, 0f));
		name.Calculation();
	}

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
		PanelColor = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Dark, MissionContainer.ColorStyle.Light);
	}

	/// <summary>
	/// 取消选中任务时
	/// <para/>更新任务的颜色
	/// </summary>
	public void OnUnselected()
	{
		PanelColor = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Dark, MissionContainer.ColorStyle.Normal);
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);
	}

	protected override void DrawChildren(SpriteBatch sb)
	{
		base.DrawChildren(sb);

		if (Mission.TimeMax < 0)
		{
			return;
		}

		var overflowHiddenRasterizerState = new RasterizerState
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true,
		};

		var sbS = sb.GetState().Value;
		sb.End();
		Effect effect = ModAsset.MissionProgressBar.Value;
		sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, overflowHiddenRasterizerState, effect, Main.UIScaleMatrix);

		effect.Parameters["uScaleFactor"].SetValue(Vector2.One / 16f);
		effect.Parameters["uRadius"].SetValue(8f);
		effect.Parameters["uThickness"].SetValue(1f);
		effect.Parameters["uProgress"].SetValue((float)(Mission.Time / (double)Mission.TimeMax));
		var color = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Light, MissionContainer.ColorStyle.Normal);
		effect.Parameters["uColor"].SetValue(new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f));
		effect.Parameters["uOpposite"].SetValue(true);
		effect.Parameters["uInterval"].SetValue(new Vector2(0, 0));
		effect.CurrentTechnique.Passes[0].Apply();

		sb.Draw(
			TextureAssets.MagicPixel.Value,
			new Rectangle(
				(int)(Info.Location.X + (block.Info.TotalLocation.X - Info.Location.X - 16f) / 2f),
				(int)(Info.Location.Y + (Info.Size.Y - 16f) / 2f),
				16, 16),
			Color.White);

		sb.End();
		sb.Begin(sbS);
	}

	private class UIMissionStatus : UIImage
	{
		public float Progress { get; set; }

		public PoolType Status { get; set; }

		public UIMissionStatus()
			: base(ModAsset.MissionStatus.Value, Color.White)
		{
		}

		protected override void DrawChildren(SpriteBatch sb)
		{
			base.DrawChildren(sb);

			// Draw status block to represent the status of mission
			var texture = Status switch
			{
				PoolType.Accepted => ModAsset.MissionStatus_Accepted.Value,
				PoolType.Available => ModAsset.MissionStatus_Available.Value,
				PoolType.Completed => ModAsset.MissionStatus_Completed.Value,
				PoolType.Overdue => ModAsset.MissionStatus_Failed.Value,
				PoolType.Failed => ModAsset.MissionStatus_Failed.Value,
				_ => ModAsset.MissionStatus.Value,
			};
			if (Status != PoolType.Accepted) // If status is not Accepted, then draw the status block directly
			{
				sb.Draw(texture, new Vector2(Info.TotalHitBox.X, Info.TotalHitBox.Y), null, color, Rotation, Origin, 1f, SpriteEffects, 0f);
			}
			else // If status is Accepted, then draw the plotted status block to represent progress and its background status block
			{
				var bgTexture = ModAsset.MissionStatus_Accepted_BG.Value;
				var sR = new Rectangle(0, (int)(texture.Height * (1f - Progress)), texture.Width, (int)(texture.Height * Progress));
				sb.Draw(bgTexture, new Vector2(Info.TotalHitBox.X, Info.TotalHitBox.Y), null, color, Rotation, Origin, 1f, SpriteEffects, 0f);
				sb.Draw(texture, new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y + sR.Y, sR.Width, sR.Height), sR, color, Rotation, Origin, SpriteEffects, 0f);
			}
		}
	}

	private class UIMissionType : UIImage
	{
		public MissionType Type { get; set; }

		public UIMissionType()
			: base(ModAsset.MissionStatus_Transparent.Value, Color.White)
		{
		}

		protected override void DrawChildren(SpriteBatch sb)
		{
			var texture = Type switch
			{
				MissionType.None => ModAsset.MissionType_Grey.Value,
				MissionType.MainStory => ModAsset.MissionType_Yellow.Value,
				MissionType.SideStory => ModAsset.MissionType_Purple.Value,
				MissionType.Achievement => ModAsset.MissionType_White.Value,
				MissionType.Challenge => ModAsset.MissionType_Red.Value,
				MissionType.Daily => ModAsset.MissionType_Blue.Value,
				MissionType.Legendary => ModAsset.MissionType_Prism.Value,
				_ => ModAsset.MissionStatus.Value,
			};

			sb.Draw(texture, new Vector2(Info.TotalHitBox.X, Info.TotalHitBox.Y), null, color, Rotation, Origin, 0.4f, SpriteEffects, 0f);
		}
	}
}