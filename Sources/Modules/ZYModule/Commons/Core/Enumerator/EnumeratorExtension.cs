using System.Text;

namespace Everglow.ZYModule.Commons.Core.Enumerator;

internal static class EnumeratorExtension
{
	public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
	{
		var it = values.GetEnumerator();
		while (it.MoveNext())
		{
			action(it.Current);
		}
	}
	public static void SetValues<T>(this IReadWriteEnumerable<T> values, Func<T, T> getValue)
	{
		var it = values.GetEnumerator();
		while (it.MoveNext())
		{
			it.Current = getValue(it.Current);
		}
	}
	public static void ResetValues<T>(this IReadWriteEnumerable<T> values)
	{
		var it = values.GetEnumerator();
		while (it.MoveNext())
		{
			it.Current = default;
		}
	}

	public static string BuildString(this IEnumerable<char> chars)
	{
		var stringBuilder = new StringBuilder();
		foreach (var ch in chars)
		{
			stringBuilder.Append(ch);
		}
		return stringBuilder.ToString();
	}
	public static string BuildString(this IEnumerable<string> chars)
	{
		var stringBuilder = new StringBuilder();
		foreach (var ch in chars)
		{
			stringBuilder.Append(ch);
		}
		return stringBuilder.ToString();
	}
}
