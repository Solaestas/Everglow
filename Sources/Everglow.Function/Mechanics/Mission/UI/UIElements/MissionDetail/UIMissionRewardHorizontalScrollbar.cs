using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail
{
	public class UIMissionRewardHorizontalScrollbar : UIHorizontalScrollbar
	{
		public bool MouseOver = false;

		/// <summary>
		/// 上下箭头
		/// </summary>
		private UIBlock _scrollbarArrow = new UIBlock();

		/// <summary>
		/// 轨道
		/// </summary>
		private UIBlock _scrollbarTrack = new UIBlock();

		/// <summary>
		/// 滑块
		/// </summary>
		private UIBlock _scrollbarThumb = new UIBlock();

		public override void OnInitialization()
		{
			base.OnInitialization();
			_scrollbarArrow.ShowBorder = _scrollbarTrack.ShowBorder = _scrollbarThumb.ShowBorder = (false, false, false, false);
			_scrollbarArrow.PanelColor = Color.Transparent;
			_scrollbarTrack.PanelColor = Color.Transparent;
			_scrollbarThumb.PanelColor = Color.Transparent;
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			var trackTexture = ModAsset.MissionSideRollingGroove.Value;
			var trackScale = new Vector2(1, Info.TotalHitBox.Height / (float)trackTexture.Height);
			sb.Draw(trackTexture, Info.TotalHitBox.Top() + new Vector2(0, -16), new Rectangle(0, 0, 7, 6), Color.White, 0, new Vector2(3.5f, 0), 2, SpriteEffects.None, 0);
			sb.Draw(trackTexture, new Rectangle(Info.TotalHitBox.Left - 6, Info.TotalHitBox.Top - 4, 14, Info.TotalHitBox.Height + 5), new Rectangle(0, 7, 7, 6), Color.White);
			sb.Draw(trackTexture, Info.TotalHitBox.Top() + new Vector2(0, Info.TotalHitBox.Height + 2), new Rectangle(0, 14, 7, 7), Color.White, 0, new Vector2(3.5f, 0), 2, SpriteEffects.None, 0);
			var thumbTexture = ModAsset.MissionSideRollingBlock.Value;
			var thumbFrame = new Rectangle(0, 0, 29, 11);
			if (MouseOver || _isMouseDown)
			{
				thumbFrame = new Rectangle(0, 11, 29, 11);
			}
			sb.Draw(thumbTexture, _scrollbarThumb.Info.TotalHitBox.Center(), thumbFrame, Color.White, 0, thumbFrame.Size() * 0.5f, 2f, SpriteEffects.None, 0);
		}
	}
}
