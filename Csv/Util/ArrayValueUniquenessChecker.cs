using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv
{
    public interface IArrayValueUniquenessChecker
    {
        bool HasDuplicates(IEnumerable<string> values, out string[] nonUniqueColumnNames);
    }

    public class ArrayValueUniquenessChecker : IArrayValueUniquenessChecker
    {
        public bool HasDuplicates(IEnumerable<string> values, out string[] nonUniqueColumnNames)
        {
            if (values.Any() == false)
            {
                nonUniqueColumnNames = null;
                return false;
            }

            var uniqueValues = values.Distinct();
            if (values.Count() == uniqueValues.Count())
            {
                nonUniqueColumnNames = null;
                return false;
            }
            else
            {
                nonUniqueColumnNames = values
                    .GroupBy(x => x)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToArray();

                return true;
            }
        }
    }
}