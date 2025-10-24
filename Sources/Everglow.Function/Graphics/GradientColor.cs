namespace Everglow.Commons.Graphics;

/// <summary>
/// 渐变色
/// <para> 例：</para>
/// <para> GradientColor g = new();</para>
/// <para> g.colorList.Add((Color.White, 0f));</para>
/// <para> g.colorList.Add((Color.Red, 1f));</para>
/// </summary>
public class GradientColor
{
	public List<(Color, float)> colorList = new();

	public Color GetColor(float position)
	{
		if (position <= 0 || position < colorList[0].Item2)
		{
			return colorList[0].Item1;
		}
		else if (position >= 1 || position > colorList[colorList.Count - 1].Item2)
		{
			return colorList[colorList.Count - 1].Item1;
		}
		else
		{
			for (int i = 0; i < colorList.Count - 1; i++)
			{
				if (position >= colorList[i].Item2 && position < colorList[i + 1].Item2)
				{
					float lerpValue = (position - colorList[i].Item2) / (colorList[i + 1].Item2 - colorList[i].Item2);
					return Color.Lerp(colorList[i].Item1, colorList[i + 1].Item1, lerpValue);
				}
			}
			return Color.Black;
		}
	}

	public static implicit operator GradientColor(Color color)
	{
		GradientColor gradientColor = new();
		gradientColor.colorList.Add((color, 0f));
		gradientColor.colorList.Add((color, 1f));
		return gradientColor;
	}
}