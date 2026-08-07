using Microsoft.Xna.Framework.Input;

namespace Everglow.Commons.UI.UIElements
{
	public class UIHorizontalScrollbar : BaseElement, IScrollbar
	{
		private const int LEFT_HEIGHT = 1;
		private const int RIGHT_HEIGHT = 1;

		public Texture2D UIScrollbarTexture { get; protected set; }

		public Texture2D UIScrollbarInnerTexture { get; protected set; }

		protected Color _innerColor = Color.White;
		protected Vector2 _innerScale = new Vector2(0.75f);

		private UIImage inner;
		private float wheelValue;
		private float alpha = 0f;
		public bool AlwaysOnLight = false;
		public bool UseScrollWheel = false;
		private float waitToWheelValue = 0f;
		private float mouseX;
		protected bool _isMouseDown = false;
		private int whell = 0;

		public virtual float Scale => Info.HitBox.Height / (float)UIScrollbarInnerTexture.Height;

		public virtual float LeftMin => LEFT_HEIGHT * Scale;

		public virtual float LeftMax => RIGHT_HEIGHT * Scale;

		public float WheelValue
		{
			get
			{
				return wheelValue;
			}

			set
			{
				if (value > 1f)
				{
					waitToWheelValue = 1f;
				}
				else if (value < 0f)
				{
					waitToWheelValue = 0f;
				}
				else
				{
					waitToWheelValue = value;
				}
			}
		}

		private float _wheelValueMult = 1f;

		public float WheelValueMult { get => _wheelValueMult; set => _wheelValueMult = value; }

		public UIHorizontalScrollbar(float wheelValue = 0f)
		{
			Info.Height = new PositionStyle(20f, 0f);
			Info.Top = new PositionStyle(-20f, 1f);
			Info.Width = new PositionStyle(-20f, 1f);
			Info.Left = new PositionStyle(10f, 0f);
			Info.LeftMargin.Pixel = 5f;
			Info.RightMargin.Pixel = 5f;
			Info.IsSensitive = true;
			UIScrollbarInnerTexture = ModAsset.HorizontalScrollbar.Value;
			WheelValue = wheelValue;

			inner = new UIImage(ModAsset.HorizontalScrollbarInner.Value, Color.White);
			inner.Info.Height.SetValue(0f, 1f);
			Register(inner);
		}

		public override void Calculation()
		{
			base.Calculation();
			inner.Info.Top.SetValue(-(inner.Info.Size.Y - Info.Size.Y) / 2f, 0f);
			var t = ModAsset.HorizontalScrollbarInner.Value;
			inner.Info.Width.Pixel = t.Width / (float)t.Height * inner.Info.Size.Y;
			base.Calculation();
		}

		public override void LoadEvents()
		{
			base.LoadEvents();
			Events.OnLeftDown += element =>
			{
				if (!_isMouseDown)
				{
					_isMouseDown = true;
				}
			};
			Events.OnLeftUp += element =>
			{
				_isMouseDown = false;
			};
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);
			if (ParentElement == null)
			{
				return;
			}

			bool isMouseHover = ParentElement.GetCanHitBox().Contains(Main.MouseScreen.ToPoint());
			if (AlwaysOnLight)
			{
				alpha = 1f;
			}
			else
			{
				if ((isMouseHover || _isMouseDown) && alpha < 1f)
				{
					alpha += 0.01f;
				}

				if ((!(isMouseHover || _isMouseDown)) && alpha > 0f)
				{
					alpha -= 0.01f;
				}
			}

			inner.Color = Color.White * alpha;

			MouseState state = Mouse.GetState();
			float width = Info.Size.X - LeftMin - LeftMax - inner.Info.Size.X;
			if (!isMouseHover)
			{
				whell = state.ScrollWheelValue;
			}

			if (UseScrollWheel && isMouseHover && whell != state.ScrollWheelValue)
			{
				WheelValue -= (state.ScrollWheelValue - whell) / 6f / width * WheelValueMult;
				whell = state.ScrollWheelValue;
			}
			if (_isMouseDown && mouseX != Main.mouseX)
			{
				WheelValue = (Main.mouseX - Info.Location.X - LeftMin - inner.Info.Size.X / 2f) / width;
				mouseX = Main.mouseX;
			}

			inner.Info.Left.Pixel = LeftMin + width * WheelValue;
			wheelValue += (waitToWheelValue - wheelValue) / 6f;

			if (waitToWheelValue != wheelValue)
			{
				Calculation();
			}
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			float scale = Info.HitBox.Height / (float)UIScrollbarInnerTexture.Height;
			int ct = (int)(12 * scale);
			sb.Draw(UIScrollbarInnerTexture, new Rectangle(
				Info.HitBox.X,
				Info.HitBox.Y, ct, Info.HitBox.Height),
				new Rectangle(0, 0, 12, UIScrollbarInnerTexture.Height), Color.White * alpha);

			sb.Draw(UIScrollbarInnerTexture, new Rectangle(
				Info.HitBox.X + ct,
				Info.HitBox.Y, Info.HitBox.Width - ct * 2, Info.HitBox.Height),
				new Rectangle(12, 0, UIScrollbarInnerTexture.Width - 24, UIScrollbarInnerTexture.Height), Color.White * alpha);

			sb.Draw(UIScrollbarInnerTexture, new Rectangle(
				Info.HitBox.X - ct + Info.HitBox.Width,
				Info.HitBox.Y, ct, Info.HitBox.Height),
				new Rectangle(UIScrollbarInnerTexture.Width - 12, 0, 12, UIScrollbarInnerTexture.Height), Color.White * alpha);
		}
	}
}
