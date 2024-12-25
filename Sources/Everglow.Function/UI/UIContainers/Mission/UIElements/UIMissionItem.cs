using Everglow.Commons.MissionSystem;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Utilities;
using Terraria.GameContent;

namespace Everglow.Commons.UI.UIContainers.Mission.UIElements;

/// <summary>
/// 任务列表<see cref="MissionContainer"/>的任务项
/// </summary>
public class UIMissionItem : UIBlock
{
	private UIBlock _block;
	public MissionBase Mission;

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
		_block = new UIBlock();
		_block.Info.Width.SetValue(-24f - 8f, 1f);
		_block.Info.Height.SetValue(32f, 0f);
		_block.Info.SetToCenter();
		_block.Info.Left.SetValue(24f, 0f);
		_block.Info.SetMargin(6f);
		_block.PanelColor = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Dark, MissionContainer.ColorStyle.Dark);
		Register(_block);

		// 任务进度
		UIProgress progress = new UIProgress();
		progress.Info.SetToCenter();
		progress.Info.Left.SetValue(PositionStyle.Full - progress.Info.Width - (4f, 0f));
		progress.Events.OnUpdate += (e, gt) =>
		{
			progress.Progress = Mission.Progress;
		};
		_block.Register(progress);

		// 任务名称
		var font = FontManager.FusionPixel12.GetFont(_block.Info.Height.Pixel - _block.Info.TopMargin.Pixel - _block.Info.BottomMargin.Pixel - 2f);
		UITextPlus name = new UITextPlus(Mission.DisplayName);
		name.StringDrawer.DefaultParameters.SetParameter("FontSize", 20f);
		name.StringDrawer.Init(name.Text);
		name.Events.OnUpdate += (e, gt) =>
		{
			name.Info.SetToCenter();
			name.Info.Left.SetEmpty();
			name.Calculation();
		};
		_block.Register(name);
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

		var scissorRectangle = sb.GraphicsDevice.ScissorRectangle;
		var overflowHiddenRasterizerState = new RasterizerState
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true,
		};

		var sbS = GraphicsUtils.GetState(sb).Value;
		sb.End();
		Effect effect = ModAsset.MissionProgressBar.Value;
		sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
			DepthStencilState.None, overflowHiddenRasterizerState, effect, Main.UIScaleMatrix);

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
				(int)(Info.Location.X + (_block.Info.TotalLocation.X - Info.Location.X - 16f) / 2f),
				(int)(Info.Location.Y + (Info.Size.Y - 16f) / 2f),
				16, 16), Color.White);

		sb.End();
		sb.Begin(sbS);
	}

	private class UIProgress : UIImage
	{
		public float Progress = 0f;

		public UIProgress()
			: base(ModAsset.MissionDuration.Value, Color.White)
		{
		}

		protected override void DrawChildren(SpriteBatch sb)
		{
			var texture = Progress >= 1f ? ModAsset.MissionDurationFinish.Value : ModAsset.MissionDurationBar.Value;
			Rectangle sr = new Rectangle(0, (int)(texture.Height * (1f - Progress)), texture.Width, (int)(texture.Height * Progress));
			sb.Draw(
				texture,
				new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y + sr.Y, sr.Width, sr.Height),
				sr, _color, Rotation, Origin, SpriteEffects, 0f);
			base.DrawChildren(sb);
		}
	}
}