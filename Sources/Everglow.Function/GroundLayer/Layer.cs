using ReLogic.Content;

namespace Everglow.Commons.GroundLayer;

public struct Layer : IEquatable<Layer>
{
	public Layer(Asset<Texture2D> texture)
	{
		id = nextID++;
		Texture = texture;
	}

	public Vector3 Position { get; set; }

	public Asset<Texture2D> Texture { get; init; }

	public static bool operator !=(Layer left, Layer right)
	{
		return !(left == right);
	}

	public static bool operator ==(Layer left, Layer right)
	{
		return left.Equals(right);
	}

	public readonly bool Equals(Layer other)
	{
		return id == other.id;
	}

	public override readonly bool Equals(object obj)
	{
		return obj is Layer layer && Equals(layer);
	}

	public override readonly int GetHashCode()
	{
		return id;
	}

	private static int nextID = int.MinValue;

	private readonly int id;
}