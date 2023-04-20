using System.Collections;

namespace Everglow.Commons.TileHelper;

public interface ITileAccessor : IEnumerator<Tile>
{
	/// <summary>
	/// 用来进行多格移动的从0开始的下标
	/// </summary>
	int Index { get; set; }

	/// <summary>
	/// 当前物块的坐标
	/// </summary>
	Point CurrentCoord { get; }

	/// <summary>
	/// 判断当前访问器是否在范围内
	/// </summary>
	bool Good { get; }

	/// <summary>
	/// 向后移动一格，并返回移动后状态
	/// </summary>
	/// <returns></returns>
	bool MovePrevious();

	object IEnumerator.Current => Current;
}
