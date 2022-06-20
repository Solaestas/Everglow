using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core.Enumerator
{
    internal static class EnumeratorExtension
    {
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            var it = values.GetEnumerator();
            while(it.MoveNext())
            {
                action(it.Current);
            }
        }
        public static void SetValues<T>(this IReadWriteEnumerable<T> values, Func<T,T> getValue)
        {
            var it = values.GetEnumerator();
            while(it.MoveNext())
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
    }
}
