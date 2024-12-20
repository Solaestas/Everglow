namespace Everglow.Commons.UI.UIElements
{
	public class UIImage : BaseElement
	{
		/// <summary>
		/// 计算实际大小时的方式
		/// </summary>
		public enum CalculationStyle
		{
			/// <summary>
			/// 不计算
			/// </summary>
			None,

			/// <summary>
			/// 锁定宽高比，以宽为参考轴
			/// </summary>
			LockAspectRatioMainWidth,

			/// <summary>
			/// 锁定宽高比，以高为参考轴
			/// </summary>
			LockedAspectRatioMainHeight,
		}

		protected Texture2D _texture;
		protected Color _color;
		public CalculationStyle Style = CalculationStyle.None;
		public Vector2 Origin = Vector2.Zero;
		public SpriteEffects SpriteEffects = SpriteEffects.None;
		public float Rotation = 0f;
		public Rectangle? SourceRectangle = null;

		public Texture2D Texture { get => _texture; set => _texture = value; }

		public Color Color { get => _color; set => _color = value; }

		public UIImage(Texture2D texture, Color color)
		{
			_texture = texture;
			_color = color;
			if (texture != null)
			{
				Info.Width.Pixel = texture.Width;
				Info.Height.Pixel = texture.Height;
			}
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			base.DrawSelf(sb);
			if (_texture != null)
			{
				sb.Draw(_texture, Info.TotalHitBox, SourceRectangle,
					_color, Rotation, Origin, SpriteEffects, 0f);
			}
		}

		public override void Calculation()
		{
			base.Calculation();
			if (Style == CalculationStyle.LockAspectRatioMainWidth)
			{
				float aspectRatio = (float)_texture.Width / (float)_texture.Height;
				Info.Height.Pixel = Info.Size.X / aspectRatio;
				base.Calculation();
			}
			else if (Style == CalculationStyle.LockedAspectRatioMainHeight)
			{
				float aspectRatio = (float)_texture.Width / (float)_texture.Height;
				Info.Width.Pixel = Info.Size.Y * aspectRatio;
				base.Calculation();
			}
		}
	}
}