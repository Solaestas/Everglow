using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements
{
	internal class UIMissionListScrollbar : UIVerticalScrollbar
	{
		public override float Scale => 1f;

		public override float TopMax => 4f * Scale;

		public override float TopMin => 4f * Scale;

		public bool MouseOver = false;

		private UIBlock _bar = new UIBlock();
		private UIBlock _inner = new UIBlock();

		public UIMissionListScrollbar()
		{
			Info.Width.SetValue(8f, 0f);
			_innerScale = new Vector2(33f, 65f) * MissionContainer.Scale;
			AlwaysOnLight = true;
			_bar.PanelColor = Color.Transparent;
			_inner.PanelColor = Color.Transparent;
			_inner.ShowBorder = (false, false, false, false);
			_inner.Info.CanBeInteract = false;
			Register(_inner);
		}

		public override void Update(GameTime gt)
		{
			ChildrenElements.ForEach(child =>
			{
				if (child != null && child.IsVisible)
				{
					child.Update(gt);
				}
			});

			if (IsVisible)
			{
				Events.Update(this, gt);
			}

			bool isMouseHover = _inner.ContainsPoint(Main.MouseScreen);
			MouseOver = isMouseHover;
			var innerHeight = 1f * _innerScale.Y;
			float height = Info.TotalSize.Y - TopMax - TopMin - innerHeight;
			if (_isMouseDown)
			{
				WheelValue = (Main.mouseY -
					Info.TotalLocation.Y - TopMin - innerHeight / 2f) / height;
			}

			if (_wheelValue != _waitToWheelValue)
			{
				_wheelValue += (_waitToWheelValue - _wheelValue) / 4f;
				Calculation();
			}
		}

		public override void Calculation()
		{
			base.Calculation();
			_inner.Info.Top.SetValue(WheelValue * (ParentElement.HitBox.Height - 65), 0);
			_inner.Info.Left.SetValue(0, 0);
			_inner.Info.Width.SetValue(33, 0);
			_inner.Info.Height.SetValue(65, 0);
			_inner.Info.InteractiveMask = false;
		}

		public override bool ContainsPoint(Point point)
		{
			Rectangle containBar = HitBox;
			return base.ContainsPoint(point) || containBar.Contains(point.X, point.Y);
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			Texture2D tex = ModAsset.Marble_Texture.Value;
			Rectangle frame = new Rectangle(14, 233, 1, 1);
			sb.Draw(tex, HitBox, frame, Color.Black * 0.5f);
			int pos_coord_x = HitBox.X - ParentElement.ParentElement.HitBox.X;
			int pos_coord_y = HitBox.Y - ParentElement.ParentElement.HitBox.Y;
			frame = new Rectangle(pos_coord_x, pos_coord_y, 33, 65);
			sb.Draw(tex, _inner.Info.HitBox, frame, Color.White);
			if (MouseOver)
			{
				Texture2D highlight = ModAsset.MissionListThumb.Value;
				frame = new Rectangle(33, 0, 33, 65);
				sb.Draw(highlight, _inner.Info.HitBox.Center(), frame, Color.White, 0, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}
		}
	}
}
