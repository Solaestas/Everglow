using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.UI.UIContainers.Mission.UIElements
{
	internal class UITextVerticalScrollbar : UIVerticalScrollbar
	{
		public override float Scale => 1f;

		public override float TopMax => 4f * Scale;

		public override float TopMin => 4f * Scale;

		private UIBlock _topBar = new UIBlock();
		private UIBlock _bottomBar = new UIBlock();
		private UIBlock _inner = new UIBlock();

		public UITextVerticalScrollbar()
		{
			Info.Width.SetValue(2f, 0f);
			_innerScale = new Vector2(6f, 30f);
			AlwaysOnLight = true;

			_topBar.ShowBorder = _bottomBar.ShowBorder = _inner.ShowBorder = (false, false, false, false);
			_topBar.PanelColor = MissionContainer.Instance.GetThemeColor(MissionContainer.ColorType.Light, MissionContainer.ColorStyle.Dark);
			_bottomBar.PanelColor = _topBar.PanelColor;
			_inner.PanelColor = _topBar.PanelColor;

			UIBlock mask = new UIBlock();
			mask.Info.Width.SetValue(4f, 0f);
			mask.Info.Height.SetValue(1f, 0f);
			mask.Info.CanBeInteract = false;
			mask.Info.SetToCenter();
			mask.Info.Top.SetValue(1f, 0f);
			mask.ShowBorder = (false, false, false, false);
			mask.PanelColor = _topBar.PanelColor;
			Register(mask);

			mask = new UIBlock();
			mask.Info.Width.SetValue(4f, 0f);
			mask.Info.Height.SetValue(1f, 0f);
			mask.Info.CanBeInteract = false;
			mask.Info.SetToCenter();
			mask.Info.Top.SetValue(-2f, 1f);
			mask.ShowBorder = (false, false, false, false);
			mask.PanelColor = _topBar.PanelColor;
			Register(mask);
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

			bool isMouseHover = ContainsPoint(Main.MouseScreen);
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

		public override bool ContainsPoint(Point point)
		{
			var innerHeight = UIScrollbarInnerTexture.Height * _innerScale.Y;
			float height = Info.TotalSize.Y - TopMax - TopMin - innerHeight;
			var top = TopMin + height * WheelValue;
			var p = new Vector2(
				Info.TotalLocation.X - (UIScrollbarInnerTexture.Width * _innerScale.X - Info.TotalSize.X) / 2f,
				Info.TotalLocation.Y + top);
			var size = UIScrollbarInnerTexture.Size() * _innerScale;
			return base.ContainsPoint(point) || new Rectangle((int)Math.Round(p.X), (int)Math.Round(p.Y),
				(int)Math.Round(size.X), (int)Math.Round(size.Y)).Contains(point.X, point.Y);
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			_topBar.Info.TotalHitBox = _bottomBar.Info.TotalHitBox = Info.TotalHitBox;

			var innerHeight = _innerScale.Y;
			float height = Info.TotalSize.Y - TopMax - TopMin - innerHeight;
			var top = TopMin + height * WheelValue;
			_inner.Info.TotalHitBox = new Rectangle(
				(int)(Info.TotalLocation.X - (UIScrollbarInnerTexture.Width * _innerScale.X - Info.TotalSize.X) / 2f),
				(int)(Info.TotalLocation.Y + top),
				(int)_innerScale.X,
				(int)_innerScale.Y);
			_topBar.Info.TotalHitBox.Height = Math.Max(2, _inner.Info.TotalHitBox.Y - Info.TotalHitBox.Y - 1);
			_bottomBar.Info.TotalHitBox.Y = Math.Max(
				_bottomBar.Info.TotalHitBox.Bottom - Info.TotalHitBox.Height - 2,
				_inner.Info.TotalHitBox.Bottom + 1);
			_bottomBar.Info.TotalHitBox.Height = Math.Max(2, Info.TotalHitBox.Bottom - _bottomBar.Info.TotalHitBox.Y);

			_topBar.Draw(sb);
			_bottomBar.Draw(sb);
			_inner.Draw(sb);
		}
	}
}