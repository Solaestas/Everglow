using Everglow.Commons.UI.StringDrawerSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Commons.UI.UIElements
{
	public class UITextPlus : BaseElement
	{
		public StringDrawer StringDrawer;
		private string text;

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				StringDrawer.Init(text);
			}
		}

		/// <summary>
		/// 绘制大小，不改变部件碰撞箱，不改变绘制中心
		/// </summary>
		public float Scale;

		public bool CalculateSize = true;
		public PositionStyle? CenterX;
		public PositionStyle? CenterY;

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