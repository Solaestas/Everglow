using System.Collections;

namespace Everglow.ZY.Commons.Core.Enumerator;

public interface IReadWriteEnumerator<T> : IEnumerator<T>
{
	new ref T Current { get; }

	T IEnumerator<T>.Current => Current;

	object IEnumerator.Current => Current;
}
