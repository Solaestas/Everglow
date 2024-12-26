using Everglow.Commons.UI.StringDrawerSystem;

namespace Everglow.Commons.UI.UIElements
{
	public class UITextPlus : BaseElement
	{
		public StringDrawer StringDrawer { get; init; }

		private string _text;

		/// <summary>
		/// 显示的文本内容
		/// <para/>注: 被赋值时自动调用<see cref="StringDrawer"/>的<see cref="StringDrawer.Init(string)"/>方法
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}

			set
			{
				_text = value;
				StringDrawer.Init(_text);
			}
		}

		/// <summary>
		/// 绘制大小，不改变部件碰撞箱，不改变绘制中心
		/// </summary>
		[Obsolete("The Scale property has been deprecated, it will affect nothing", true)]
		public float Scale { get; private set; }

		/// <summary>
		/// 是否自动修改Size为<see cref="StringDrawer"/>的Size
		/// </summary>
		public bool CalculateSize { get; private set; } = true;

		/// <summary>
		/// 自定义中心位置的X坐标
		/// </summary>
		public PositionStyle? CenterX { get; private set; }

		/// <summary>
		/// 自定义中心位置的Y坐标
		/// </summary>
		public PositionStyle? CenterY { get; private set; }

		public UITextPlus(string t)
		{
			StringDrawer = new StringDrawer();
			Text = t;

			Info.Width.Pixel = StringDrawer.Size.X;
			Info.Height.Pixel = StringDrawer.Size.Y;
		}

		public override void Calculation()
		{
			if (CalculateSize)
			{
				Info.Width.Pixel = StringDrawer.Size.X;
				Info.Height.Pixel = StringDrawer.Size.Y;
				Info.Width.Percent = 0f;
				Info.Height.Percent = 0f;
			}
			if (CenterX != null && CenterY != null)
			{
				var x = CenterX.Value;
				var y = CenterY.Value;
				Info.Left.Percent = x.Percent;
				Info.Top.Percent = y.Percent;
				Info.Left.Pixel = x.Pixel - Info.Width.Pixel / 2f;
				Info.Top.Pixel = y.Pixel - Info.Height.Pixel / 2f;
			}
			base.Calculation();
			StringDrawer.SetPosition(Info.Location);
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			base.DrawSelf(sb);
			StringDrawer.Draw(sb);
		}
	}
}