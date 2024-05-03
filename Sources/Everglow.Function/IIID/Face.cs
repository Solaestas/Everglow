using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Everglow.Commons.IIID;

public struct Face
{
	/// <summary>
	/// [0] = Normals<br/>
	/// [1] = Positions<br/>
	/// [2] = TextureCoords<br/>
	/// </summary>
	private Array3<Array3<int>> _data;

	public readonly Array3<int> Normals => _data[0];

	public readonly Array3<int> Positions => _data[1];

	public readonly Array3<int> TextureCoords => _data[2];

	[InlineArray(3)]
	public struct Array3<T> : ISpanParsable<Array3<T>>
		where T : ISpanParsable<T>
	{
		public T Item;

		/// <summary>
		/// 将x/y/z格式的字符串解析为Array3
		/// </summary>
		/// <param name="s"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		public static Array3<T> Parse(ReadOnlySpan<char> s, IFormatProvider provider = null)
		{
			int i = s.IndexOf('/');
			int j = i + 1 + s[(i + 1)..].IndexOf('/');
			Array3<T> ints = default;
			ints[0] = T.Parse(s[..i], provider);
			ints[1] = T.Parse(s[(i + 1)..j], provider);
			ints[2] = T.Parse(s[(j + 1)..], provider);
			return ints;
		}

		public static Array3<T> Parse(string s, IFormatProvider provider = null) => Parse(s.AsSpan());

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, [MaybeNullWhen(false)] out Array3<T> result) => throw new NotImplementedException();

		public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out Array3<T> result) => throw new NotImplementedException();
	}

	/// <summary>
	/// 传入的值为法线，坐标，UV组成的数组
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public Array3<int> this[int index]
	{
		set
		{
			_data[0][index] = value[0];
			_data[1][index] = value[1];
			_data[2][index] = value[2];
		}
	}
}