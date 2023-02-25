namespace Everglow.Commons.CustomTile;

public enum Direction : byte
{
	None = 0,
	Top = 1,
	Left = 2,
	Right = 4,
	Bottom = 8,
	TopLeft = Top | Left,
	TopRight = Top | Right,
	BottomLeft = Bottom | Left,
	BottomRight = Bottom | Right,
	Inside = 16
}
