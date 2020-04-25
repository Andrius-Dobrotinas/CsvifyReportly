using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class MerchanNameVariationComparer : IMerchanNameVariationComparer
    {
        private readonly IDictionary<string, string[]> nameMap;

        public MerchanNameVariationComparer(IDictionary<string, string[]> nameMap)
        {
            this.nameMap = nameMap;
        }

        public bool IsMatch(string merchantName, string statementDetailsString)
        {
            string[] variations;
            if (nameMap.TryGetValue(merchantName, out variations))
            {
                if (variations.Any(
                    name => statementDetailsString.StartsWith(
                        name,
                        StringComparison.InvariantCultureIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}