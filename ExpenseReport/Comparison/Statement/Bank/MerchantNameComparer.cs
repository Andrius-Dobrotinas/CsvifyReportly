using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class MerchantNameComparer : IMerchantNameComparer
    {
        private readonly IMerchanNameMapComparer nameMapComparer;

        public MerchantNameComparer(IMerchanNameMapComparer nameMapComparer)
        {
            this.nameMapComparer = nameMapComparer;
        }

        // Take this as a constructor parameter
        private const string paypalDescriptionPrefix = "PAYPAL *";
        private static int paypalDescriptionPrefixLength = paypalDescriptionPrefix.Length;

        public bool DoStatementDetailsReferToMerchant(string statementDetails, string merchant, bool isViaPayPal)
        {
            if (isViaPayPal)
            {
                if (!IsPayPalTransaction(statementDetails))
                    return false;

                var actualValue = statementDetails.Substring(paypalDescriptionPrefixLength);

                return string.Equals(
                    actualValue,
                    merchant,
                    StringComparison.InvariantCultureIgnoreCase);
            }

            if (nameMapComparer.IsMatch(merchant, statementDetails))
                return true;

            return string.Equals(statementDetails, merchant, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsPayPalTransaction(string details)
        {
            return details != null
                    && details.StartsWith(paypalDescriptionPrefix);
        }
    }
}