using System.Runtime.CompilerServices;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.Commons.Utilities.BackgroundHelper;

/// <summary>
/// unsafe+闭包Lambda 高性能顶点生成（性能敏感专用）
/// </summary>
public static unsafe class TileVertexRenderer
{
	// 预计算旋转偏移（全局静态，仅初始化一次）
	private static readonly Vector2[] _rotOffsets = new[]
	{
		new Vector2(0, -24),
		new Vector2(0, 24),
	};

	/// <summary>
	/// 行优先TriangleStrip渲染，闭包捕获不变量，unsafe指针写入顶点
	/// </summary>
	public static void Add_TileBgVertice_UnsafeLambda(
		BackgroundSlideBase bg,
		List<Point> tiles,
		List<Vertex2D> bars)
	{
		// ========== 逐帧仅计算一次【不变量】，全部捕获进Lambda闭包 ==========
		Vector2 screenHalf = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		Vector2 screenCenter = Main.screenPosition + screenHalf;
		Vector2 texSize = bg.Texture.Size();
		Vector2 texMoveBase = (screenCenter - bg.WorldAnchor) / bg.Distance / texSize;
		Vector2 screenPos = Main.screenPosition;

		// 提前扩容List，避免unsafe写入时数组越界（预估顶点数，可按需放大）
		bars.Capacity = Math.Max(bars.Capacity, tiles.Count * 6 + 256);

		// ========== Lambda闭包：捕获所有不变量，消除传参开销 ==========
		Action<Vector2> addVertex = (Vector2 pos) =>
		{
			// 闭包自动访问外层预计算变量，无参数传递
			Vector2 screenOffset = pos - screenCenter;
			Vector2 uv = screenOffset / texSize + texMoveBase;
			uv.X += 0.5f;
			uv.Y += 0.5f;

			Color drawColor = GetColor(bg, pos);
			Vector3 uv3 = new Vector3(uv.X, uv.Y, 0f);
			Vertex2D vtx = new Vertex2D(pos - screenPos, drawColor, uv3);

			// ========== unsafe 直接操作List底层数组，零开销写入 ==========
			ref Vertex2D[] itemArray = ref GetItems(bars);
			ref int size = ref GetSize(bars);

			// 直接写入，跳过List.Add安全检查
			itemArray[size] = vtx;
			size++;
		};

		Action<Vector2> nextRow = (Vector2 pos) =>
		{
			ref Vertex2D[] itemArray = ref GetItems(bars);
			ref int size = ref GetSize(bars);
			ref Vertex2D lastRef = ref itemArray[size - 1];
			Vertex2D degenerate = lastRef;
			degenerate.color = Color.Transparent;
			ref Vertex2D degRef0 = ref itemArray[size];
			degRef0 = degenerate;
			degRef0.position += new Vector2(0, -48);
			size++;
			ref Vertex2D degRef1 = ref itemArray[size];
			degRef1 = degenerate;
			size++;
			Vector2 finalPos = pos + new Vector2(24, 24) - Main.screenPosition;
			ref Vertex2D degRef2 = ref itemArray[size];
			degRef2 = degenerate;
			degRef2.position = finalPos + new Vector2(0, -24);
			size++;
			ref Vertex2D degRef3 = ref itemArray[size];
			degRef3.position = finalPos + new Vector2(0, 24);
			size++;
		};

		// ========== 视口过滤+行排序（和之前逻辑一致） ==========
		var visibleTiles = tiles
			.Select(p => new { Pos = p, World = p.ToWorldCoordinates() })
			.Where(t => VFXManager.InScreen(t.World, 64))
			.OrderBy(t => t.Pos.Y)
			.ThenBy(t => t.Pos.X)
			.ToList();

		if (visibleTiles.Count == 0)
		{
			return;
		}

		int lastY = visibleTiles[0].Pos.Y;

		foreach (var tile in visibleTiles)
		{
			Vector2 basePos = tile.World;
			if (tile.Pos.Y != lastY)
			{
				// 退化顶点：复用最后一个顶点，透明衔接上下行
				nextRow(basePos);
				lastY = tile.Pos.Y;
			}

			// 调用闭包写入顶点，无参数传递开销
			foreach (var offset in _rotOffsets)
			{
				addVertex(basePos + offset + new Vector2(24, 24));
			}
		}
	}

	private static Color GetColor(BackgroundSlideBase bg, Vector2 worldPos)
	{
		Color c = Color.White;
		switch (bg.UseColorStyle)
		{
			case 0:
				c = Main.ColorOfTheSkies;
				break;
			case 1:
				c = Lighting.GetColor((worldPos + new Vector2(-8)).ToTileCoordinates());
				break;
		}
		return c * bg.Alpha;
	}

	// Define an accessor to get/set the private '_items' array
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
	private static extern ref T[] GetItems<T>(List<T> list);

	// Define an accessor to get/set the private '_size' field
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_size")]
	private static extern ref int GetSize<T>(List<T> list);
}