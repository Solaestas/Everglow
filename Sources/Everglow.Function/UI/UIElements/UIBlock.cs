using Terraria.GameContent;

namespace Everglow.Commons.UI.UIElements
{
	public class UIBlock : BaseElement
	{
		public bool CanDrag = false;
		private bool dragging = false;

		public Color PanelColor = new Color(71, 92, 172);
		public Color BorderColor = Color.Black;
		public (bool LeftBorder, bool TopBorder, bool RightBorder, bool BottomBorder) ShowBorder = (true, true, true, true);
		public int BorderWidth = 2;
		private Vector2 _mousePosCache = Vector2.Zero;

		public UIBlock()
		{
			Info.SetMargin(2f);
		}

		public override void LoadEvents()
		{
			base.LoadEvents();
			Events.OnLeftDown += (element) =>
			{
				dragging = true;
				_mousePosCache = Main.MouseScreen;
			};
			Events.OnLeftUp += (element) => dragging = false;
		}

		public override void Calculation()
		{
			base.Calculation();
			if (Info.TotalSize.X < 1)
			{
				Info.TotalSize.X = 1;
				Info.TotalHitBox.Width = (int)Info.TotalSize.X;
			}
			if (Info.TotalSize.Y < 1)
			{
				Info.TotalSize.Y = 1;
				Info.TotalHitBox.Height = (int)Info.TotalSize.Y;
			}
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);
			if (CanDrag && dragging)
			{
				Vector2 offset = Main.MouseScreen - _mousePosCache;
				_mousePosCache = Main.MouseScreen;
				Info.Left.Pixel += offset.X;
				Info.Top.Pixel += offset.Y;
				Calculation();
			}
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			base.DrawSelf(sb);
			Texture2D texture = TextureAssets.MagicPixel.Value;
			sb.Draw(texture, Info.TotalHitBox, PanelColor);
			if (ShowBorder.LeftBorder)
			{
				if (ShowBorder.BottomBorder)
					sb.Draw(texture,
						new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y, BorderWidth, Info.TotalHitBox.Height - BorderWidth),
						BorderColor);
				else
					sb.Draw(texture,
					new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y, BorderWidth, Info.TotalHitBox.Height),
					BorderColor);
			}
			if (ShowBorder.TopBorder)
			{
				if (ShowBorder.LeftBorder)
					sb.Draw(texture,
						new Rectangle(Info.TotalHitBox.X + BorderWidth, Info.TotalHitBox.Y, Info.TotalHitBox.Width - BorderWidth, BorderWidth),
						BorderColor);
				else
					sb.Draw(texture,
					new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Y, Info.TotalHitBox.Width, BorderWidth),
					BorderColor);
			}
			if (ShowBorder.RightBorder)
			{
				if (ShowBorder.TopBorder)
					sb.Draw(texture,
						new Rectangle(Info.TotalHitBox.Right - BorderWidth, Info.TotalHitBox.Y + BorderWidth, BorderWidth, Info.TotalHitBox.Height - BorderWidth),
						BorderColor);
				else
					sb.Draw(texture,
					new Rectangle(Info.TotalHitBox.Right - BorderWidth, Info.TotalHitBox.Y, BorderWidth, Info.TotalHitBox.Height),
					BorderColor);
			}
			if (ShowBorder.BottomBorder)
			{
				if (ShowBorder.RightBorder)
					sb.Draw(texture,
						new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Bottom - BorderWidth, Info.TotalHitBox.Width - BorderWidth, BorderWidth),
						BorderColor);
				else
					sb.Draw(texture,
					new Rectangle(Info.TotalHitBox.X, Info.TotalHitBox.Bottom - BorderWidth, Info.TotalHitBox.Width, BorderWidth),
					BorderColor);
			}
		}
	}
}