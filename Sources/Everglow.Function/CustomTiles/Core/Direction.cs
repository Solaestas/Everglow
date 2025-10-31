namespace Everglow.Commons.CustomTiles.Core;

[Flags]
public enum Direction : byte
{
	None = 0,
	Top = 1,
	Left = 2,
	Right = 4,
	Bottom = 8,
	Inside = 16,
	TopLeft = Top | Left,
	TopRight = Top | Right,
	BottomLeft = Bottom | Left,
	BottomRight = Bottom | Right,
}