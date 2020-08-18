using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Comparison.String
{
    public class StringValueComparerResolver
    {
        public static IStringComparer Build(ValueComparisonMethod method, StringComparison mode)

        {    
            switch (method)
            {
                case ValueComparisonMethod.OneToOneMatch:
                    return new StraightComparer(mode);
                case ValueComparisonMethod.StartsWith:
                    return new StartsWithComparer(mode);
                case ValueComparisonMethod.Contains:
                    return new ContainsComparer(mode);
                case ValueComparisonMethod.EndsWith:
                    return new EndsWithComparer(mode);
                default:
                    throw new NotImplementedException($"{nameof(ValueComparisonMethod)} value {method.ToString()}");
            }
        }
    }
}