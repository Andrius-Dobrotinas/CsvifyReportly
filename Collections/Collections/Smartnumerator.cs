using System;
using System.Collections;
using System.Collections.Generic;

namespace Andy.Collections
{
    /// <summary>
    /// Enumerates a given enumerable once, while making a copy of it as it goes.
    /// Subsequent enumerations don't use the original enumerable.
    /// In cases where the initial enumeration is stopped before the end of the original 
    /// enumerable is reached, serves the "cached" values, and then resumes the
    /// enumeration of the original enumerable where it first left off.
    /// Primarily intended for enumerating enumerables that are expensive to enumerate
    /// (or would only enumerate once), and which the user might want to enumerate more
    /// than once.
    /// This effectively serves the same function as simply calling ToList/ToArray on an
    /// enumerable like that, except:
    /// 1) this is seemless and therefore, by definition, cooler and/or fancier
    /// 2) in cases where the enumerable is not enumerated to the end each time (for
    /// example, calling IsAny first, and then maybe taking the first few items), the
    /// initial few enumerations would happen faster due to not having to wait for the
    /// whole thing to get enumerated with ToList/ToArray first.
    /// </summary>
    public class Smartnumerator<T> : IEnumerator<T> where T : class
    {
        private readonly EnumerableCopyManager<T> copyManager;
        private readonly IEnumerator<T> source;

        private bool isEndOfTheLine = false;

        public Smartnumerator(IEnumerator<T> source)
        {
            this.source = source;
            this.copyManager = new EnumerableCopyManager<T>(new List<T>());
        }

        public T Current => copyManager.Current;
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
            copyManager.Reset();
        }

        public bool MoveNext()
        {
            if (copyManager.IsEndOfTheLine)
            {
                if (isEndOfTheLine)
                    return false;

                var hasAny = source.MoveNext();
                if (hasAny)
                    copyManager.Add(source.Current);
                else
                    isEndOfTheLine = true;

                return hasAny;
            }
            else
            {
                copyManager.AdvancePosition();
                return true;
            }
        }

        public void Reset()
        {
            //it's unclear what this method is actually for

            source.Reset();
        }
    }
}