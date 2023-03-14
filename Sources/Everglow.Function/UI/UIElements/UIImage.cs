namespace Everglow.Commons.UI.UIElements;

public class UIImage : BaseElement
{
	/// <summary>
	/// 计算实际位置时的方式
	/// </summary>
	public enum CalculationStyle
	{
		/// <summary>
		/// 默认
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

	private Texture2D _texture;
	private Color _color;
	public CalculationStyle Style = CalculationStyle.None;

	public UIImage(Texture2D texture, Color color)
	{
		_texture = texture;
		_color = color;
		Info.Width.Pixel = texture.Width;
		Info.Height.Pixel = texture.Height;
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		base.DrawSelf(sb);
		sb.Draw(_texture, Info.TotalHitBox, _color);
	}

	public void ChangeColor(Color color) => _color = color;

	public override void Calculation()
	{
		if (Style == CalculationStyle.LockAspectRatioMainWidth)
		{
			float aspectRatio = _texture.Width / (float)_texture.Height;
			Info.Height.Pixel = Info.Width.Pixel / aspectRatio;
			Info.Height.Percent = Info.Width.Percent / aspectRatio * Main.screenWidth / Main.screenHeight;
		}
		else if (Style == CalculationStyle.LockedAspectRatioMainHeight)
		{
			float aspectRatio = _texture.Width / (float)_texture.Height;
			Info.Width.Pixel = Info.Height.Pixel * aspectRatio;
			Info.Width.Percent = Info.Height.Percent / aspectRatio / Main.screenWidth * Main.screenHeight;
		}
		base.Calculation();
	}

	public void ChangeImage(Texture2D texture) => _texture = texture;

	public Texture2D GetImage() => _texture;

	public Color GetColor() => _color;
}