using System.Runtime.CompilerServices;
using Everglow.Commons.Vertex;

public static class ListAccessor
{
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
	public static extern ref Vertex2D[] GetItems(List<Vertex2D> list);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_size")]
	public static extern ref int GetSize(List<Vertex2D> list);
}