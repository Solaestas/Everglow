﻿using System.Collections;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core.Enumerator
{
    public interface IReadWriteEnumerable<T> : IEnumerable<T>
    {
        new IReadWriteEnumerator<T> GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
