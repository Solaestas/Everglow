using System.Collections;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core
{
    public class Array2<T> : IEnumerable<T>
    {
        public T valueA, valueB;
        public Array2()
        {
            valueA = default;
            valueB = default;
        }
        public Array2(T valueA, T valueB)
        {
            this.valueA = valueA;
            this.valueB = valueB;
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return valueA;
            yield return valueB;
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                Debug.Assert(index <= 1);
                return index == 0 ? valueA : valueB;
            }
            set
            {
                Debug.Assert(index <= 1);
                if (index == 0)
                {
                    valueA = value;
                }
                else
                {
                    valueB = value;
                }
            }
        }
    }
}
