using Everglow.Commons.MissionSystem;
using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;

namespace Everglow.Commons.UI.UIContainers.Mission.UIElements;

public class UIMissionItem : UIBlock
{
	private UIBlock _block;
	public MissionBase Mission;

	public UIMissionItem(MissionBase missionBase)
	{
		Mission = missionBase;
		Info.Width.SetValue(-24f, 1f);
		Info.Height.SetValue(42f, 0f);
		Info.SetToCenter();
		PanelColor = MissionContainer.Instance.GetThemeColor();

		_block = new UIBlock();
		_block.Info.Width.SetValue(-24f - 8f, 1f);
		_block.Info.Height.SetValue(32f, 0f);
		_block.Info.SetToCenter();
		_block.Info.Left.SetValue(24f, 0f);
		_block.Info.SetMargin(6f);
		_block.PanelColor = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Dark, MissionContainer.ColorStyle.Dark);
		Register(_block);

		UIProgress progress = new UIProgress();
		progress.Info.SetToCenter();
		progress.Info.Left.SetValue(PositionStyle.Full - progress.Info.Width - (4f, 0f));
		progress.Events.OnUpdate += (e, gt) =>
		{
			progress.Progress = Mission.Progress;
		};
		_block.Register(progress);

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

		Info.IsSensitive = true;
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);
		PanelColor = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Dark, 
			MissionContainer.Instance.SelectedItem == this ? MissionContainer.ColorStyle.Light : MissionContainer.ColorStyle.Normal);
	}

	protected override void DrawChildren(SpriteBatch sb)
	{
		base.DrawChildren(sb);
		if (Mission.TimeMax < 0)
			return;
		var scissorRectangle = sb.GraphicsDevice.ScissorRectangle;
		var overflowHiddenRasterizerState = new RasterizerState
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true,
		};

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
		sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
			DepthStencilState.None, overflowHiddenRasterizerState, null, Main.UIScaleMatrix);
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