using Microsoft.Xna.Framework.Input;

namespace Everglow.Commons.UI.UIElements
{
	public class UIVerticalScrollbar : BaseElement, IScrollbar
	{
		private const int TOP_HEIGHT = 1;
		private const int BUTTOM_HEIGHT = 1;

		public Texture2D UIScrollbarInnerTexture { get; protected set; }

		private UIImage inner;
		private bool isMouseDown = false;
		private float mouseY;
		private int whell = 0;
		private float wheelValue;
		private float waitToWheelValue;
		private float alpha = 0f;
		public float Alpha = 1f;
		public bool AlwaysOnLight = false;
		public float LightSpeed = 0.04f;

		public float Scale => Info.HitBox.Width / (float)UIScrollbarInnerTexture.Width;

		public float TopMin => 116f * Scale;

		public float TopMax => 142f * Scale;

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

		public bool UseScrollWheel = true;

		public UIVerticalScrollbar(float wheelValue = 0f)
		{
			Info.Width = new PositionStyle(20f, 0f);
			Info.Left = new PositionStyle(-20f, 1f);
			Info.Height = new PositionStyle(-20f, 1f);
			Info.Top = new PositionStyle(10f, 0f);
			Info.TopMargin.Pixel = 5f;
			Info.ButtomMargin.Pixel = 5f;
			Info.IsSensitive = true;
			UIScrollbarInnerTexture = ModAsset.VerticalScrollbar.Value;
			WheelValue = wheelValue;

			inner = new UIImage(ModAsset.VerticalScrollbarInner.Value, Color.White);
			inner.Info.Width.SetValue(0f, 0.75f);
			Register(inner);
		}

		public override void Calculation()
		{
			base.Calculation();
			inner.Info.Left.SetValue(-(inner.Info.Size.X - Info.Size.X) / 2f, 0f);
			var t = ModAsset.VerticalScrollbarInner.Value;
			inner.Info.Height.Pixel = t.Height / (float)t.Width * inner.Info.Size.X;
			base.Calculation();
		}

		public override void LoadEvents()
		{
			base.LoadEvents();
			Events.OnLeftDown += element =>
			{
				if (!isMouseDown)
				{
					isMouseDown = true;
				}
			};
			Events.OnLeftUp += element =>
			{
				isMouseDown = false;
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
				if ((isMouseHover || isMouseDown) && alpha < 1f)
				{
					alpha += LightSpeed;
				}

				if ((!(isMouseHover || isMouseDown)) && alpha > 0f)
				{
					alpha -= LightSpeed;
				}
			}

			inner.ChangeColor(Color.White * alpha * Alpha);

			MouseState state = Mouse.GetState();
			float height = Info.Size.Y - TopMin - TopMax - inner.Info.Size.Y;
			if (!isMouseHover)
			{
				whell = state.ScrollWheelValue;
			}

			if (UseScrollWheel && isMouseHover && whell != state.ScrollWheelValue)
			{
				WheelValue -= (state.ScrollWheelValue - whell) / 6f / height * WheelValueMult;
				whell = state.ScrollWheelValue;
			}
			if (isMouseDown && mouseY != Main.mouseY)
			{
				WheelValue = (Main.mouseY - Info.Location.Y - TopMin - inner.Info.Size.Y / 2f) / height;
				mouseY = Main.mouseY;
			}

			inner.Info.Top.Pixel = TopMin + WheelValue * height;
			wheelValue += (waitToWheelValue - wheelValue) / 6f;

			if (waitToWheelValue != wheelValue)
			{
				Calculation();
			}
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			int cTopHeight = (int)(TOP_HEIGHT * Scale), cButtomHeight = (int)(BUTTOM_HEIGHT * Scale);

			sb.Draw(UIScrollbarInnerTexture, new Rectangle(
				Info.HitBox.X,
				Info.HitBox.Y, Info.HitBox.Width, cTopHeight),
				new Rectangle(0, 0, UIScrollbarInnerTexture.Width, TOP_HEIGHT),
				Color.White * MathHelper.SmoothStep(0, 1, alpha));

			sb.Draw(UIScrollbarInnerTexture, new Rectangle(
				Info.HitBox.X,
				Info.HitBox.Y + cTopHeight, Info.HitBox.Width, Info.HitBox.Height - cTopHeight - cButtomHeight),
				new Rectangle(0, TOP_HEIGHT, UIScrollbarInnerTexture.Width,
				UIScrollbarInnerTexture.Height - TOP_HEIGHT - BUTTOM_HEIGHT),
				Color.White * MathHelper.SmoothStep(0, 1, alpha));

			sb.Draw(UIScrollbarInnerTexture, new Rectangle(
				Info.HitBox.X,
				Info.HitBox.Y - cButtomHeight + Info.HitBox.Height, Info.HitBox.Width, cButtomHeight),
				new Rectangle(0, UIScrollbarInnerTexture.Height - BUTTOM_HEIGHT,
				UIScrollbarInnerTexture.Width, BUTTOM_HEIGHT),
				Color.White * MathHelper.SmoothStep(0, 1, alpha));
		}
	}
}