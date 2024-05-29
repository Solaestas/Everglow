using ReLogic.Graphics;

namespace Everglow.Commons.UI.UIElements
{
	public class UIText : BaseElement
	{
		private string text;

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				Calculation();
			}
		}

		private DynamicSpriteFont font;
		public Color Color;

		/// <summary>
		/// 绘制大小，不改变部件碰撞箱，不改变绘制中心
		/// </summary>
		public float Scale;

		public bool CalculateSize = true;
		public PositionStyle? CenterX;
		public PositionStyle? CenterY;

		public UIText(string t, DynamicSpriteFont spriteFont, float scale = 1f)
		{
			text = t;
			font = spriteFont;
			Scale = scale;
			Color = Color.White;
		}

		public UIText(string t, DynamicSpriteFont spriteFont, Color textColor, float scale = 1f)
		{
			text = t;
			font = spriteFont;
			Scale = scale;
			Color = textColor;
		}

		public override void Calculation()
		{
			if (CalculateSize)
			{
				Vector2 size = font.MeasureString(text) * Scale;
				Info.Width.Pixel = size.X;
				Info.Height.Pixel = size.Y;
				Info.Width.Percent = 0f;
				Info.Height.Percent = 0f;

				if (CenterX != null && CenterY != null)
				{
					var x = CenterX.Value;
					var y = CenterY.Value;
					Info.Left.Percent = x.Percent;
					Info.Top.Percent = y.Percent;
					Info.Left.Pixel = x.Pixel - Info.Width.Pixel / 2f;
					Info.Top.Pixel = y.Pixel - Info.Height.Pixel / 2f;
				}
			}
			base.Calculation();
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			base.DrawSelf(sb);
			sb.DrawString(font, text, Info.Location, Color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
		}
	}
}