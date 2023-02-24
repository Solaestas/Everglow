namespace Everglow.ZYModule.Commons.Function.MapIO;

internal class TileAccessor : ITileAccessor
{
	private readonly int minX;
	private readonly int minY;
	private readonly int maxX;
	private readonly int maxY;
	private int x;
	private int y;

	public TileAccessor(int minX, int minY, int maxX, int maxY)
	{
		x = this.minX = minX;
		y = this.minY = minY;
		--x;
		this.maxX = maxX;
		this.maxY = maxY;
	}

	public Point CurrentCoord => new(x, y);
	public Tile Current => Good ? Main.tile[x, y] : throw new InvalidOperationException();

	public bool Good => minX <= x && x < maxX && minY <= y && y < maxY;

	public int Index
	{
		get
		{
			return x - minX + (y - minY) * (maxX - minX);
		}
		set
		{
			x = value / (maxX - minX) + minX;
			y = value % (maxX - minX) + minY;
		}
	}

	public void Dispose() { }

	public bool MoveNext()
	{
		++x;
		if (x >= maxX)
		{
			x = minX;
			++y;
		}
		return y < maxY;
	}

	public bool MovePrevious()
	{
		--x;
		if (x < minX)
		{
			x = maxX;
			--y;
		}
		return y > minY;
	}

	public void Reset()
	{
		x = minX - 1;
		y = minY;
	}
}
