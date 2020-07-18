using System;
using System.Collections.Generic;

namespace Andy.Collections
{
    /// <summary>
    /// An enumerable that uses <see cref="Smartnumerator{T}"/> type of enumerator 
    /// that allows to enumerate enumerables that are expensive to enumerate
    /// (or only enumerate once) multiple times without the original cost on
    /// subsequent enumerations.
    /// Refer to the documentation of <see cref="Smartnumerator{T}"/> for more details
    /// </summary>
    public interface ISmartnumerable<TValue> : IEnumerable<TValue>
    {
    }
}