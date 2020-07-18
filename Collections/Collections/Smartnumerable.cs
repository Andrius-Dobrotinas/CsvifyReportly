using System;
using System.Collections.Generic;

namespace Andy.Collections
{
    /// <inheritdoc/>
    public class Smartnumerable<TValue> : ISmartnumerable<TValue>
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