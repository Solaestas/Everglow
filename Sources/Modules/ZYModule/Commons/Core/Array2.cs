using Everglow.Sources.Modules.ZYModule.Commons.Core.Enumerator;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core
{
    public class Array2<T> : IReadWriteEnumerable<T>
    {
        public T valueA, valueB;
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
        public (T, T) Tuple => (valueA, valueB);
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

        public IReadWriteEnumerator<T> GetEnumerator()
        {
            return new ReadWriterEnumerator(this);
        }

        private class ReadWriterEnumerator : IReadWriteEnumerator<T>
        {
            private Array2<T> array;
            private int index;
            public ReadWriterEnumerator(Array2<T> array)
            {
                this.array = array;
                index = -1;
            }
            public ref T Current
            {
                get
                {
                    switch (index)
                    {
                        case -1: throw new InvalidOperationException();
                        case 0: return ref array.valueA;
                        case 1: return ref array.valueB;
                        default: throw new IndexOutOfRangeException();
                    }
                }
            }

            public void Dispose()
            {
                array = null;
            }

            public bool MoveNext()
            {
                ++index;
                return index <= 1;
            }

            public void Reset()
            {
                index = 0;
            }
        }
    }
}
