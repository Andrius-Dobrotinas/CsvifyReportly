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
    public class Smartnumerable<TValue> : IEnumerable<TValue>
    {
        private readonly Smartnumerator<TValue> enumerator;

        public Smartnumerable(Smartnumerator<TValue> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return enumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return enumerator;
        }
    }

    public static class Smartnumerable
    {
        /// <summary>
        /// Creates a <see cref="Smartnumerable{TValue}"/> that wraps the supplied enumerable
        /// </summary>
        public static Smartnumerable<TValue> Create<TValue>(IEnumerable<TValue> source)
        {
            return new Smartnumerable<TValue>(
                new Smartnumerator<TValue>(source.GetEnumerator()));
        }
    }
}