using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv
{
    public interface IArrayValueUniquenessChecker
    {
        bool HasDuplicates(IEnumerable<string> values);
    }

    public class ArrayValueUniquenessChecker : IArrayValueUniquenessChecker
    {
        public bool HasDuplicates(IEnumerable<string> values)
        {
            if (values.Any() == false) return false;

            var uniqueValues = values.Distinct();
            return values.Count() != uniqueValues.Count();
        }
    }
}